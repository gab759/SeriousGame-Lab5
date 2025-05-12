using UnityEngine;
using Photon.Pun;

public class Bullet : MonoBehaviourPun
{
    public float speed = 10f;
    public float damage = 20f;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetDirection(Vector3 dir)
    {
        if (!photonView.IsMine) return;

        rb.linearVelocity = dir.normalized * speed;
        Invoke(nameof(DestroyBullet), 2f);
    }

    void Update()
    {
            rb.linearVelocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine) return;

        PhotonView targetView = other.GetComponentInParent<PhotonView>();
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit player: " + targetView.Owner.NickName);
            targetView.RPC("TakeDamage", targetView.Owner, damage);
        }

        PhotonNetwork.Destroy(gameObject);
    }

    void DestroyBullet()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Destroy(gameObject);
        }
    }
}