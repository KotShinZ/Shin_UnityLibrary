using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShinjiGeneral
{
    public class IntClamp : MonoBehaviour
    {
        public int max = 1000000;
        public int min = 0;
        private int _value;
        public int Value
        {
            get { return _value; }
            set { _value = Mathf.Clamp(value, min, max); }
        }

        public IntClamp(int n = 0) 
        { 
            _value = n;
        }
    }
}

