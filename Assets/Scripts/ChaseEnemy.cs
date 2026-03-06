using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseEnemy : MonoBehaviour
{
    Transform player;
    [SerializeField] float speed;
    [SerializeField] Vector2 attackSize = Vector2.one;
    [SerializeField] int damage = 1;
    [SerializeField] float timeToAttack = 2f;
    float attackTimer;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance.player.transform;
        attackTimer = Random.Range(0, timeToAttack);

        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

        if (anim != null)
        {
            anim.SetFloat("moveX", direction.x);
            anim.SetFloat("moveY", direction.y);

        }
        Attack();
    }

    private void Attack()
    {
        attackTimer -= Time.deltaTime;

        if (attackTimer > 0f) { return; }

        attackTimer = timeToAttack;

        if (anim != null)
        {
            anim.SetTrigger("attack");
        }

        Collider2D[] targets = Physics2D.OverlapBoxAll(transform.position, attackSize, 0f);

        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i].gameObject == this.gameObject) { continue; }

            if (targets[i].CompareTag("Player"))
            {
                Damageable character = targets[i].GetComponent<Damageable>();
                if (character != null)
                {
                    character.TakeDamage(damage);
                }
            }
        }
    }
}
