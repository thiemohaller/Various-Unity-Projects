using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class TriangleGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] verticeArray;
    int[] triangleArray;


    void Start()
    {
        mesh = new Mesh(); // generate new mesh
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
        UpdateMesh();
    }

    private void UpdateMesh() {
        mesh.Clear();
        mesh.vertices = verticeArray;
        mesh.triangles = triangleArray;
    }

    private void CreateShape() {
        verticeArray = new Vector3[] {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(1,0,0)
            //quad:
            //,new Vector3(1,0,1)
        };

        triangleArray = new int[] {
            0, 1, 2
            // quad:
            //,1,3,2
            // culling: 1,2,3 -> no lighting, only half triangle
        };
    }
}
