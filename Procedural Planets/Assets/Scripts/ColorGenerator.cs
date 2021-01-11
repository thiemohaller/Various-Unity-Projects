using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGenerator {
    ColorSettings settings;
    Texture2D texture;
    const int textureResolution = 50;
    INoiseFilter biomeNoiseFilter;

    public void UpdateSettings(ColorSettings settings) {
        this.settings = settings;
        if (texture == null || texture.height != settings.biomeColorSettings.biomes.Length) {
            // textureResolution: first half ocean, other half land -> * 2
            texture = new Texture2D(textureResolution * 2, settings.biomeColorSettings.biomes.Length, TextureFormat.RGBA32, false);
        }

        biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.biomeColorSettings.noise);
    }

    public void UpdateElevation(MinMax elevationMinMax) {
        settings.planetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    // Should return 0 for first biome, 1 for the last, anything in between -> other biomes
    public float BiomePercentFromPoint(Vector3 pointOnUnitSphere) {
        // unit sphere is from +1 to -1 -> '/2f'
        float heightPercent = (pointOnUnitSphere.y + 1) / 2f;
        // add noise
        heightPercent += (biomeNoiseFilter.Evaluate(pointOnUnitSphere) - settings.biomeColorSettings.noiseOffset) * settings.biomeColorSettings.noiseStrength;
        float biomeIndex = 0;
        int numBiomes = settings.biomeColorSettings.biomes.Length;
        float blendRange = settings.biomeColorSettings.blendAmount / 2f + 0.001f; // blend amount should not be zero because otherwise the weight calculation won't work properly

        for (int i = 0; i < numBiomes; i++) {
            // blend biomes by 
            // calculate distance between startheight and currentheight
            float distance = heightPercent - settings.biomeColorSettings.biomes[i].startHeight;
            // depends on whether distance is within blending distance -> a little above start height, a little below start height
            float weight = Mathf.InverseLerp(-blendRange, blendRange, distance);
            biomeIndex *= 1 - weight; // this prevents the index from becoming too high
            biomeIndex += i * weight;
        }

        // no division by zero in case of only 1 biome: 'Mathf.Max(1, ...)' -> 1 or bigger
        return biomeIndex / Mathf.Max(1, (numBiomes - 1));
    }

    public void UpdateColors() {
        Color[] colors = new Color[texture.width * texture.height];
        int colorIndex = 0;

        foreach (var biome in settings.biomeColorSettings.biomes) {
            for (int i = 0; i < textureResolution * 2; i++) {
                Color gradientColor;
                if (i < textureResolution) {
                    gradientColor = settings.oceanColor.Evaluate(i / (textureResolution - 1f));
                } else {
                    gradientColor = biome.gradient.Evaluate((i - textureResolution) / (textureResolution - 1f));
                }
                Color tintColor = biome.tint;
                colors[colorIndex] = gradientColor * (1 - biome.tintPercent) + tintColor * biome.tintPercent;
                colorIndex++;
            }
        }

        texture.SetPixels(colors);
        texture.Apply();
        settings.planetMaterial.SetTexture("_texture", texture);
    }
}
