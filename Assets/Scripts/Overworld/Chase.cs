using UnityEngine;

public class Chase : MonoBehaviour
{
    public float speed;
    public float detectionRange = 5f;
    public float chaseDuration = 5f;

    private Transform target;
    public bool isChasing = false;
    private float chaseTimer = 0f;

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
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

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
