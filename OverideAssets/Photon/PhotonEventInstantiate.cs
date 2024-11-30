using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonEventInstantiate : MonoBehaviour, IOnEventCallback
{
    private const byte CustomInstantiateEventCode = 1;
    [Header("ネットワークを通して生成するPrefabは全てこの中にいれておく")]
    public List<PhotonView> networkPrefabs;

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
                return prefab.gameObject;
            }
        }
        return null;
    }

    // RaiseEventを受け取る
    public void OnEvent(EventData photonEvent)
    {
        if(PhotonNetwork.IsConnected == false) return;

        var eventCode = photonEvent.Code;

        if (eventCode != CustomInstantiateEventCode)
        {
            return;
        }

        var data = (object[])photonEvent.CustomData;

        // 受信したtransformを設定
        var obj = Instantiate(GetNetworkPrefab((string)data[0]), (Vector3)data[1], (Quaternion)data[2]);
        obj.GetComponent<PhotonObjectSynchronizer>().objectClient = true;

        // Photon
        var photonView = obj.GetComponent<PhotonView>();

        // 受信したViewIDを用いて同期する
        photonView.ViewID = (int)data[3];

        generatingPrefabName = null;
    }
}
