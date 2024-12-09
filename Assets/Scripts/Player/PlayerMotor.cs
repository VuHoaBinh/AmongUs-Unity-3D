using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool lerpCrouch;
    private bool crouching;
    private bool sprinting;
    public float crouchTimer;
    public float speed =5f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;
    // Start is called before the first frame update
    void Start()
    {
        controller =GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        if(lerpCrouch){
            crouchTimer += Time.deltaTime;
            float p= crouchTimer / 1;
            p *=p;
            if(crouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
            {
                controller.height = Mathf.Lerp(controller.height, 2, p);
            }

            if(p>1){
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }
    public void ProcessMove(Vector2 input){
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;
        controller.Move(transform.TransformDirection(moveDirection)*speed*Time.deltaTime);
        playerVelocity.y += gravity*Time.deltaTime;
        if(isGrounded && playerVelocity.y < 0){
            playerVelocity.y = -2f;
        }
        controller.Move(playerVelocity*Time.deltaTime);
    }
    public void Jump(){
        if(isGrounded)
            playerVelocity.y = Mathf.Sqrt(jumpHeight*-3.0f*gravity);
    }
    public void Crouch(){
        crouching =!crouching;
        crouchTimer =0;
        lerpCrouch = true;
    }
    public void Sprint(){
        sprinting =! sprinting;
        if(sprinting)
            speed *= 2;
        else
            speed = 5;
    }
    
}
