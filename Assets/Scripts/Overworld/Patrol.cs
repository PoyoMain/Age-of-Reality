using UnityEngine;

public class Patrol : MonoBehaviour
{

    public float speed;
    private float waitTime;
    public float startWaitTime;

    public Transform[] moveSpots;
    private int randomSpot;
    public Chase chaseRef;
    public bool chasing;

    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    void Start()
    {
        waitTime = startWaitTime;
        randomSpot = Random.Range(0, moveSpots.Length);
    }


    void Update()
    {
        chasing = chaseRef.isChasing;
        if (!chasing)
        {
            transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);
            Vector2 disp = moveSpots[randomSpot].position - transform.position;

            _anim.SetBool("isMoving", true);
            if (transform.position != moveSpots[randomSpot].position)
            {
                _anim.SetFloat("X", disp.x);
                _anim.SetFloat("Y", disp.y);
            }

            if (Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
            {
                _anim.SetBool("isMoving", false);
                if (waitTime <= 0)
                {
                    randomSpot = Random.Range(0, moveSpots.Length);
                    waitTime = startWaitTime;
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
        }
    }
}
