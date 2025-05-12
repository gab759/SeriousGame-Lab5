using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class HealthPlayer : MonoBehaviourPun
{
    public Slider healthBar;
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    [PunRPC]
    public void TakeDamage(float damage)
    {
        if (photonView.IsMine)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            photonView.RPC("UpdateHealthBar", RpcTarget.AllViaServer, currentHealth);

            if (currentHealth <= 0)
            {
                Debug.Log("Player " + photonView.Owner.NickName + " died.");
            }
        }
    }

    [PunRPC]
    void UpdateHealthBar(float newHealth)
    {
        currentHealth = newHealth;
        healthBar.value = currentHealth;
    }
}