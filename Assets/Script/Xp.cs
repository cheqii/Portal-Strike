using System;
using UnityEngine;

public class Xp : MonoBehaviour
{
    public Transform target; // The target to follow (usually the player)
    public float smoothSpeed = 5.0f; // Adjust the smoothness of the camera follow

    private Vector3 offset;

    private void Start()
    {
        target = FindObjectOfType<Player>().transform;
        offset = Vector3.zero;
    }

    private void LateUpdate()
    {
        // Calculate the desired camera position
        Vector3 targetPosition = target.position + offset;

        // Keep the y-axis position constant
        targetPosition.y = transform.position.y;

        // Smoothly move the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
}