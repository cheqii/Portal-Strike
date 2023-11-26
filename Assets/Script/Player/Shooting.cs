using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] private float fireRate = 0.1f;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootingPoint;

    private float shotTime;

    [Header("Muzzle Flash")] 
    [SerializeField] private ParticleSystem muzzleFlash;
    
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
        muzzleFlash.Play(true);
        // Create {bullet} in the location of {shootingPoint.position}
        GameObject bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.gameObject.transform.rotation);

        // Get the Rigidbody component attached to the {bullet} GameObject
        Rigidbody bullet_rb = bullet.GetComponent<Rigidbody>();

        // Calculate the direction and speed.
        bullet_rb.velocity = (bullet.transform.forward * bulletSpeed);
    }
}