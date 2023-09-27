using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject _bullet;

    public void BulletShoot()
    {
        var bullet = Instantiate(_bullet, transform.position,transform.parent.rotation);
    }
}
