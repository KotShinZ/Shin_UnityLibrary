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

        // PrefabのtransformとViewIDを通知する準備をする
        var data = new object[]
        {
                GetPrefabName(),
                obj.transform.position,
                obj.transform.rotation,
                photonView.ViewID,
        };

        // 同じRoomの自分以外に通知
        var raiseEventOptions = new RaiseEventOptions
        {
            Receivers = ReceiverGroup.Others,
            CachingOption = EventCaching.AddToRoomCache
        };

        var sendOptions = new SendOptions
        {
            Reliability = true
        };

        // 同じRoom内の他のユーザーへ通知
        PhotonNetwork.RaiseEvent(CustomInstantiateEventCode, data, raiseEventOptions, sendOptions);
    }

    public string GetPrefabName()
    {
        return isThisPrefabName ? RemoveTrailingNumberInParentheses(gameObject.name) : prefab_name;
    }

    // 文字列の末尾にある (数字) を取り除く関数
    public static string RemoveTrailingNumberInParentheses(string input)
    {
        // 括弧とその中身を削除する正規表現
        string pattern = @"\([^()]*\)";

        // 入れ子になった括弧にも対応
        while (Regex.IsMatch(input, pattern))
        {
            input = Regex.Replace(input, pattern, "");
        }

        // 余分な空白を削除
        return input.Trim();
    }
}
