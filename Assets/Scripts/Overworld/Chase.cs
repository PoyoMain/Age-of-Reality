using UnityEngine;

public class Chase : MonoBehaviour
{
    public float speed;
    public float detectionRange = 5f;
    public float chaseDuration = 5f;
    public float velocityCapX;
    public float velocityCapY;

    private Transform target;
    private Animator _anim;
    private Rigidbody2D rigid;

    public bool isChasing = false;
    private float chaseTimer = 0f;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        // Check if the player is within the detection range and start chasing
        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
            chaseTimer = 0f;
        }
        else
        {
            isChasing = false;
        }

        // If chasing, move towards the player
        if (isChasing)
        {
            Vector3 ogPos = transform.position;
            //transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            rigid.AddForce(speed * Time.deltaTime * (target.position - transform.position), ForceMode2D.Impulse);
            float newVelX = Mathf.Clamp(rigid.velocity.x, -velocityCapX, velocityCapX);
            float newVelY = Mathf.Clamp(rigid.velocity.y, -velocityCapY, velocityCapY);
            Vector2 vel = new(newVelX, newVelY);
            rigid.velocity = vel;

            Vector2 disp = ogPos - transform.position;

            if (Mathf.Abs(rigid.velocity.x) > Mathf.Abs(rigid.velocity.y))
            {
                _anim.SetFloat("X", rigid.velocity.x);
                _anim.SetFloat("Y", 0);
            }
            else
            {
                _anim.SetFloat("X", 0);
                _anim.SetFloat("Y", rigid.velocity.y);
            }
            


            // Increment timer
            chaseTimer += Time.deltaTime;

            // If chase duration has exceeded, stop chasing
            if (chaseTimer >= chaseDuration)
            {
                isChasing = false;
            }
        }
    }
}
