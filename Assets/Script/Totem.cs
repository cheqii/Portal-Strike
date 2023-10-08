using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform shootingPoint;

    private Transform player;
    private float distance;
    [SerializeField]  private float delay = 4;
    [SerializeField] private float shootDistance = 5f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().gameObject.transform;
        StartCoroutine(IEnu_TotemShoot());
    }

    // Update is called once per frame
    void FixedUpdate()
    { 
        distance = Vector3.Distance (this.transform.position, player.position);
    }
    

    IEnumerator IEnu_TotemShoot()
    {
        while (true)
        {
            if (distance < shootDistance)
            {
                GameObject totem_bullet = Instantiate(bullet, shootingPoint.position, Quaternion.identity);
                totem_bullet.GetComponent<BossBullet>().target = player;
            }
            yield return new WaitForSeconds(delay);
        }
    }
}
