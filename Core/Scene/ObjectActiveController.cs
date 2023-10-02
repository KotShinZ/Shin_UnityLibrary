using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectActiveController : MonoBehaviour
{
    [Space(15)]
    public List<GameObject> objects;
    bool isActive = true;

    [ContextMenu("Active")]
    public void ActiveObjects(bool b)
    {
        if(b == true && isActive == false)
        {
            isActive = true;
            Active(b);
        }
        else if(b == false && isActive == true)
        {
            isActive = false;
            Active(b);
        }

        void Active(bool acitve)
        {
            foreach (var o in objects)
            {
                if (o.TryGetComponent<Terrain>(out Terrain t))
                {
                    t.drawHeightmap = acitve;
                    t.drawTreesAndFoliage = acitve;
                    var te = o.GetComponent<TerrainCollider>();
                    if (te != null) te.enabled = acitve;
                }
                else
                {
                    o.SetActive(acitve);
                }
            }
        }
    }
}
