using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonEventInstantiate : MonoBehaviour, IOnEventCallback
{
    protected virtual byte CustomInstantiateEventCode => 1;
    [Header("ネットワークを通して生成するPrefabは全てこの中にいれておく")]
    public List<GameObject> networkPrefabs;

    public static string generatingPrefabName = null;

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public GameObject GetNetworkPrefab(string name)
    {
        foreach (var prefab in networkPrefabs)
        {
            if (prefab.name == name)
            {
                return prefab;
            }
        }
        Debug.LogWarning("ネットワークPrefabが見つかりません: " + name);
        return null;
    }

    /// <summary>
    /// 他のプレイヤーからの生成依頼を受け取ると、リストの中から該当するPrefabを探して生成する
    /// </summary>
    /// <param name="photonEvent"></param>
    public void OnEvent(EventData photonEvent)
    {
        if(PhotonNetwork.IsConnected == false) return;
        //Debug.Log("OnEvent");
        var eventCode = photonEvent.Code;

        if (eventCode != CustomInstantiateEventCode)
        {
            return;
        }
        var data = (object[])photonEvent.CustomData;
        //Debug.Log("InstantiateEvent受信" + (string)data[0]);

        // 受信したtransformを設定
        
        var obj = GameObject.Instantiate(GetNetworkPrefab((string)data[0]), (Vector3)data[1], (Quaternion)data[2]);
        if(obj.TryGetComponent(out PhotonObjectSynchronizer pos))
        {
            pos.setMine = false;
        }
        
        // Photon
        var photonView = obj.GetComponent<PhotonView>();

        // 受信したViewIDを用いて同期する
        photonView.ViewID = (int)data[3];

        OnInstantiate(obj, data);

        generatingPrefabName = null;
        
    }

    protected virtual void OnInstantiate(GameObject obj, object[] data) 
    { 
        
    }
}
