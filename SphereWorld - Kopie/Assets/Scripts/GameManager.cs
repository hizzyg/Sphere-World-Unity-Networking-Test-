using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Player Count
    /// </summary>
    public int m_playerCount = 0;
    /// <summary>
    /// Player Script
    /// </summary>
    public Player m_player;

    const string PLAYER_ID_PREFIX = "Player ";

    /// <summary>
    /// Player Dictionary
    /// </summary>
    private static Dictionary<string, PlayerOnlineFunction> m_players = new Dictionary<string, PlayerOnlineFunction>();

    public static void RegisterPlayer(string _netID, PlayerOnlineFunction _player)
    {
        // (string) Player ID
        string _playerID = PLAYER_ID_PREFIX + _netID;
        // Add to players Dictionary
        m_players.Add(_playerID, _player);
        // Set transform name to player id (Player + ID)
        _player.transform.name = _playerID;
    }

    public static void UnRegisterPlayer(string _playerID)
    {
        // Remove player from dict.
        m_players.Remove(_playerID);
    }

    // GetPlayer
    public static PlayerOnlineFunction GetPlayer(string _playerID)
    {
        return m_players[_playerID];
    }
}