using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Player))]
public class PlayerNetwork : NetworkBehaviour
{
    /// <summary>
    /// Components to Dissable
    /// </summary>
    [SerializeField]
    Behaviour[] compToDis;
    /// <summary>
    /// Lobby Camera
    /// </summary>
    Camera lobbyCam;

    private void Start()
    {
        // Check if localplayer
        if (!isLocalPlayer)
        {
            for (int i = 0; i < compToDis.Length; i++)
            {
                // disable all components to dissable
                compToDis[i].enabled = false;
            }
        }
        // else, you must be at the lobby
        else
        {
            // set the main cam
            lobbyCam = Camera.main;
            if (lobbyCam != null)
            {
                lobbyCam.gameObject.SetActive(false);
            }
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        // set the netID
        string _netID = GetComponent<NetworkIdentity>().netId.ToString();
        // create player
        PlayerOnlineFunction _player = GetComponent<PlayerOnlineFunction>();
        // register the player
        GameManager.RegisterPlayer(_netID, _player);
    }

    private void OnDisable()
    {
        if (lobbyCam != null)
        {
            lobbyCam.gameObject.SetActive(true);
        }
        // unregister the player on disable
        GameManager.UnRegisterPlayer(transform.name);
    }

    private void OnPlayerDisconnected(NetworkIdentity player)
    {
        if (isServer)
        {
            // Stop the servr, if host disconnects
            NetworkManager.singleton.StopHost();
        }
        // after 2018.2 it is to strong to find a solution for catching timeout error, this must be fixed
    }
}
