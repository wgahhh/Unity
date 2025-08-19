using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Javelin : Weapon
{
    public float bulletSpeed;
    public GameObject bulletPrefab;
    private GameObject bulletGo;

    private void Start()
    {
        SpawnBullet();
    }
    public override void Attack()
    {
        if (bulletGo != null)
        {
            bulletGo.GetComponent<Collider>().enabled = true;
            bulletGo.transform.parent = null;
            bulletGo.GetComponent<Rigidbody>().velocity = transform.forward * bulletSpeed;
            Destroy(bulletGo.gameObject, 10);
            bulletGo = null;
            Invoke("SpawnBullet", 0.5f);
        }
        else
        {
            return;
        }
    }

    private void SpawnBullet()
    {
        bulletGo = GameObject.Instantiate(bulletPrefab, transform.position, transform.rotation);
        bulletGo.GetComponent<Collider>().enabled = false;
        bulletGo.transform.parent = transform;
        if (tag == Tag.INTERACTABLE)
        {
            Destroy(bulletGo.GetComponent<JavelinBullet>());
            bulletGo.tag = Tag.INTERACTABLE;

            PickableObject po = bulletGo.AddComponent<PickableObject>();
            po.item = GetComponent<PickableObject>().item;

            Collider collider = bulletGo.GetComponent<Collider>();
            collider.enabled = true;

            Rigidbody rb = bulletGo.GetComponent <Rigidbody>();
            rb.constraints = ~RigidbodyConstraints.FreezePositionY;
            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }
}
