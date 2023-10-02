using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IMoveable  : IEventSystemHandler
{
    /// <summary>
    /// “®‚¯‚é‚©‚Ç‚¤‚©‚ğİ’è
    /// </summary>
    /// <param name="b"></param>
    abstract void SetActiveMove(bool b);
}