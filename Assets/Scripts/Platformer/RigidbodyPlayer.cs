using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;



public enum PlayerState
{
    Walk,
    Jump,
    MidAir,
    Land,
}

[RequireComponent(typeof(Rigidbody2D))]
public class RigidbodyPlayer : MonoBehaviour
{

    public PlayerState playerState;
    public ParticleManager particleManager;

    [Header("Walk")]
    [SerializeField] private float speed = 6f;
    [Header("Jump")]
    [HorizontalLine(color: EColor.Orange)]
    [SerializeField] private float jumpVelocity = 6f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;
    [Range(0f, 1f)]
    [SerializeField] private float inAirMultiplier = 0.5f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float raycastDistace = 2.5f;
    [InfoBox("Depends on size.x of player")]
    [SerializeField] private float[] raycastOrigins = { -0.58f, 0f, 0.58f };

    [ShowNonSerializedField] bool isGrounded;
    [ShowNonSerializedField]float inAirControl;


    protected float inputX;
    protected float inputY;
    protected float gravity;
    protected bool jump;
   


    Rigidbody2D rbPlayer;

    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody2D>();
        gravity = Physics2D.gravity.y;
    }

    // Update is called once per frame
    void Update()
    {      
        GetInput();
        FlipPlayer();
        IsGrounded();
        if (isGrounded)
        {
            Jump();
        }
       
        GravityChange();
    }

    private void FixedUpdate()
    {
        MoveRb();
    }

    void GetInput() {
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
        jump = Input.GetButtonDown("Jump");
    }

    void MoveRb() {
        rbPlayer.velocity = new Vector2(
            inputX * speed * inAirControl * Time.deltaTime
            ,rbPlayer.velocity.y);
    }

    void FlipPlayer() {
        if (inputX > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else if (inputX < 0) {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }

        return;
    }

    void IsGrounded() {
        isGrounded = RaycastCheck();
        if (isGrounded)
        {
            inAirControl = 1;
            playerState = PlayerState.Walk;
        }
        else {
            inAirControl = inAirMultiplier;
            playerState = PlayerState.MidAir;
        }
       
    }

    bool RaycastCheck() {
        for (int i = 0; i < raycastOrigins.Length; i++)
        {
            Vector2 origin = new Vector2(transform.localPosition.x - raycastOrigins[i], transform.position.y);
            bool isGrounded = Physics2D.Raycast(origin, -transform.up, raycastDistace, groundLayer);
            Debug.DrawRay(origin, -transform.up * raycastDistace, Color.red);
            if (isGrounded)
            {
                return true;
            }
            else {
                continue;
            }
        }

        return false;
    }

    void Jump() {
        if (jump) {
            inAirControl = inAirMultiplier;
            rbPlayer.velocity = Vector2.up * jumpVelocity;
            playerState = PlayerState.Jump;
            particleManager.InstatiateEffect(playerState);
            Debug.Log("Jump Yeahh!");
        }
    }

    void GravityChange() {
        if (rbPlayer.velocity.y < 0) {
            rbPlayer.velocity += Vector2.up * gravity * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rbPlayer.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rbPlayer.velocity += Vector2.up * gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
