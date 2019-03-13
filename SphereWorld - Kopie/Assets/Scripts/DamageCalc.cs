using UnityEngine;
using UnityEngine.Networking;

public class DamageCalc : NetworkBehaviour
{
    /// <summary>
    /// Bomb Script
    /// </summary>
    private BombScript m_bombScript;
    /// <summary>
    /// Player Online Funktion (NetworkBehaviour(Script) of the Player)
    /// </summary>
    private PlayerOnlineFunction m_playerOnline;
    /// <summary>
    /// Behaviour of Bomb Spawn
    /// </summary>
    public Behaviour m_BombSpawn;

    private void Awake()
    {
        // Find the BombScript at the Hierarchy
        m_bombScript = FindObjectOfType<BombScript>();
    }

    private void Update()
    {
        // if playerOnline equals null, find playeronlinefunction at the hierarchy
        if (m_playerOnline == null)
            m_playerOnline = FindObjectOfType<PlayerOnlineFunction>();
    }

    // NetworkBehaviour Script, that runs only on the Clients
    [Client]
    void OnCollisionEnter(Collision _col)
    {
        // if bomb collides with player
        if (_col.gameObject.tag == "Player")
        {
            // health of the player decreases
            m_playerOnline.CmdPlayerGotHitted(_col.gameObject.name);
        }
    }
}
