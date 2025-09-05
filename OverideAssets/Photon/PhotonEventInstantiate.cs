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

    /// <summary>
    /// ネットワークが繋がっている場合はオンライン上の全てのプレイヤーにPrefabを生成する。繋がっていない場合はローカルにのみ生成する。
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public GameObject Instantiate(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (PhotonNetwork.IsConnected)
        {
            return NetworkInstantiate(prefab, position, rotation);
        }
        else
        {
            return GameObject.Instantiate(prefab, position, rotation);
        }
    }

    /// <summary>
    /// オンライン上の全てのプレイヤーにPrefabを生成する。PhotonView付きでリストに入れているPrefabを指定すること。
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public GameObject NetworkInstantiate(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (!PhotonNetwork.IsConnected) return null;

        // --- 1. まず、自分自身のゲーム内にオブジェクトを生成する ---
        GameObject newObject = GameObject.Instantiate(prefab.gameObject, position, rotation);
        var newPhotonView = AddPhotonComponents(newObject);    // PhotonViewとPhotonTransformViewを追加する

        // --- 2. 他のプレイヤーに同じプレハブを生成するように依頼する ---
        SendInstantiateMessage(newPhotonView, position, rotation);

        return newObject;
    }

    /// <summary>
    /// 他のプレイヤーに同じプレハブを生成するように依頼する
    /// </summary>
    /// <param name="newPhotonView"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    public void SendInstantiateMessage(PhotonView newPhotonView, Vector3 position, Quaternion rotation)
    {
        // 2. 新しいネットワークIDを確保し、生成したオブジェクトに割り当てる
        int viewID = PhotonNetwork.AllocateViewID(newPhotonView.ViewID);

        // 3. 他のプレイヤーに生成を依頼するためのイベントを送信する
        object[] content = new object[]
        {
            newPhotonView.gameObject.name,
            position,
            rotation,
            viewID // 自分で確保したIDを他の人にも教える
        };

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(CustomInstantiateEventCode, content, raiseEventOptions, SendOptions.SendReliable);
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
        Debug.LogError("ネットワークPrefabが見つかりません: " + name);
        return null;
    }

    /// <summary>
    /// 他のプレイヤーからの生成依頼を受け取ると、リストの中から該当するPrefabを探して生成する
    /// </summary>
    /// <param name="photonEvent"></param>
    public void OnEvent(EventData photonEvent)
    {
        if(PhotonNetwork.IsConnected == false) return;
        Debug.Log("OnEvent");
        var eventCode = photonEvent.Code;

        if (eventCode != CustomInstantiateEventCode)
        {
            return;
        }
        var data = (object[])photonEvent.CustomData;
        Debug.Log("InstantiateEvent受信" + (string)data[0]);

        // 受信したtransformを設定
        var obj = Instantiate(GetNetworkPrefab((string)data[0]), (Vector3)data[1], (Quaternion)data[2]);
        Debug.Log("Instantiate: " + (string)data[0]);
        // Photon
        var photonView = obj.GetComponent<PhotonView>();

        // 受信したViewIDを用いて同期する
        photonView.ViewID = (int)data[3];

        generatingPrefabName = null;
    }

    public PhotonView AddPhotonComponents(GameObject obj)
    {
        // Photon
        if (obj.TryGetComponent<PhotonView>(out PhotonView photonView) == false)
        {
            photonView = obj.AddComponent<PhotonView>();

            var photonTransformView = obj.AddComponent<PhotonTransformView>();

            // 初期化を行う
            photonView.ObservedComponents = new List<Component>();

            // Synchronizeするものを設定
            photonTransformView.m_SynchronizePosition = true;
            photonTransformView.m_SynchronizeRotation = true;
            photonTransformView.m_SynchronizeScale = false;
            photonTransformView.m_UseLocal = true;

            // PhotonViewに紐付ける
            photonView.ObservedComponents.Add(photonTransformView);
        }

        return photonView;
    }

}
