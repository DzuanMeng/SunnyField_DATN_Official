using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour, IDamageable
{
    [SerializeField] int hp = 10;

    public void ApplyDamage(int damage)
    {
        hp -= damage;
    }

    public void CalculateDamage(ref int damage)
    {
    }

    public void CheckState()
    {
        if (hp <= 0)
        {
            Animator anim = GetComponentInChildren<Animator>();
            if (anim != null)
            {
                anim.SetTrigger("die");
            }
            Destroy(gameObject, 1.0f);
        }
    }
}
