using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShinjiGeneral;
using Sirenix.OdinInspector;
using System;
using UniRx;
using System.Reflection;

namespace ShinjiGeneral
{
    public enum Resolution
    {
        r128 = 128,
        r256 = 256,
        r512 = 512,
        r1024 = 1024,
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Front,
        Back
    }

    [Serializable]
    public class HSL
    {
        [Range(0, 360)] public float hue = 0;
        [Range(0, 2)] public float saturation = 1;
        [Range(0, 2)] public float value = 1;

        public Color HSLColorConvert(Color color)
        {
            float h;
            float s;
            float v;
            Color.RGBToHSV(color, out h, out s, out v);

            return Color.HSVToRGB(h * hue, s * saturation, v * value);
        }

        public Texture2D HSLTexture(Texture2D tex)
        {
            Type typeFile = tex.GetType();
            MethodInfo mi = typeFile.GetMethod("HSL");
            object[] methodParams = { hue, saturation, value };
            if (mi != null)
            {
                return (Texture2D)mi.Invoke(null, methodParams);
            }
            else
            {
                Debug.LogError("No method like [HSL]");
                return null;
            }
        }
    }

    [Serializable]
    public class ValueMaxSet
    {
        public float max;
        [ShowInInspector]
        private float nowValue;

        public ValueMaxSet(float value, float max)
        {
            this.max = max;
            this.value = value;
        }

        public float value
        {
            get { return nowValue; }
            set
            {
                nowValue = value < max ? value : max;
            }
        }

        public static void SetValue(ReactiveProperty<ValueMaxSet> property, float v)
        {
            SetValue(property, v, property.Value.max);
        }
        public static void AddValue(ReactiveProperty<ValueMaxSet> property, float v)
        {
            SetValue(property, property.Value.value + v, property.Value.max);
        }
        public static void SetValue(ReactiveProperty<ValueMaxSet> property, float v, float m)
        {
            property.Value = new ValueMaxSet(v, m);
        }

        public void Full()
        {
            nowValue = max;
        }
    }
}

