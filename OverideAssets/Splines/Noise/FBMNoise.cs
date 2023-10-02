using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Spline", menuName = "Spline/Noise/FBM")]
public class FBMNoise : Noise
{
    public float x = 0;
    public float y = 0;
    public float scale = 1;
    public int octave = 8;
    [Range(0, 1)]
    public float persistence = (float)0.5;
    public float lacunarity = 2;
    public FBMNoise()
    {
        noiseType = NoiseType.FBM;
    }

    public override float[,] GetNoiseFloats(int pixel)
    {
        float[,] map = new float[pixel, pixel];

        float i = 0;

        while (i < pixel)
        {
            float j = 0;
            while (j < pixel)
            {
                float amplitude = (float)0.5;
                float frequentcy = scale;

                float k = 0;

                for (int n = 1; n <= octave; n++)
                {
                    float xCoord = x + j / pixel * frequentcy;
                    float yCoord = y + i / pixel * frequentcy;
                    k += amplitude * (Mathf.PerlinNoise(xCoord, yCoord));
                    amplitude *= persistence;
                    frequentcy *= lacunarity;
                }
                k *= Mathf.Clamp(curve.Evaluate(k), 0, 1);
                map[(int)i, (int)j] = k;
                j++;
            }
            i++;
        }
        return map;
    }

    public override float[,] GetNoiseFloats(int pixelX, int pixelY)
    {
        float[,] map = new float[pixelX, pixelY];

        float i = 0;

        while (i < pixelX)
        {
            float j = 0;
            while (j < pixelY)
            {
                float amplitude = (float)0.5;
                float frequentcy = scale;

                float k = 0;

                for (int n = 1; n <= octave; n++)
                {
                    float xCoord = x + j / pixelY * frequentcy;
                    float yCoord = y + i / pixelX * frequentcy;
                    k += amplitude * (Mathf.PerlinNoise(xCoord, yCoord));
                    amplitude *= persistence;
                    frequentcy *= lacunarity;
                }
                k *= Mathf.Clamp(curve.Evaluate(k), 0, 1);
                map[(int)i, (int)j] = k;
                j++;
            }
            i++;
        }
        return map;
    }
    public override float[,] GetNoiseFloats(int pixelX, int pixelY, int seed)
    {
        float[,] map = new float[pixelX, pixelY];

        float i = 0;

        while (i < pixelX)
        {
            float j = 0;
            while (j < pixelY)
            {
                float amplitude = (float)0.5;
                float frequentcy = scale;

                float k = 0;

                for (int n = 1; n <= octave; n++)
                {
                    float xCoord = x + j / pixelY * frequentcy;
                    float yCoord = y + i / pixelX * frequentcy;
                    k += amplitude * (Mathf.PerlinNoise(xCoord * seed * 100, yCoord * seed * 100));
                    amplitude *= persistence;
                    frequentcy *= lacunarity;
                }
                k *= Mathf.Clamp(curve.Evaluate(k), 0, 1);
                map[(int)i, (int)j] = k;
                j++;
            }
            i++;
        }
        return map;
    }

    public override float GetNoise2D(Vector2 pos)
    {
        float amplitude = (float)0.5;
        float frequentcy = scale;

        float k = 0;

        for (int n = 1; n <= octave; n++)
        {
            float xCoord = pos.x * frequentcy;
            float yCoord = pos.y * frequentcy;
            k += amplitude * (Mathf.PerlinNoise(xCoord, yCoord));
            amplitude *= persistence;
            frequentcy *= lacunarity;
        }
        k *= Mathf.Clamp(curve.Evaluate(k), 0, 1);
        return k;
    }
}
