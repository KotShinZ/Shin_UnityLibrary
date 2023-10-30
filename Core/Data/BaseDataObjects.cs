using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Numerics;
using Shin_UnityLibrary;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

[System.Serializable]
public abstract class BaseData<T> : IOptimizedObservable<T>, IFormattable, IValueable<T>, IObservableStr, IAdditive<T>
{
    [SerializeField] protected ReactiveProperty<T> prop = new();
    public IObservable<T> observableNum => prop;

    public virtual T value { get => prop.Value; set => prop.Value = value; }

    public BaseData(T _num)
    {
        Set(_num);
    }

    public virtual void Set(T n)
    {
        value = n;
    }

    public abstract T Add(T item);
    public T Add(BaseData<T> item) { return Add(item.value); }

    public static BaseData<T> operator +(BaseData<T> c1, BaseData<T> c2)
    {
        c1.Add(c2);
        return c1;
    }
    public static BaseData<T> operator +(BaseData<T> c1, T c2)
    {
        c1.Add(c2);
        return c1;
    }

    public IDisposable Subscribe(IObserver<T> observer)
    {
        return observableNum.Subscribe(observer);
    }
    public IDisposable SubscribeToString(IObserver<string> observer)
    {
        return observableNum.Select(n => n.ToString()).Subscribe(observer);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return value.ToString();
    }

    public bool IsRequiredSubscribeOnCurrentThread()
    {
        return false;
    }
}

[System.Serializable]
public class BaseDataInt : BaseData<int>
{
    public BaseDataInt(int _num) : base(_num) { }

    public override int Add(int item) { value += item; return value; }

    public static BaseDataInt operator +(BaseDataInt d1, int d2)
    {
        d1.Add(d2);
        return d1;
    }
}

[System.Serializable]
public class BaseDataFloat : BaseData<float>
{
    public BaseDataFloat(float _num) : base(_num) { }

    public override float Add(float item) { value += item; return value; }

    public static BaseDataFloat operator +(BaseDataFloat d1, float d2)
    {
        d1.Add(d2);
        return d1;
    }
}

[System.Serializable]
public class BaseDataVector3 : BaseData<Vector3>
{
    public BaseDataVector3(Vector3 _num) : base(_num) { }

    public override Vector3 Add(Vector3 item) { value += item; return value; }
}

[System.Serializable]
public class BaseDataVector2 : BaseData<Vector2>
{
    public BaseDataVector2(Vector2 _num) : base(_num) { }

    public override Vector2 Add(Vector2 item) { value += item; return value; }
}

[System.Serializable]
public class BaseDataIntClamp : BaseDataInt
{
    public int max;
    public int min;

    public override int value
    {
        get => base.value; set
        {
            var c = Mathf.Clamp(value, min, max);
            base.value = c;
        }
    }

    public BaseDataIntClamp(int _num) : base(_num)
    {
        value = _num;
    }
    public BaseDataIntClamp(int _num, int _min, int _max) : base(_num)
    {
        min = _min;
        max = _max;
    }

    public override void Set(int n)
    {
        var c = Mathf.Clamp(n, min, max);
        base.Set(c);
    }
}

public class BaseDataTime : BaseDataFloat
{
    public BaseDataTime(float _num) : base(_num)
    {

    }
}
