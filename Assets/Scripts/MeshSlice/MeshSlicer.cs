using System.Collections.Generic;
using UnityEngine;

namespace NeoCasual.GoingHyper.MeshSlice
{
    public class MeshSlicer
    {
        private const float THRESHOLD = 1e-6f;
        private const int INITIAL_ARRAY_SIZE = 256;

        private readonly List<Vector3> _originalVertices;
        private readonly List<int> _originalTriangles;
        private readonly List<Vector3> _originalNormals;
        private readonly List<Vector2> _originalUVs;
        private readonly Vector3[] _intersectPairs;
        private readonly Vector3[] _tempTriangles;

        private List<Vector3> _addedPairs;
        private MeshIntersection _intersection;

        private MeshPiece _meshA;
        private MeshPiece _meshB;
        private MeshPiece _biggerMesh;
        private MeshPiece _smallerMesh;

        private Plane _slicePlane = new Plane ();

        public MeshSlicer ()
        {
            _originalVertices = new List<Vector3> (INITIAL_ARRAY_SIZE);
            _originalTriangles = new List<int> (INITIAL_ARRAY_SIZE * 3);
            _originalNormals = new List<Vector3> (INITIAL_ARRAY_SIZE);
            _originalUVs = new List<Vector2> (INITIAL_ARRAY_SIZE);
            _intersectPairs = new Vector3[2];
            _tempTriangles = new Vector3[3];

            _addedPairs = new List<Vector3> (INITIAL_ARRAY_SIZE);
            _intersection = new MeshIntersection ();

            _meshA = new MeshPiece (INITIAL_ARRAY_SIZE);
            _meshB = new MeshPiece (INITIAL_ARRAY_SIZE);
        }

        public void SliceObject (GameObject obj, float[] slicePoints)
        {
            Bounds objBounds = obj.GetComponent<MeshFilter> ().mesh.bounds;
            Vector3 objBottomLeft = obj.transform.position + objBounds.min;
            Vector3 objSize = objBounds.size;

            for (int i = 0; i < slicePoints.Length; i++)
            {
                Vector3 transformedNormal = ((Vector3) (obj.transform.localToWorldMatrix.transpose * Vector2.up)).normalized;
                Vector3 slicePoint = new Vector3 (
                    objBottomLeft.x, objBottomLeft.y + objSize.y * slicePoints[i], objBottomLeft.z
                );

                _slicePlane.SetNormalAndPosition (
                    transformedNormal, obj.transform.InverseTransformPoint (slicePoint)
                );

                Mesh mesh = obj.GetComponent<MeshFilter> ().mesh;
                if (!SliceMesh (mesh, ref _slicePlane))
                {
                    return;
                }

                bool posBigger = _meshA.SurfaceArea > _meshB.SurfaceArea;
                if (posBigger)
                {
                    _biggerMesh = _meshA;
                    _smallerMesh = _meshB;
                }
                else
                {
                    _biggerMesh = _meshB;
                    _smallerMesh = _meshA;
                }

                GameObject newObj = GameObject.Instantiate (obj, obj.transform.parent);
                newObj.transform.SetPositionAndRotation (obj.transform.position, obj.transform.rotation);
                Mesh newObjMesh = newObj.GetComponent<MeshFilter> ().mesh;

                ReplaceMesh (mesh, _biggerMesh);
                ReplaceMesh (newObjMesh, _smallerMesh);
            }
        }

        private void ReplaceMesh (Mesh mesh, MeshPiece tempMesh)
        {
            mesh.Clear ();

            mesh.SetVertices (tempMesh.Vertices);
            mesh.SetTriangles (tempMesh.Triangles, 0);
            mesh.SetNormals (tempMesh.Normals);
            mesh.SetUVs (0, tempMesh.UVs);

            mesh.RecalculateTangents ();
        }

