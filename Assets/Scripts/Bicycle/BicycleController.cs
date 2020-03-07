using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BicycleController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D frontTire;
    [SerializeField]
    private Rigidbody2D backTire;
    [SerializeField]
    private float speed = 10f;
    [SerializeField]
    private float JumpForce = 50f;
    [SerializeField]
    private float tireForce = 50f;
    [SerializeField]
    private float groundDetectDistance = 3f;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private bool isGrounded;

    float movement;

    [SerializeField]
    private Transform bikeBody;

    bool Jump;
    bool fw_jump;
    bool bw_jump;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(bikeBody.position, -bikeBody.up * groundDetectDistance, Color.red);

        isGrounded = Physics2D.Raycast(bikeBody.position, -bikeBody.up, groundDetectDistance, groundLayer);
        movement = Input.GetAxis("Horizontal");

        if (isGrounded) {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                backTire.AddForce(Vector3.up * JumpForce);
                frontTire.AddForce(Vector3.up * JumpForce);
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                backTire.AddForce(Vector3.up * tireForce);
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                frontTire.AddForce(Vector3.up * tireForce);
            }
        }

    }

    private void FixedUpdate(){
        if (isGrounded)
        {
            float torque = -movement * speed * Time.deltaTime;
            backTire.AddTorque(torque);
            frontTire.AddTorque(torque);
            //Debug.Log("applied torque is : " + torque);
        }
    }


}
