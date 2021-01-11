using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidNoiseFilter : INoiseFilter {
    NoiseSettings.RigidNoiseSettings settings;
    Noise noise = new Noise();

    public RigidNoiseFilter(NoiseSettings.RigidNoiseSettings settings) {
        this.settings = settings;
    }

    public float Evaluate(Vector3 point) {
        float noiseValue = 0;
        float frequency = settings.baseRoughness;
        float amplitude = 1;
        float weight = 1 * settings.weightMultiplier;

        for (int i = 0; i < settings.numberOfLayers; i++) {
            // get absolute value of noise, then invert it (flip sin wave)
            float v = 1 - Mathf.Abs(noise.Evaluate(point * frequency + settings.centre));
            // make ridges more pronounced by squaring
            v *= v;
            // get more detail in mountain ridges
            v *= weight;
            weight = Mathf.Clamp01(v * settings.weightMultiplier);

            noiseValue += v * amplitude;
            frequency *= settings.roughness; // if roughness > 1 -> frequency increase
            amplitude *= settings.persistance; // if persistance < 1 -> amplitude will decrease
        }

        noiseValue =  noiseValue - settings.minValue;
        return noiseValue * settings.strength;
    }
}

