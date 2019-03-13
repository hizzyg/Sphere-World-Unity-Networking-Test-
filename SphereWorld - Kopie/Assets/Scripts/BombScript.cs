using UnityEngine;

public class BombScript : MonoBehaviour, IPooledObj
{
    /// <summary>
    /// World Script
    /// </summary>
    public World m_World;
    /// <summary>
    /// Player Script
    /// </summary>
    private Player m_player;
    /// <summary>
    /// GameManager Script
    /// </summary>
    private GameManager m_gameManager;

    public void Awake()
    {
        // Find World Script at the Hierarchy
        m_World = FindObjectOfType<World>();
        // Find GameManager Script at the Hierarchy
        m_gameManager = FindObjectOfType<GameManager>();
    }
    public void OnObjectSpawn()
    {
        // Check if playercount is more or equals 2
        if (m_gameManager.m_playerCount >= 2)
        {
            // set the gravity of the bomb
            m_World.Gravity(transform, -800f);
        }
    }
}
