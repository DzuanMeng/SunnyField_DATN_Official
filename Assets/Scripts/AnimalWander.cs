using UnityEngine;

public class ChickWander : MonoBehaviour
{
    [Header("Cấu hình di chuyển")]
    [SerializeField] float speed = 1.5f;
    [SerializeField] float wanderRadius = 3f;
    [SerializeField] float wanderTime = 2f;
    [SerializeField] float stayTime = 3f;

    [Header("Cấu hình âm thanh")]
    [SerializeField] AudioClip cluckSound;
    [Range(0, 1)]
    [SerializeField] float cluckChance = 0.5f;

    private Vector3 startPosition;
    private float timer;
    private bool isWalking;
    private Vector3 direction;
    private Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        startPosition = transform.position;
        SwitchState();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0) SwitchState();

        if (isWalking)
        {
            if (Vector3.Distance(startPosition, transform.position) > wanderRadius)
            {
                direction = (startPosition - transform.position).normalized;
                UpdateAnimator(direction);
            }
            transform.position += direction * speed * Time.deltaTime;
        }
    }

    void SwitchState()
    {
        isWalking = !isWalking;

        if (isWalking)
        {
            Vector2 randomPoint = Random.insideUnitCircle * wanderRadius;
            Vector3 targetPosition = startPosition + new Vector3(randomPoint.x, randomPoint.y, 0);
            direction = (targetPosition - transform.position).normalized;
            timer = wanderTime;
            UpdateAnimator(direction);
        }
        else
        {
            timer = stayTime;

            PlayCluckSound();
        }

        if (anim != null) anim.SetBool("isWalking", isWalking);
    }

    void PlayCluckSound()
    {
        if (cluckSound != null && AudioManager.instance != null)
        {
            if (Random.value < cluckChance)
            {
                AudioManager.instance.Play(cluckSound);
            }
        }
    }

    void UpdateAnimator(Vector3 dir)
    {
        if (anim != null)
        {
            anim.SetFloat("moveX", dir.x);
            anim.SetFloat("moveY", dir.y);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(Application.isPlaying ? startPosition : transform.position, wanderRadius);
    }
}