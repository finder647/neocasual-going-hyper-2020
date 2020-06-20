using System.Collections.Generic;
using UnityEngine;

namespace NeoCasual.GoingHyper.MeshSlice
{
    public class MeshIntersection
    {
        private readonly Vector3[] _vs;
        private readonly Vector2[] _us;
        private readonly int[] _ts;
        private readonly bool[] _isPositives;

        private Ray _edgeRay;

        public MeshIntersection ()
        {
            _vs = new Vector3[3];
            _us = new Vector2[3];
            _ts = new int[3];
            _isPositives = new bool[3];
        }

        public static bool BoundPlaneIntersect (Mesh mesh, ref Plane plane)
        {
            float r = mesh.bounds.extents.x * Mathf.Abs (plane.normal.x) +
                mesh.bounds.extents.y * Mathf.Abs (plane.normal.y) +
                mesh.bounds.extents.z * Mathf.Abs (plane.normal.z);
            float s = Vector3.Dot (plane.normal, mesh.bounds.center) - (-plane.distance);

            return Mathf.Abs (s) <= r;
        }

        public System.ValueTuple<Vector3, Vector2> Intersect (Plane plane, Vector3 first, Vector3 second, Vector2 uv1, Vector2 uv2)
        {
            _edgeRay.origin = first;
            _edgeRay.direction = (second - first).normalized;

            plane.Raycast (_edgeRay, out float distance);
            float maxDistance = Vector3.Distance (first, second);
            float relativeDistance = distance / maxDistance;

            System.ValueTuple<Vector3, Vector2> returnValue = new System.ValueTuple<Vector3, Vector2>
            {
                Item1 = _edgeRay.GetPoint (distance)
            };

            returnValue.Item2.x = Mathf.Lerp (uv1.x, uv2.x, relativeDistance);
            returnValue.Item2.y = Mathf.Lerp (uv1.y, uv2.y, relativeDistance);

            return returnValue;
        }

        public bool TrianglePlaneIntersect (List<Vector3> vertices, List<Vector2> uvs, List<int> triangles, int startIndex, ref Plane plane, MeshPiece meshA, MeshPiece meshB, Vector3[] intersectVectors)
        {
            for (int i = 0; i < 3; ++i)
            {
                _ts[i] = triangles[startIndex + i];
                _vs[i] = vertices[_ts[i]];
                _us[i] = uvs[_ts[i]];
            }

            meshA.ContainsKeys (triangles, startIndex, _isPositives);

            if (_isPositives[0] == _isPositives[1] && _isPositives[1] == _isPositives[2])
            {
                (_isPositives[0] ? meshA : meshB).AddOriginalTriangle (_ts);

                return false;
            }

            int lonelyPoint = 0;
            if (_isPositives[0] != _isPositives[1])
            {
                lonelyPoint = _isPositives[0] != _isPositives[2] ? 0 : 1;
            }
            else
            {
                lonelyPoint = 2;
            }

            int prevPoint = lonelyPoint - 1;
            if (prevPoint == -1)
            {
                prevPoint = 2;
            }

            int nextPoint = lonelyPoint + 1;
            if (nextPoint == 3)
            {
                nextPoint = 0;
            }

            System.ValueTuple<Vector3, Vector2> newPointPrev = Intersect (plane, _vs[lonelyPoint], _vs[prevPoint], _us[lonelyPoint], _us[prevPoint]);
            System.ValueTuple<Vector3, Vector2> newPointNext = Intersect (plane, _vs[lonelyPoint], _vs[nextPoint], _us[lonelyPoint], _us[nextPoint]);

            (_isPositives[lonelyPoint] ? meshA : meshB).AddSlicedTriangle (_ts[lonelyPoint], newPointNext.Item1, newPointPrev.Item1, newPointNext.Item2, newPointPrev.Item2);
            (_isPositives[prevPoint] ? meshA : meshB).AddSlicedTriangle (_ts[prevPoint], newPointPrev.Item1, newPointPrev.Item2, _ts[nextPoint]);
            (_isPositives[prevPoint] ? meshA : meshB).AddSlicedTriangle (_ts[nextPoint], newPointPrev.Item1, newPointNext.Item1, newPointPrev.Item2, newPointNext.Item2);

            if (_isPositives[lonelyPoint])
            {
                intersectVectors[0] = newPointPrev.Item1;
                intersectVectors[1] = newPointNext.Item1;
            }
            else
            {
                intersectVectors[0] = newPointNext.Item1;
                intersectVectors[1] = newPointPrev.Item1;
            }

            return true;
        }
    }
}