        private bool SliceMesh (Mesh mesh, ref Plane slice)
        {
            mesh.GetVertices (_originalVertices);

            if (!MeshIntersection.BoundPlaneIntersect (mesh, ref slice))
            {
                return false;
            }

            mesh.GetTriangles (_originalTriangles, 0);
            mesh.GetNormals (_originalNormals);
            mesh.GetUVs (0, _originalUVs);

            _meshA.Clear ();
            _meshB.Clear ();
            _addedPairs.Clear ();

            for (int i = 0; i < _originalVertices.Count; ++i)
            {
                if (slice.GetDistanceToPoint (_originalVertices[i]) >= 0)
                {
                    _meshA.AddVertex (_originalVertices, _originalNormals, _originalUVs, i);
                }
                else
                {
                    _meshB.AddVertex (_originalVertices, _originalNormals, _originalUVs, i);
                }
            }

            if (_meshB.Vertices.Count == 0 || _meshA.Vertices.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < _originalTriangles.Count; i += 3)
            {
                if (_intersection.TrianglePlaneIntersect (_originalVertices, _originalUVs, _originalTriangles, i, ref slice, _meshA, _meshB, _intersectPairs))
                {
                    _addedPairs.AddRange (_intersectPairs);
                }
            }

            if (_addedPairs.Count > 0)
            {
                FillBoundaryFace (_addedPairs);
                return true;
            }

            return false;
        }

        private Vector3 GetFirstVertex ()
        {
            if (_originalVertices.Count != 0)
            {
                return _originalVertices[0];
            }

            return Vector3.zero;
        }

        private void FillBoundaryGeneral (List<Vector3> added)
        {
            MeshUtils.ReorderList (added);

            Vector3 center = MeshUtils.FindCenter (added);
            _tempTriangles[2] = center;

            for (int i = 0; i < added.Count; i += 2)
            {
                _tempTriangles[0] = added[i];
                _tempTriangles[1] = added[i + 1];
                _meshA.AddTriangle (_tempTriangles);

                _tempTriangles[0] = added[i + 1];
                _tempTriangles[1] = added[i];
                _meshB.AddTriangle (_tempTriangles);
            }
        }

        private void FillBoundaryFace (List<Vector3> added)
        {
            MeshUtils.ReorderList (added);

            List<Vector3> faces = FindRealPolygon (added);

            int traceForward = 0;
            int traceBackward = faces.Count - 1;
            int traceNew = 1;
            bool isTraceIncrement = true;

            while (traceNew != traceForward && traceNew != traceBackward)
            {
                AddTriangle (faces, traceBackward, traceForward, traceNew);

                if (isTraceIncrement)
                {
                    traceForward = traceNew;
                }
                else
                {
                    traceBackward = traceNew;
                }

                isTraceIncrement = !isTraceIncrement;
                traceNew = isTraceIncrement ? traceForward + 1 : traceBackward - 1;
            }
        }

        private List<Vector3> FindRealPolygon (List<Vector3> pairs)
        {
            List<Vector3> vertices = new List<Vector3> ();
            Vector3 edge1, edge2;

            for (int i = 0; i < pairs.Count; i += 2)
            {
                edge1 = (pairs[i + 1] - pairs[i]);

                if (i == pairs.Count - 2)
                {
                    edge2 = pairs[1] - pairs[0];
                }
                else
                {
                    edge2 = pairs[i + 3] - pairs[i + 2];
                }

                edge1.Normalize ();
                edge2.Normalize ();

                if (Vector3.Angle (edge1, edge2) > THRESHOLD)
                {
                    vertices.Add (pairs[i + 1]);
                }
            }

            return vertices;
        }

        private void AddTriangle (List<Vector3> face, int trace1, int trace2, int trace3)
        {
            _tempTriangles[0] = face[trace1];
            _tempTriangles[1] = face[trace2];
            _tempTriangles[2] = face[trace3];
            _meshA.AddTriangle (_tempTriangles);

            _tempTriangles[1] = face[trace3];
            _tempTriangles[2] = face[trace2];
            _meshB.AddTriangle (_tempTriangles);
        }
    }
}