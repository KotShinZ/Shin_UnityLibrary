using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using Random = UnityEngine.Random;

namespace Shin_UnityLibrary
{
    public static class Utils
    {
        public static T GetRandomInList<T>(this List<T> list)
        {
            if(list.Count == 0)
            {
                Debug.Log("List is Zero");
                return default(T);
            }
            var n = UnityEngine.Random.Range(0, list.Count);
            T t;
            try { t = list[n]; }
            catch { 
                Debug.Log("ListCount is " + list.Count + "  N is " + n);
                return list[0];
            }
            return t;
        }

        public static bool Probability(int percent)
        {
            Mathf.Clamp(percent, 0, 100);
            if (Random.Range(0, 100) <= percent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool Probability(float percent)
        {
            float p = Mathf.Clamp(percent, 0, 1);
            if (Random.Range(0, (float)1) <= p)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool NumberInRange(float number, float Max, float Min, bool IsContainMaxMin = true)
        {
            if (IsContainMaxMin == true)
            {
                if (number <= Max && number >= Min)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (number < Max && number > Min)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public static float[,] TexToFloat(Texture2D texture)
        {
            float[,] f = new float[texture.width, texture.height];

            for (int i = 0; i < texture.width; i++)
            {
                for (int j = 0; j < texture.height; j++)
                {
                    f[i, j] = texture.GetPixel(i, j).grayscale;
                }
            }
            return f;
        }

        /// <summary>
        /// png画像をpathに保存します
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="path"></param>
        public static void SaveTexturePng(Texture2D texture, string path)
        {
            var bytes = texture.EncodeToPNG();
            File.WriteAllBytes(path + "/" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png", bytes);
        }

        /// <summary>
        /// テクスチャの解像度を変更します。
        /// </summary>
        /// <param name="srcTexture"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <returns></returns>
        public static Texture2D ResizeTexture(Texture2D texture, int width, int height)
        {
            // リサイズ後のサイズを持つRenderTextureを作成して書き込む
            var rt = RenderTexture.GetTemporary(width, height);
            Graphics.Blit(texture, rt);

            // リサイズ後のサイズを持つTexture2Dを作成してRenderTextureから書き込む
            var preRT = RenderTexture.active;
            RenderTexture.active = rt;
            var ret = new Texture2D(width, height);
            ret.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            ret.Apply();
            RenderTexture.active = preRT;

            RenderTexture.ReleaseTemporary(rt);
            return ret;
        }

        /// <summary>
        /// 一定の値のFloat[,]を返します
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static float[,] ReturnFloats(int width, int height)
        {
            float[,] f = new float[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    f[i, j] = 1;
                }
            }

            return f;
        }
        /// <summary>
        /// 一定の値のFloat[,]を返します
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static float[,] ReturnFloats(int width,int height,int value)
        {
            float[,] f = new float[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    f[i, j] = value;
                }
            }

            return f;
        }

        public static Vector2Int VectorThreeToTwoInt(Vector3 p)
        {
            return new Vector2Int(Mathf.RoundToInt(p.x), Mathf.RoundToInt(p.z));
        }
        public static Vector2 VectorThreeToTwo(Vector3 p)
        {
            return new Vector2(Mathf.Round(p.x), Mathf.Round(p.z));
        }
        public static Vector3 VectorTwoToThree(Vector2 p)
        {
            return new Vector3(Mathf.Round(p.x), Mathf.Round(p.y), 0);
        }

        public static void AddDictionary<K, V>(this Dictionary<K, V> dict, Dictionary<K, V> add)
        {
            List<KeyValuePair<K, V>> pairs = add.ToList();
            pairs.ForEach(pair => dict.Add(pair.Key, pair.Value));
        }

        public static Vector3 GetCameraForward()
        {
            var v = Camera.main.transform.forward;
            v.y = 0;
            return v.normalized;
        }
    }


}
