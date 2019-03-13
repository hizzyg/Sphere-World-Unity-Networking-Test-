using UnityEngine;

public class Player : MonoBehaviour
{
    /// <summary>
    /// Movement Speed
    /// </summary>
    public float m_MoveSpeed = 25f;
    /// <summary>
    /// Angle Rotation Speed
    /// </summary>
    public float m_AngleSpeed = 2.5f;
    /// <summary>
    /// Distance to the ground
    /// </summary>
    private float m_distToGround;
    /// <summary>
    /// Direction (Vector)
    /// </summary>
    private Vector3 m_dir;
    /// <summary>
    /// Game Manager
    /// </summary>
    private GameManager m_gameManager;
    /// <summary>
    /// RigidBody of the Player
    /// </summary>
    private Rigidbody m_rb;
    /// <summary>
    /// Transform of the Player
    /// </summary>
    private Transform m_transform;
    /// <summary>
    /// World (Script) Gravity 
    /// </summary>
    public World m_Gravity;

    private void Awake()
    {
        // Get Component of Rigidbody
        m_rb = GetComponent<Rigidbody>();
        // Get Component of Transform
        m_transform = GetComponent<Transform>();
        // Find the World Script at the hierarchy
        m_Gravity = FindObjectOfType<World>();
        // Find the GameManager Script at the hierarchy
        m_gameManager = FindObjectOfType<GameManager>();
    }
    void Start()
    {
        // Count the Player UP
        m_gameManager.m_playerCount++;
        // Call the PhysicCondition Script
        PhysicConditions();
        // Ignore Collision between Player and Player
        Physics.IgnoreLayerCollision(8, 8);
    }

    void FixedUpdate()
    {
        // Call the Movement Script
        Movement();
    }

    void PhysicConditions()
    {
        // The default gravity is false
        m_rb.useGravity = false;
        // Freeze all rotations of the player
        m_rb.constraints = RigidbodyConstraints.FreezeRotation;
        //distance to ground
        m_distToGround = GetComponent<BoxCollider>().bounds.extents.y;
    }

    // Check if player is grounded
    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, m_distToGround + 0.1f);
    }

    // The movement Script
    void Movement()
    {
        // Let the Car move always forward
        m_rb.MovePosition(m_rb.position + transform.TransformDirection(Vector3.forward) * m_MoveSpeed * Time.fixedDeltaTime);

        // Rotate to left

        if (Input.GetKey(KeyCode.A))
        {
            m_transform.Rotate(0, -20 * Time.fixedDeltaTime * m_AngleSpeed, 0);
        }
        // Rotate to right
        if (Input.GetKey(KeyCode.D))
        {
            m_transform.Rotate(0, 20 * Time.fixedDeltaTime * m_AngleSpeed, 0);
        }
        // if gravity of the world script exists, use it
        if (m_Gravity)
        {
            m_Gravity.Gravity(transform);
        }
    }
}
