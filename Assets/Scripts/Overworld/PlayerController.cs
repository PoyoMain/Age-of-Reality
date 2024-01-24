using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.DefaultInputActions;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Stats")]
    [SerializeField] private float speed = 5;

    [Header("Party")]
    [SerializeField] private ScriptableHero playerClass; // The class of the player
    [SerializeField] private List<ScriptableHero> allies; // The player's allies

    [HideInInspector] public List<ScriptableHero> Party // The entire party, player included
    {  get { return GetParty(); }
        private set { } }

    private PlayerControls playerControls;
    private PlayerControls.OverworldControlsActions overworldActions;
    private Rigidbody2D rigid;
    //private Animator anim;

    void Awake()
    {
        playerControls = new PlayerControls();
        overworldActions = playerControls.OverworldControls;

        rigid = GetComponent<Rigidbody2D>();
        //anim = GetComponentInChildren<Animator>();
    }

    public void HandleUpdate()
    {
        Move(overworldActions.Movement.ReadValue<Vector2>());
    }

    /// <summary>
    /// Moves the player 
    /// </summary>
    /// <param name="input">Keyboard/Controller input vectorized</param>
    public void Move(Vector2 input)
    {
        //anim.SetFloat("XAxis", input.x);
        //anim.SetFloat("YAxis", input.y);

        Vector3 moveDirection = new(input.x, input.y, 0);
        //rigid.MovePosition(speed * Time.deltaTime * transform.TransformDirection(moveDirection)); // Moves the player
        rigid.velocity = speed * moveDirection; // Moves the player

        //Uncomment if this ends up being a sidescroller
        //if (!controller.isGrounded) controller.Move(9.8f * Time.deltaTime * Vector3.down); // If off the ground, go towards the ground
    }

    /// <summary>
    /// Gets the player's party
    /// </summary>
    /// <returns>The players party members</returns>
    public List<ScriptableHero> GetParty()
    {
        List<ScriptableHero> partyMembers = new() { playerClass };
        partyMembers.AddRange(allies);

        return partyMembers;
    }

    public void Freeze()
    {
        rigid.velocity = Vector3.zero;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
        {
            GameManager.Instance.EnemyHit(enemy);
        }
    }

    private void OnEnable()
    {
        overworldActions.Enable(); //Enables the controls
    }

    private void OnDisable()
    {
        overworldActions.Disable(); //Disables the controls
    }
}
