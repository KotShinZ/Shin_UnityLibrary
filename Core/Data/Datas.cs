using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using System.Reflection;
using System.Linq;

namespace Shin_UnityLibrary
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
        [AttributeVector(0, 1, 0)]
        Up = 1 << 0,
        [AttributeVector(0, -1, 0)]
        Down = 1 << 1,
        [AttributeVector(-1, 0, 0)]
        Left = 1 << 2,
        [AttributeVector(1, 0, 0)]
        Right = 1 << 3,
        [AttributeVector(0, 0, 1)]
        Front = 1 << 4,
        [AttributeVector(0, 0, -1)]
        Back = 1 << 5
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
        [SerializeField]
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

    [Serializable]
    public class Vector3Class
    {
        public Vector3 vector;
        public Vector3Class(Vector3 vector) { this.vector = vector; }
    }

    [Serializable]
    public class CollisionData<T> where T : UnityEngine.Component
    {
        private int ID;
        public T component;
        public HitType hitType;
        public Vector3 direction;
        public Transform hitted;

        #region コンストラクタ
        /// <summary>
        /// 
        /// </summary>
        /// <param name="col">当たった方</param>
        /// <param name="hitted">あてられた方</param>
        public CollisionData(T _component, Collider col, Transform hitted)
        {
            ID = col.gameObject.GetInstanceID();
            Init(_component, HitType.Trigger, Vector3.Normalize(col.ClosestPointOnBounds(hitted.position) - hitted.position), hitted);
        }
        public CollisionData(T _component, Collision col, Transform hitted)
        {
            ID = col.gameObject.GetInstanceID();
            Init(_component, HitType.Collision, Vector3.Normalize(col.contacts.Last().point - hitted.position), hitted);
        }
        public CollisionData(T component, HitType hitType, Vector3 direction, Transform hitted)
        {
            this.component = component;
            this.hitType = hitType;
            this.direction = Vector3.Normalize(direction);
            this.hitted = hitted;
        }

        public void Init(T component, HitType hitType, Vector3 direction, Transform hitted)
        {
            this.component = component;
            this.hitType = hitType;
            this.direction = Vector3.Normalize(direction);
            this.hitted = hitted;
        }


        #endregion

        public bool Equal(Collision obj)
        {
            return obj.gameObject.GetInstanceID() == ID;
        }
        public bool Equal(Collider obj)
        {
            return obj.gameObject.GetInstanceID() == ID;
        }

        public bool isDirection(Vector3 _direction, float threshold = 0.7f)
        {
            return Vector3.Dot(direction, _direction) > threshold;
        }

        /// <summary>
        /// GetColliderEventも含めてGetComponentする
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetComponent<T>()
        {
            return component.GetComponentWithGetColliderEvent<T>();
        }

        /// <summary>
        /// ComponentをS型に切り替える
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <returns></returns>
        public CollisionData<S> SwapComponent<S>() where S : UnityEngine.Component
        {
            var c = component.GetComponent<S>();
            if (c != null)
            {
                var newData = new CollisionData<S>(c, hitType, direction, hitted);
                return newData;
            }
            else { return null; }
        }
    }
    public enum HitType
    {
        Trigger,
        Collision
    }

    public class TypeEqaulObject
    {
        public override int GetHashCode()
        {
            Debug.Log(this.GetType());
            return this.GetType().GetHashCode();
        }

        public override bool Equals(object obj)
        {
            TypeEqaulObject ins = obj as TypeEqaulObject;
            if (ins == null) return false;
            return ins.GetType() == this.GetType();
        }
    }
}

