using Photon.Pun;
using UnityEngine;
using Photon.Realtime;
using Unity.VisualScripting;

public class TestConnect : MonoBehaviourPunCallbacks
{
    void Start()
    {
        print("Connecting to server.");
        PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }
        
    public override void OnConnectedToMaster()
    {
        print("Connecting to server.");
        print(PhotonNetwork.LocalPlayer.NickName);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        print("Disconnected from server for reason " + cause.ToString());
    }
}
