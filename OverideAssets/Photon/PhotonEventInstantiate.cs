using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonEventInstantiate : MonoBehaviour, IOnEventCallback
{
    private const byte CustomInstantiateEventCode = 1;
    [Header("�l�b�g���[�N��ʂ��Đ�������Prefab�͑S�Ă��̒��ɂ���Ă���")]
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

    // RaiseEvent���󂯎��
    public void OnEvent(EventData photonEvent)
    {
        if(PhotonNetwork.IsConnected == false) return;

        var eventCode = photonEvent.Code;

        if (eventCode != CustomInstantiateEventCode)
        {
            return;
        }

        var data = (object[])photonEvent.CustomData;

        // ��M����transform��ݒ�
        var obj = Instantiate(GetNetworkPrefab((string)data[0]), (Vector3)data[1], (Quaternion)data[2]);
        obj.GetComponent<PhotonObjectSynchronizer>().objectClient = true;

        // Photon
        var photonView = obj.GetComponent<PhotonView>();

        // ��M����ViewID��p���ē�������
        photonView.ViewID = (int)data[3];

        generatingPrefabName = null;
    }
}
