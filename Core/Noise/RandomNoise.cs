using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AWD", menuName = "AWD/Noise/Random")]
public class RandomNoise : Noise
{
    [Range(0, 1)]
    public float max = 1, min = 0;
    public RandomNoise()
    {
        noiseType = NoiseType.Ramdom;
    }

    public override float[,] GetNoiseFloats(int pixel)
    {
        float[,] map = new float[pixel, pixel];
        for (int i = 0; i < pixel; i++)
        {
            for (int j = 0; j < pixel; j++)
            {
                var r = Random.Range(min, max);
                r *= Mathf.Clamp(curve.Evaluate(r), 0, 1);
                map[i, j] = r;
            }
        }
        return map;
    }

    public override float[,] GetNoiseFloats(int pixelX, int pixelY)
    {
        float[,] map = new float[pixelX, pixelY];
        for (int i = 0; i < pixelX; i++)
        {
            for (int j = 0; j < pixelY; j++)
            {
                var r = Random.Range(min, max);
                r *= Mathf.Clamp(curve.Evaluate(r), 0, 1);
                map[i, j] = r;
            }
        }
        return map;
    }
    public override float[,] GetNoiseFloats(int pixelX, int pixelY, int seed)
    {
        float[,] map = new float[pixelX, pixelY];
        Random.seed = seed;
        for (int i = 0; i < pixelX; i++)
        {
            for (int j = 0; j < pixelY; j++)
            {
                var r = Random.Range(min, max);
                r *= Mathf.Clamp(curve.Evaluate(r), 0, 1);
                map[i, j] = r;
            }
        }
        return map;
    }

    public override float GetNoise2D(Vector2 pos)
    {
        var r = Random.Range(min, max);
        r *= Mathf.Clamp(curve.Evaluate(r), 0, 1);
        return r;
    }

    public static float[,] GetRandomNoise(int pixel, int max, int min)
    {
        float[,] map = new float[pixel, pixel];
        for (int i = 0; i < pixel; i++)
        {
            for (int j = 0; j < pixel; j++)
            {
                var r = Random.Range(min, max);
                map[i, j] = r;
            }
        }
        return map;
    }

    public static float[,] GetRandomNoise(int pixelX, int pixelY, int max, int min)
    {
        float[,] map = new float[pixelX, pixelY];
        for (int i = 0; i < pixelX; i++)
        {
            for (int j = 0; j < pixelY; j++)
            {
                var r = Random.Range(min, max);
                map[i, j] = r;
            }
        }
        return map;
    }
}

