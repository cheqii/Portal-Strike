using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootingPoint;

    private float shotTime;

    /* Ensures that the Fire() function can only be called once every {fireRate} seconds
     * to control the rate of shooting or firing. */
    public void Perform()
    {
        if (Time.time - shotTime >= fireRate)
        {
            Fire();
            shotTime = Time.time;
        }
    }

    private void Fire()
    {
        // Create {bullet} in the location of {shootingPoint.position}
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);

        // Get the Rigidbody component attached to the {bullet} GameObject
        Rigidbody rb = bullet.GetComponent<Rigidbody>();

        // Calculate the direction and speed.
        rb.velocity = (transform.forward * bulletSpeed);
    }
}