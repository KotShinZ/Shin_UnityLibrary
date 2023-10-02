#if Shin_Overide_SelectionGroup

using System.Collections;
using System.Collections.Generic;
using Unity.SelectionGroups;
using UnityEngine;

namespace Unity.SelectionGroups
{
    public class SelectionUpdater : SingletonMonoBehaviour<SelectionUpdater>
    {
        public List<SelectionGroup> groups;
        [Readonly] public bool updated = false;

        public void SelectionUpdate()
        {
            if(!updated)
            {
                //groups.ForEach(g => g.SetQuery(g.Query));
                updated = true;
            }
        }

        public void Update()
        {
            updated = false;
        }
    }
}

#endif