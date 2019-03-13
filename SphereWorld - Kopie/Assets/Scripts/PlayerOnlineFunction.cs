using System;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerOnlineFunction : NetworkBehaviour
{
    [SerializeField]
    // Max Health of the Player (Currently set for implementing later functions, like Destroy and Win/Lose conditions)
    private int maxHealth = 1;
    // implemented, for adding future features
    public bool hit = false;

    [SyncVar]
    // This Synchs the Value of the health, to all the connected clients
    private int currentHealth = 1;

    private void Awake()
    {
        // Calls the SetDefault func.
        SetDefaults();
    }

    public void SetDefaults()
    {
        // Set the currentHealth to the given max Health
        currentHealth = maxHealth;
    }

    // Send Command from Client to Server
    [Command]
    public void CmdPlayerGotHitted(string _ID)
    {
        // Register, that Player got hitted
        Debug.Log(_ID + " got hitted");
        // added for features in the next update (coming soon 201... :D)
        hit = true;
        // Get the Player ID
        PlayerOnlineFunction _player = GameManager.GetPlayer(_ID);
        // Player loses life
        _player.TakeDamage(1);
    }

    // Function for decreasing the health
    private void TakeDamage(int v)
    {
        // minus v health
        currentHealth -= v;
        // Debug, that the Player X has X health (currently), after getting damaged
        Debug.Log(transform.name + " now has " + currentHealth + " health");
    }
}
