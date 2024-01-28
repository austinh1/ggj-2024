using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : Hat
{
    public Rigidbody projectile;
    public float speed = 4;
    private bool lockedAndLoaded = false;
    public float fireRate = 1.5f;
    public float upVelocity = 1f;

    private void Update()
    {
        if (equipped && !lockedAndLoaded)
        {
            lockedAndLoaded = true;
            InvokeRepeating("Fire", 0, fireRate);
        }
    }

    void Fire()
    {
        Rigidbody p = Instantiate(projectile, transform.position, transform.rotation);
        p.transform.rotation = Quaternion.FromToRotation(Vector3.down, p.transform.forward);
        p.velocity = transform.forward * speed + transform.up * upVelocity;
        StartCoroutine(DestroyProjectile(p.gameObject));
    }

    IEnumerator DestroyProjectile(GameObject bullet)
    {
        yield return new WaitForSeconds(10);
        Destroy(bullet);
    }
}
