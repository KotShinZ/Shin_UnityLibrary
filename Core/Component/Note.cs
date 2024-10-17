using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
#if UNITY_EDITOR

    [SerializeField]
    [VisualTextArea(1, 6, 20)]
    private string m_note;

    public string note { get { return m_note; } set { m_note = value; } }
#else
        private void Awake()
        {
            Destroy(this);
        }
#endif
}
