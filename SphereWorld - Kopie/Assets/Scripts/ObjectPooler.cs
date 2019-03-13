using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ObjectPooler : NetworkBehaviour
{
    #region SINGLETON

    public static ObjectPooler Instance;
    private void Awake()
    {
        Instance = this;
        m_gameManager = FindObjectOfType<GameManager>();
    }
    #endregion

    /// <summary>
    /// Game Manager
    /// </summary>
    GameManager m_gameManager;
    /// <summary>
    /// Call it once
    /// </summary>
    bool m_once = true;
    /// <summary>
    /// Timer for Bomb Spawn
    /// </summary>
    float m_timer = 1f;
    /// <summary>
    /// Dictionary for Pool Objects
    /// </summary>
    public Dictionary<string, Queue<GameObject>> m_PoolDictionary = new Dictionary<string, Queue<GameObject>>();
    /// <summary>
    /// List of Pools
    /// </summary>
    public List<Pool> m_Pools = new List<Pool>();

    [System.Serializable]
    public class Pool
    {
        /// <summary>
        /// Pool Tag
        /// </summary>
        public string m_Tag;
        /// <summary>
        /// Pool Prefab (as GameObject)
        /// </summary>
        public GameObject m_Prefab;
        /// <summary>
        /// Prefab Size (Length)
        /// </summary>
        public int m_Size;
    }

    public void Update()
    {
        // Check if it the localPlayer
        if (isLocalPlayer)
        {
            return;
        }
        // else call the Command for PoolSpawn and BombSpawner
        else
        {
            CmdPoolSpawn();
            BombSpawner();
        }
    }

    [Command]
    public void CmdPoolSpawn()
    {
        // Check if the PlayerCount is more or equals 2 & call the function once
        if (m_gameManager.m_playerCount >= 2 && m_once == true)
        {
            // Checks all pools ind the Pools List
            foreach (Pool pools in m_Pools)
            {
                // Adds object Pools as GameObjects in the Queue
                Queue<GameObject> objectPool = new Queue<GameObject>();

                for (int i = 0; i < pools.m_Size; i++)
                {
                    // Check if it the Server
                    if (this.isServer)
                    {
                        // Instantiate GameObjects from the pool List
                        GameObject obj = Instantiate(pools.m_Prefab);
                        // Spawns the GameObjects on the Server
                        NetworkServer.Spawn(obj);
                        // Set the obj (Objects) ACTIVE
                        obj.SetActive(false);
                        // Enqueue the obj
                        objectPool.Enqueue(obj);
                    }
                    else
                    {
                        // If you are not the Server, u will chill your life :))
                        Debug.Log("This is not the Server");
                    }
                }
                // Fill the Pool Dictionary
                m_PoolDictionary.Add(pools.m_Tag, objectPool);
            }
            m_once = false;
        }
    }

    public GameObject SpawnFromPool(string _tag, Vector3 _pos, Quaternion _rot)
    {
        // Check if the tag is the right one, else return null
        if (!m_PoolDictionary.ContainsKey(_tag))
        {
            Debug.LogWarning("Pool with tag: " + _tag + "doesn't exist");
            return null;
        }
        // save the objects to spawn
        GameObject objectToSpawn = m_PoolDictionary[_tag].Dequeue();
        
        // set the objtospawn true
        objectToSpawn.SetActive(true);
        // set the transform
        objectToSpawn.transform.position = _pos;
        // set the rotation
        objectToSpawn.transform.rotation = _rot;

        // set the interface
        IPooledObj pooledObj = objectToSpawn.GetComponent<IPooledObj>();
        if (pooledObj != null)
        {
            // if the interface is not null, call OnObjectSpawn
            pooledObj.OnObjectSpawn();
        }
        // Enqueue the objtospawn
        m_PoolDictionary[_tag].Enqueue(objectToSpawn);
        // return the objtospawn
        return objectToSpawn;
    }

    public void BombSpawner()
    {
        // check if playercount is more or equals two
        if (m_gameManager.m_playerCount >= 2)
        {
            // check if timer is 1
            if (m_timer == 1f)
            {
                Vector3 pos = Random.onUnitSphere * 40;
                SpawnFromPool("Bomb", pos, Quaternion.Euler(Vector3.one));
                m_timer = 0;
            }
            // else if timer is 0 or less than 1
            else if (m_timer == 0 || m_timer < 1f)
            {
                // increase timer
                m_timer += Time.deltaTime;
                // if timer is bigger than 1, set timer to 1
                if (m_timer > 1f)
                {
                    m_timer = 1f;
                }
            }
        }
    }

}
