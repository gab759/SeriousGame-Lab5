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
        Debug.Log("Connected to Photon.", this);
            
        Debug.Log("My nickname is " + PhotonNetwork.LocalPlayer.NickName, this);
        if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Failed to connect to Proton: " + cause.ToString(), this);
    }
    public override void OnJoinedLobby()
    {
        print("Joined lobby");
    }
}
