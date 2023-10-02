using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "AWD", menuName = "AWD/Noise/Perlin")]
public class PerlinNoise : Noise
{
    public float x = 0;
    public float y = 0;
    public float scale = 20f;
    public PerlinNoise()
    {
        noiseType = NoiseType.Perlin;
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
                float xCoord = x + j / pixel * scale;
                float yCoord = y + i / pixel * scale;
                float k = Mathf.PerlinNoise(xCoord, yCoord);
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
                float xCoord = x + j / pixelY * scale;
                float yCoord = y + i / pixelX * scale;
                float k = Mathf.PerlinNoise(xCoord, yCoord);
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
                float xCoord = x + j / pixelY * scale;
                float yCoord = y + i / pixelX * scale;
                float k = Mathf.PerlinNoise(xCoord + seed * 100, yCoord + seed * 100);
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
        float xCoord = pos.x  * scale;
        float yCoord = pos.y  * scale;
        float k = Mathf.PerlinNoise(xCoord, yCoord);
        k *= Mathf.Clamp(curve.Evaluate(k), 0, 1);
        return k;
    }
}
