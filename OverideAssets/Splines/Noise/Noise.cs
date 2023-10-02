using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Noise : ScriptableObject
{
    public enum NoiseType
    {
        Ramdom,
        Perlin,
        FBM,
    }
    public NoiseType noiseType;
    public AnimationCurve curve = AnimationCurve.Linear(
    timeStart: 0f,
    valueStart: 0f,
    timeEnd: 1f,
    valueEnd: 1f
);

    [VisualizeTexture(200)]
    public Texture2D sampleTexture;

    private void OnValidate()
    {
        SetNoiseMap();
    }

    [HideInInspector]
    public int pixelX = 256;
    public int pixelY = 256;

    void SetNoiseMap()
    {
        this.sampleTexture = this.GetNoiseMap(pixelX, pixelY);
    }

    public Texture2D GetNoiseMap(int pixelX, int pixelY)
    {
        float[,] floatMap = this.GetNoiseFloats(pixelX, pixelY);
        Texture2D map = new Texture2D(pixelX, pixelY);
        Color[] color = new Color[pixelX * pixelY];
        for (int i = 0; i < pixelX; i++)
        {
            for (int j = 0; j < pixelY; j++)
            {
                var r = floatMap[i, j];
                color[j * pixelY + i] = new Color(r, r, r);
            }
        }
        map.SetPixels(color);
        map.Apply();
        return map;

    }

    public virtual float[,] GetNoiseFloats(int pixel)
    {
        float[,] map = new float[pixel, pixel];
        return map;
        
    }

    public virtual float[,] GetNoiseFloats(int pixelX,int pixelY)
    {
        float[,] map = new float[pixelX, pixelY];
        return map;

    }

    public virtual float[,] GetNoiseFloats(int pixelX, int pixelY,int seed)
    {
        float[,] map = new float[pixelX, pixelY];
        return map;

    }

    public virtual float GetNoise2D(Vector2 pos)
    {
        return 0;
    }

    public virtual float GetNoise3D(Vector3 pos)
    {
        pos.y += 1;
        pos.z += 2;
        float xy = _perlin3DFixed(pos.x, pos.y);
        float xz = _perlin3DFixed(pos.x, pos.z);
        float yz = _perlin3DFixed(pos.y, pos.z);
        float yx = _perlin3DFixed(pos.y, pos.x);
        float zx = _perlin3DFixed(pos.z, pos.x);
        float zy = _perlin3DFixed(pos.z, pos.y);

        return xy * xz * yz * yx * zx * zy;

        float _perlin3DFixed(float a, float b)
        {
            return Mathf.Sin(Mathf.PI * GetNoise2D(new Vector2(a,b)));
        }
    }
}
