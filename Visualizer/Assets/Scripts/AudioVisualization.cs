using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public class AudioVisualization : MonoBehaviour
{
    AudioSource audioSource;    
    private float[] frequencyBands = new float[8];
    private float[] bandBuffer = new float[8];
    private float[] bufferDecrease = new float[8];
    private float[] frequencyBandsHighestBuffer = new float[8];

    public static float[] audioBand = new float[8];
    public static float[] audioBandBuffer = new float[8];
    public static float[] samples = new float[512];

    public bool isFourtyKHertz = true;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        BandBuffer();
        CreateAudioBands();
    }

    void GetSpectrumAudioSource() {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }

    void MakeFrequencyBands() {
        /*
         * ODESZA - Something About You -> 44k
         * 44K / e.g. 512 ~= 86 hz per sample
         * Calc for 22K
         * 2^8 = 256 -> add all previous entries (2,4,8,16,32,64,128) = 510
         */
        var average = 0f;
        var count = 0;
        for(int i = 1; i <= frequencyBands.LongLength; i++) {
            int sampleCount = (int)(Mathf.Pow(2, i));

            if (i == 8) {
                // 510 + 2 = 512
                sampleCount += 2;
            }

            for (int j = 0; j < sampleCount; j++) {
                average += samples[count] * (count + 1);
                count++;
            }

            average /= count;
            if (isFourtyKHertz) { 
                frequencyBands[i - 1] = average * 100;
                // have to offset by 1 because this loop starts at 1
                // average will be just below 0 without `*10`
            } else {
                frequencyBands[i - 1] = average * 10;
            }
        }
    }

    void BandBuffer() {
        for (int i = 0; i < bandBuffer.Length; i++) {
            if (frequencyBands[i] > bandBuffer[i]) {
                bandBuffer[i] = frequencyBands[i];
                bufferDecrease[i] = 0.005f;
            }

            if (frequencyBands[i] < bandBuffer[i]) {
                bandBuffer[i] -= bufferDecrease[i];
                bufferDecrease[i] *= 1.2f; // 20%

            }
        }
    }

    void CreateAudioBands() {
        for (int i = 0; i < 8; i++) {
            if (frequencyBands[i] > frequencyBandsHighestBuffer[i]) {
                frequencyBandsHighestBuffer[i] = frequencyBands[i];
            }

            audioBand[i] = (frequencyBands[i] / frequencyBandsHighestBuffer[i]);
            audioBandBuffer[i] = (bandBuffer[i] / frequencyBandsHighestBuffer[i]);
        }
    }
}
