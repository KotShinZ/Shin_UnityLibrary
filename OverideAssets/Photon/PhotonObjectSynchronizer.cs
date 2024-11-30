using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

[RequireComponent(typeof(PhotonView))]
public class PhotonObjectSynchronizer : MonoBehaviour
{
    private const byte CustomInstantiateEventCode = 1;
    public bool isHostOnly = false;
    public bool isThisPrefabName = true;
    public string prefab_name;
    public static List<string> instantiatedViewIDs;

    protected PhotonView photonView;

    public bool objectClient = false;

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsConnected == false) return;
        if(isHostOnly && PhotonNetwork.IsMasterClient == false) return;
        if (objectClient) return;
        

        photonView = GetComponent<PhotonView>();
         PhotonNetwork.AllocateViewID(photonView);

        var obj = gameObject;

        // Prefab��transform��ViewID��ʒm���鏀��������
        var data = new object[]
        {
                GetPrefabName(),
                obj.transform.position,
                obj.transform.rotation,
                photonView.ViewID,
        };

        // ����Room�̎����ȊO�ɒʒm
        var raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.Others,
            CachingOption = EventCaching.AddToRoomCache
        };

        var sendOptions = new SendOptions
        {
            Reliability = true
        };

        // ����Room���̑��̃��[�U�[�֒ʒm
        PhotonNetwork.RaiseEvent(CustomInstantiateEventCode, data, raiseEventOptions, sendOptions);
    }

    public string GetPrefabName()
    {
        return isThisPrefabName ? RemoveTrailingNumberInParentheses(gameObject.name) : prefab_name;
    }

    // ������̖����ɂ��� (����) ����菜���֐�
    public static string RemoveTrailingNumberInParentheses(string input)
    {
        // ���ʂƂ��̒��g���폜���鐳�K�\��
        string pattern = @"\([^()]*\)";

        // ����q�ɂȂ������ʂɂ��Ή�
        while (Regex.IsMatch(input, pattern))
        {
            input = Regex.Replace(input, pattern, "");
        }

        // �]���ȋ󔒂��폜
        return input.Trim();
    }
}
