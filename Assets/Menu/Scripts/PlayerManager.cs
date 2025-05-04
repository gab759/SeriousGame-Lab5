using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerManager : MonoBehaviour {
  PhotonView pv;

  private void Awake() {
    pv = GetComponent<PhotonView>();
  }

  private void Start() {
    if (pv.IsMine) {
      CreateController();
    }
  }

    private void CreateController()
    {
        int personajeIndex = PlayerPrefs.GetInt("PersonajeSeleccionado", 0);

        string[] personajeNombres = new string[] {
            "PlayerController",
            "PlayerControllerCube",
            "PlayerControllerCylinder"
        };

        string prefabName = personajeNombres[personajeIndex];
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", prefabName), Vector3.zero, Quaternion.identity);
    }
}
