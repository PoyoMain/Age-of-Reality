using UnityEngine;

public class Chase : MonoBehaviour
{
    public float speed;
    public float detectionRange = 5f;
    public float chaseDuration = 5f;

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

        // If chasing, move towards the player
        if (isChasing)
        {
            Vector3 ogPos = transform.position;
            //transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            rigid.AddForce((target.position - transform.position) * speed * Time.deltaTime, ForceMode2D.Impulse);

            Vector2 disp = transform.position - ogPos;
            _anim.SetFloat("X", disp.x);
            _anim.SetFloat("Y", disp.y);


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
