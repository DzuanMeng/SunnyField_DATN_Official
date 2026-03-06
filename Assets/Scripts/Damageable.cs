using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    IDamageable damageable;

    [Header("Audio")]
    [SerializeField] AudioClip hitSound;

    internal void TakeDamage(int damage)
    {
        if ( damageable == null)
        {
            damageable = GetComponent<IDamageable>();
        }
        damageable.CalculateDamage(ref damage);
        damageable.ApplyDamage(damage);
        GameManager.instance.messageSystem.PostMessage(transform.position, damage.ToString());

        Animator anim = GetComponentInChildren<Animator>();

        if (anim != null)
        {
            anim.SetTrigger("hurt");
        }

        damageable.CheckState();

        if (hitSound != null)
        {
            AudioManager.instance.Play(hitSound);
        }
    }
}
