using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public class RPC_Receiver : MonoBehaviourPunCallbacks
{
    public UnityEvent RPC1 = new UnityEvent();
    public UnityEvent RPC2 = new UnityEvent();
    public UnityEvent RPC3 = new UnityEvent();
    public UnityEvent RPC4 = new UnityEvent();
    public UnityEvent RPC5 = new UnityEvent();

    [PunRPC]
    public void RPC_1()
    {
        RPC1.Invoke();
    }

    [PunRPC]
    public void RPC_2()
    {
        RPC2.Invoke();
    }

    [PunRPC]
    public void RPC_3()
    {
        RPC3.Invoke();
    }

    [PunRPC]
    public void RPC_4()
    {
        RPC4.Invoke();
    }

    [PunRPC]
    public void RPC_5()
    {
        RPC5.Invoke();
    }
}
