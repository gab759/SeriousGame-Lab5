using System;
using UnityEngine;
using Photon.Pun;

public class PlayerShooting : MonoBehaviourPun
{
    public Transform firePoint;
    private Camera mainCamera;
    public float firePointDistance = 1.5f;
    void Start()
    {
        if (!photonView.IsMine) return;
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        UpdateFirePoint();

        if (InputManager.Instance.playerFiredThisFrame())
        {
            Vector3 shootDirection = mainCamera.transform.forward;
            photonView.RPC("ShootRPC", RpcTarget.AllViaServer, shootDirection);
        }
    }
    void UpdateFirePoint()
    {
        Vector3 lookDirection = mainCamera.transform.forward;
        firePoint.position = transform.position + lookDirection.normalized * firePointDistance;
        firePoint.rotation = Quaternion.LookRotation(lookDirection);
    }

    [PunRPC]
    private void ShootRPC(Vector3 shootDirection, PhotonMessageInfo info)
    {
        if (photonView.IsMine || PhotonNetwork.LocalPlayer.ActorNumber == info.Sender.ActorNumber)
        {
            GameObject bullet = PhotonNetwork.Instantiate(
                "PhotonPrefabs/Bullet",
                firePoint.position,
                Quaternion.LookRotation(shootDirection)
            );
            bullet.GetComponent<Bullet>().SetDirection(shootDirection);

        }
    }
}