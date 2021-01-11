using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamCube : MonoBehaviour
{
    public int band;
    public float scaleStart = 2;
    public float scaleMultiplier = 10;
    public bool useBuffer;

    private Material prefabMaterial;

    void Start()
    {
        prefabMaterial = GetComponent<MeshRenderer>().materials[0];        
    }

    void Update()
    {
        if (useBuffer) { // bandbuffer is between 0 and 1
            transform.localScale = new Vector3(transform.localScale.x, (AudioVisualization.audioBandBuffer[band] * scaleMultiplier) + scaleStart, transform.localScale.z);
            Color color = new Color(AudioVisualization.audioBandBuffer[band], AudioVisualization.audioBandBuffer[band], AudioVisualization.audioBandBuffer[band]);
            prefabMaterial.SetColor("_EmissionColor", color);
        } else {
            transform.localScale = new Vector3(transform.localScale.x, (AudioVisualization.audioBand[band] * scaleMultiplier) + scaleStart, transform.localScale.z);
            Color color = new Color(AudioVisualization.audioBand[band], AudioVisualization.audioBand[band], AudioVisualization.audioBand[band]);
            prefabMaterial.SetColor("_EmissionColor", color);
        }        
    }
}
