using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5.0f; // Adjust the forward speed as needed

    void Update()
    {
        // Calculate the forward movement vector
        Vector3 moveDirection = transform.forward * speed * Time.deltaTime;

        // Move the object forward
        transform.Translate(moveDirection, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(this.gameObject);
        }
    }
}