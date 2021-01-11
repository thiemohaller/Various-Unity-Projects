using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour {
    Mesh mesh;
    MeshCollider meshCollider;

    Vector3[] verticeArray;
    int[] triangleArray;

    [Range(5,256)]
    public int xSize = 20;
    [Range(5, 256)]
    public int zSize = 20;
    public int heightMultiplier = 2;
    public float noiseMultiplier = 0.3f;

    void Start() {
        mesh = new Mesh(); // generate new mesh
        GetComponent<MeshFilter>().mesh = mesh;
        meshCollider = gameObject.AddComponent<MeshCollider>();

        //StartCoroutine(CreateShape());
        CreateShape();
        UpdateMesh();
        meshCollider.sharedMesh = mesh;
    }

    private void Update() {
        //UpdateMesh();
    }

    private void UpdateMesh() {
        mesh.Clear();
        mesh.vertices = verticeArray;
        mesh.triangles = triangleArray;

        mesh.RecalculateNormals();
    }

    /**
     * CoRoutine
     */
    //IEnumerator CreateShape() {
    void CreateShape() { 
        verticeArray = new Vector3[(xSize + 1) * (zSize + 1)];
        triangleArray = new int[xSize * zSize * 6]; // create quads = 2 triangles = 6 * grid 

        for (int z = 0, index = 0; z <= zSize; z++) {
            for (int x = 0; x <= xSize; x++, index++) {
                float height = Mathf.PerlinNoise(x * noiseMultiplier, z * noiseMultiplier) * heightMultiplier;
                //height += Mathf.PerlinNoise(x * noiseMultiplier, z * noiseMultiplier) * heightMultiplier;
                height += Mathf.PerlinNoise(x * .3f, z * .3f) * 1.2f;
                verticeArray[index] = new Vector3(x, height, z);
            }
        }

        int vert = 0;
        int triangles = 0;
        for (int z = 0; z < zSize; z++) {
            for (int x = 0; x < xSize; x++) {
                // use vert as offset
                triangleArray[triangles + 0] = vert + 0;
                triangleArray[triangles + 1] = vert + xSize + 1;
                triangleArray[triangles + 2] = vert + 1;
                triangleArray[triangles + 3] = vert + 1;
                triangleArray[triangles + 4] = vert + xSize + 1;
                triangleArray[triangles + 5] = vert + xSize + 2;

                vert++;
                triangles += 6;

                // for generation purposes:
                //yield return new WaitForSeconds(.1f);
                //yield return new WaitForSeconds(.01f);
            }

            vert++;
        }
    }

    /**
     * Draw spheres for grid (=verices) in the editor when running the game
     */
    private void OnDrawGizmos() {
        if (verticeArray == null) {
            return;
        }

        for (int i = 0; i < verticeArray.Length; i++) {
            Gizmos.DrawSphere(verticeArray[i], .1f);
        }
    }
}
