using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IValueable<T>
{
    public abstract T value { get; set; }
}
