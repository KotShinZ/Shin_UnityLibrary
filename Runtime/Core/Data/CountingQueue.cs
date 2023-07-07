using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UniRx;
using System;

public class CountingReactiveList<T> : IEnumerable<T>
{
    public ReactiveCollection<T> list;

    public IObservable<T> ObservableAdd => (IObservable<T>)list.ObserveAdd();
    public IObservable<T> ObservableRemove => (IObservable<T>)list.ObserveRemove();
    public IObservable<T> ObservableChanged => ObservableAdd.Merge(ObservableRemove);

    public int Count => list.Count;

    public int Capacity { get; private set; }

    public CountingReactiveList(int capacity)
    {
        Capacity = capacity;
        list = new ReactiveCollection<T>();
    }

    public T Get(int index) => list[index];

    public void Add(T item)
    {
        list.Add(item);

        if (Count > Capacity) list.RemoveAt(0);
    }

    public void Remove(T item) => list.Remove(item);

    public void RemoveAll()
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            list.RemoveAt(i);
        }
    }
    public bool Contains(T item) => list.Contains(item);

    public void RemoveAt(int n) => list.RemoveAt(n);

    public IEnumerator<T> GetEnumerator() => list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => list.GetEnumerator();
}
