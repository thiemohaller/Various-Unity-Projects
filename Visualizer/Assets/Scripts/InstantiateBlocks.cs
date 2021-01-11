using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateBlocks : MonoBehaviour
{
    public GameObject sampleCubePrefab;
    private GameObject[] sampleCubes = new GameObject[512];
    public int distance = 100;
    public float maxScale = 10000; 

    void Start() {
        // this may become problematic if length changes -> float accuracy https://stackoverflow.com/questions/15400903/division-in-c-sharp-to-get-exact-value
        var degreesForRoatation = (float) 360 / sampleCubes.Length; 
        //Debug.Log(degreesForRoatation);

        for (int i = 0; i < sampleCubes.Length; i++) {
            GameObject instanceSampleCube = Instantiate(sampleCubePrefab);
            instanceSampleCube.transform.position = transform.position;
            instanceSampleCube.transform.parent = transform;
            instanceSampleCube.name = "SampleCube" + i;
            transform.eulerAngles = new Vector3(0, degreesForRoatation * -i, 0);
            instanceSampleCube.transform.position = Vector3.forward * distance;
            
            sampleCubes[i] = instanceSampleCube;
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < sampleCubes.Length; i++) {
            if(sampleCubes != null) {
                sampleCubes[i].transform.localScale = new Vector3(10, (AudioVisualization.samples[i] * maxScale) + 2, 10);
            }
        }
    }
}
