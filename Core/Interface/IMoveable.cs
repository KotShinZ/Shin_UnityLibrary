using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IMoveable  : IEventSystemHandler
{
    /// <summary>
    /// �����邩�ǂ�����ݒ�
    /// </summary>
    /// <param name="b"></param>
    abstract void SetActiveMove(bool b);
}