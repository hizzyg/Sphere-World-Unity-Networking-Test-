using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    // The gravity value
    public float m_Gravity = -10;

    public void Gravity(Transform _transform)
    {
        // Get the difference between world and player
        Vector3 posUp = (_transform.position - transform.position).normalized;
        // Get the Y Axis in the World cor.
        Vector3 locUp = _transform.up;

        _transform.GetComponent<Rigidbody>().AddForce(posUp * m_Gravity);

        // Create rotation from from direction to direction
        Quaternion quaternion = Quaternion.FromToRotation(locUp,posUp) * _transform.rotation;
        _transform.rotation = Quaternion.Slerp(_transform.rotation, quaternion, 30f * Time.deltaTime);
    }

    public void Gravity(Transform _transform, float _gravity)
    {
        // Get the difference between world and player
        Vector3 posUp = (_transform.position - transform.position).normalized;
        // Get the Y Axis in the World cor.
        Vector3 locUp = _transform.up;

        _transform.GetComponent<Rigidbody>().AddForce(posUp * _gravity);

        // Create rotation from from direction to direction
        Quaternion quaternion = Quaternion.FromToRotation(locUp, posUp) * _transform.rotation;
        _transform.rotation = Quaternion.Slerp(_transform.rotation, quaternion, 30f * Time.deltaTime);
    }
}
