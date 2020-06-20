using System.Collections.Generic;
using UnityEngine;

namespace NeoCasual.GoingHyper.MeshSlice
{
    public class MeshPiece
    {
        public List<Vector3> Vertices;
        public List<Vector3> Normals;
        public List<Vector2> UVs;
        public List<int> Triangles;
        public float SurfaceArea;

        private Dictionary<int, int> _indicesMappings;

        public MeshPiece (int vertexCapacity)
        {
            Vertices = new List<Vector3> (vertexCapacity);
            Normals = new List<Vector3> (vertexCapacity);
            UVs = new List<Vector2> (vertexCapacity);
            Triangles = new List<int> (vertexCapacity * 3);
            SurfaceArea = 0f;

            _indicesMappings = new Dictionary<int, int> (vertexCapacity);
        }

        public void Clear ()
        {
            Vertices.Clear ();
            Normals.Clear ();
            UVs.Clear ();
            Triangles.Clear ();
            SurfaceArea = 0f;

            _indicesMappings.Clear ();
        }

        private void AddPoint (Vector3 point, Vector3 normal, Vector2 uv)
        {
            Triangles.Add (Vertices.Count);

            Vertices.Add (point);
            Normals.Add (normal);
            UVs.Add (uv);
        }

        public void AddOriginalTriangle (int[] indices)
        {
            for (int i = 0; i < 3; ++i)
            {
                Triangles.Add (_indicesMappings[indices[i]]);
            }

            SurfaceArea += GetTriangleArea (Triangles.Count - 3);
        }

        public void AddSlicedTriangle (int indice1, Vector3 vertice2, Vector2 uv2, int indice3)
        {
            int v1 = _indicesMappings[indice1];
            int v3 = _indicesMappings[indice3];
            Vector3 normal = Vector3.Cross (vertice2 - Vertices[v1], Vertices[v3] - vertice2).normalized;

            Triangles.Add (v1);
            AddPoint (vertice2, normal, uv2);
            Triangles.Add (_indicesMappings[indice3]);

            SurfaceArea += GetTriangleArea (Triangles.Count - 3);
        }

        public void AddSlicedTriangle (int indice1, Vector3 vertice2, Vector3 vertice3, Vector2 uv2, Vector2 uv3)
        {
            int v1 = _indicesMappings[indice1];
            Vector3 normal = Vector3.Cross (vertice2 - Vertices[v1], vertice3 - vertice2).normalized;

            Triangles.Add (v1);
            AddPoint (vertice2, normal, uv2);
            AddPoint (vertice3, normal, uv3);

            SurfaceArea += GetTriangleArea (Triangles.Count - 3);
        }

        public void AddTriangle (Vector3[] points)
        {
            Vector3 normal = Vector3.Cross (points[1] - points[0], points[2] - points[1]).normalized;
            for (int i = 0; i < 3; ++i)
            {
                AddPoint (points[i], normal, Vector2.zero);
            }

            SurfaceArea += GetTriangleArea (Triangles.Count - 3);
        }

        public void ContainsKeys (List<int> triangles, int startIdx, bool[] isTrueCases)
        {
            for (int i = 0; i < 3; ++i)
            {
                isTrueCases[i] = _indicesMappings.ContainsKey (triangles[startIdx + i]);
            }
        }

        public void AddVertex (List<Vector3> originalVertices, List<Vector3> originalNormals, List<Vector2> originalUVs, int index)
        {
            _indicesMappings[index] = Vertices.Count;

            Vertices.Add (originalVertices[index]);
            Normals.Add (originalNormals[index]);
            UVs.Add (originalUVs[index]);
        }

        private float GetTriangleArea (int i)
        {
            Vector3 verticeA = Vertices[Triangles[i + 2]] - Vertices[Triangles[i]];
            Vector3 verticeB = Vertices[Triangles[i + 1]] - Vertices[Triangles[i]];

            float a = verticeA.magnitude;
            float b = verticeB.magnitude;
            float gamma = Mathf.Deg2Rad * Vector3.Angle (verticeB, verticeA);

            return a * b * Mathf.Sin (gamma) / 2;
        }
    }
}