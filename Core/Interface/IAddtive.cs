using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAdditive<T>
{
    T Add(T item);
}
