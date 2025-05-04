using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float gravityValue = -9.81f;

    private CharacterController controller;
    private PhotonView pv;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        pv = GetComponent<PhotonView>();

        if (!pv.IsMine)
        {
            Destroy(GetComponentInChildren<CinemachineVirtualCamera>().gameObject);
            Destroy(GetComponentInChildren<CharacterController>());
            Destroy(GetComponentInChildren<Rigidbody>());
        }

        Cursor.visible = false;
        cameraTransform = Camera.main.transform;

        Transform nameTag = transform.Find("UserNameText");
        if (nameTag != null)
        {
            TextMeshPro tmp = nameTag.GetComponent<TextMeshPro>();
            if (tmp != null)
            {
                tmp.text = pv.Owner.NickName;
            }
        }
    }

    void Update()
    {
        if (!pv.IsMine)
        {
            return;
        }

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        Transform nameTag = transform.Find("UserNameText");
        if (nameTag != null)
        {
            nameTag.rotation = Quaternion.identity;
        }
        Vector2 movement = InputManager.Instance.GetPlayerMovement();
        Vector3 move = new Vector3(movement.x, 0f, movement.y);
        move = cameraTransform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (InputManager.Instance.playerJumpedThisFrame() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}