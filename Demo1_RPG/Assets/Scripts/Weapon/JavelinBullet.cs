using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JavelinBullet : MonoBehaviour
{
    public int attackValue = 30;
    private Rigidbody rb;
    private Collider col;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == Tag.PLAYER) return;
        rb.isKinematic = true;
        col.enabled = false;

        transform.parent = collision.gameObject.transform;

        if (collision.gameObject.tag == Tag.ENEMY)
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(attackValue);
        }

        Destroy(this.gameObject, 1f);
    }
}
