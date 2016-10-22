using UnityEngine;
using UnityEngine.Networking;

public class Combat : NetworkBehaviour
{
    public const int maxHealth = 100;

    [SyncVar]
    public int health = maxHealth;

    [Command]
    public void CmdSetHealth(int _health)
    {
        CommandLogger.Log(this, _health);
        health = _health;
    }


    [Command]
    public void CmdTakeDamage(int amount)
    {
        CommandLogger.Log(this, amount);
        health -= amount;
        if (health <= 0)
        {
            CmdSetHealth(maxHealth);
            ViveAvatar.me.controllers[ViveAvatar.LEFT].LongHaptic(90);
            ViveAvatar.me.controllers[ViveAvatar.RIGHT].LongHaptic(90);
        }
    }

    void Update()
    {
        if (GetComponent<FightPlayer>().healthBar)
            GetComponent<FightPlayer>().healthBar.GetComponent<TextMesh>().text = health.ToString();
    }

    public void OnStartRecording()
    {
        // When recording starts, we want to send mock commands so the starting state is saved.
        CmdSetHealth(health);
    }
}
