using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks {
  public static Launcher Instance;

  [SerializeField] TMP_InputField playerNameInputField;
  [SerializeField] TMP_Text titleWelcomeText;
  [SerializeField] TMP_InputField roomNameInputField;
  [SerializeField] TMP_InputField maxPlayersInputField;
  [SerializeField] Transform roomListContent;
  [SerializeField] GameObject roomListItemPrefab;
  [SerializeField] TMP_Text roomNameText;
  [SerializeField] Transform playerListContent;
  [SerializeField] GameObject playerListItemPrefab;
  [SerializeField] GameObject startGameButton;
  [SerializeField] TMP_Text errorText;
  
  private void Awake() {
    Instance = this;
  }

  private void Start() {
    Debug.Log("Connecting to master...");
    PhotonNetwork.ConnectUsingSettings();
  }

  public override void OnConnectedToMaster() {
    Debug.Log("Connected to master!");
    PhotonNetwork.JoinLobby();
    // Automatically load scene for all clients when the host loads a scene
    PhotonNetwork.AutomaticallySyncScene = true;
  }

    public override void OnJoinedLobby()
    {
        if (PhotonNetwork.NickName == "")
        {
            PhotonNetwork.NickName = "Player " + Random.Range(0, 1000).ToString(); // Asignamos nombre aleatorio por seguridad
            MenuManager.Instance.OpenMenu("name"); // Abre panel para escribir nombre
        }
        else
        {
            MenuManager.Instance.OpenMenu("title"); // Si ya tiene nombre, va directo al título normal
        }
        Debug.Log("Joined lobby");
    }

    public void SetName()
    {
        string name = playerNameInputField.text;
        if (!string.IsNullOrEmpty(name))
        {
            PhotonNetwork.NickName = name;
            titleWelcomeText.text = $"Bienvenido {name}";

            MenuManager.Instance.OpenMenu("selector");

            playerNameInputField.text = "";
        }
        else
        {
            Debug.Log("No player name entered");
        }
    }

    // Nuevo método para continuar desde el "selector" hacia "title"
    public void ContinueAfterSelector()
    {
        MenuManager.Instance.OpenMenu("title");
    }

    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(roomNameInputField.text) && !string.IsNullOrEmpty(maxPlayersInputField.text))
        {
            string roomName = roomNameInputField.text;
            int maxPlayers = int.Parse(maxPlayersInputField.text);

            RoomOptions options = new RoomOptions();
            options.MaxPlayers = (byte)maxPlayers; // Definir el máximo de jugadores

            PhotonNetwork.CreateRoom(roomName, options); // Crear la sala con nombre y opciones
            MenuManager.Instance.OpenMenu("loading");

            // Limpiar campos
            roomNameInputField.text = "";
            maxPlayersInputField.text = "";
        }
        else
        {
            Debug.Log("Room name or max players not entered");
            // TODO: Mostrar un mensaje de error al usuario
        }
    }

    public override void OnJoinedRoom()
    {
        // Abrir el menú de sala
        MenuManager.Instance.OpenMenu("room");

        // Mostrar el nombre de la sala + el número de jugadores actuales y máximo
        roomNameText.text = PhotonNetwork.CurrentRoom.Name + " " + PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;

        // Limpiar la lista de jugadores
        Player[] players = PhotonNetwork.PlayerList;
        foreach (Transform trans in playerListContent)
        {
            Destroy(trans.gameObject);
        }

        // Instanciar la lista de jugadores
        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void LeaveRoom() {
    PhotonNetwork.LeaveRoom();
    MenuManager.Instance.OpenMenu("loading");
  }

  public void JoinRoom(RoomInfo info) {
    PhotonNetwork.JoinRoom(info.Name);
    MenuManager.Instance.OpenMenu("loading");
  }

  public override void OnLeftRoom() {
    MenuManager.Instance.OpenMenu("title");
  }

  public override void OnRoomListUpdate(List<RoomInfo> roomList) {
    foreach (Transform trans in roomListContent) {
      Destroy(trans.gameObject);
    }
    for (int i = 0; i < roomList.Count; i++) {
      if (roomList[i].RemovedFromList) {
        // Don't instantiate stale rooms
        continue;
      }
      Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
    }
  }

  public override void OnCreateRoomFailed(short returnCode, string message) {
    errorText.text = "Room Creation Failed: " + message;
    MenuManager.Instance.OpenMenu("error");
  }

  public override void OnPlayerEnteredRoom(Player newPlayer) {
    Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
  }

    public void StartGame()
    {
        // 1 is used as the build index of the game scene, defined in the build settings
        // Use this instead of scene management so that *everyone* in the lobby goes into this scene
        PhotonNetwork.LoadLevel(1);
    }

    public void QuitGame() {
    Application.Quit();
  }
}
