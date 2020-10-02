using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

// Quelle: https://wiki.unity3d.com/index.php/ReverseNormals
// to invert normals of the sphere (inside texture)
public class InvertNormals : MonoBehaviour
{
    void Start()
    {
        Mesh mesh = this.GetComponent<MeshFilter>().mesh;
        Vector3[] normals = mesh.normals;
        for (int i = 0; i < normals.Length; i++)
            normals[i] = -1 * normals[i];
        mesh.normals = normals;
        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            int[] tris = mesh.GetTriangles(i);
            for (int j = 0; j < tris.Length; j += 3) 
            {
                //swap order of tri vertices
                int temp = tris[j];
                tris[j] = tris[j + 1];
                tris[j + 1] = temp;
            }
            mesh.SetTriangles(tris, i);
        }
    }
}