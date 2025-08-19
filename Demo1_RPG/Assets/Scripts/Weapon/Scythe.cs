using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Scythe : Weapon
{
    public const string ANIM_PARM_ISATTACK = "isAttack";

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        attackValue = 50;
    }

    public override void Attack()
    {
        anim.SetTrigger(ANIM_PARM_ISATTACK);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Tag.ENEMY)
        {
            other.GetComponent<Enemy>().TakeDamage(attackValue);
        }
    }
}
