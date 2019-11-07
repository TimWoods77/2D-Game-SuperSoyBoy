using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), // Validates that any GameObject attached to this script has the minimum necessary components required to work
    typeof(Animator))]

public class SoyBoyController : MonoBehaviour
{
    public float jump = 14f;
    public float airAccel = 3f;//air control
    public float jumpDurationThreshold = 0.25f;
    private float jumpDuration;
    public float speed = 14f;//Holds pre-deﬁned values to use when calculating how much force to apply to Super Soy Boy’s Rigidbody.
    public float accel = 6f; //Holds pre-deﬁned values to use when calculating how much force to apply to Super Soy Boy’s Rigidbody.
    private Vector2 input;//Stores the controller’s current input values for x and y at any point in time. Negatives mean the controls are going left (-x) or down (-y), and positives mean right (x) or up (y). 
    private SpriteRenderer sr;// references SpriteRenderer
    private Rigidbody2D rb;// References Rigidbody 2D
    private Animator animator;// References Animator

    public bool isJumping;
    public float jumpSpeed = 8f;
    private float rayCastLengthCheck = 0.005f;
    private float width;
    private float height;

    void Awake()// This ensures component references are cached when the game starts.
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        width = GetComponent<Collider2D>().bounds.extents.x + 0.1f;// grabs width of SoyBoy's sprites and adds 0.1f to them
        height = GetComponent<Collider2D>().bounds.extents.y + 0.2f;// grabs height of SoyBoy's sprites and adds 0.2f to them
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    public bool PlayerIsOnGround()
    {
        // 1  
        bool groundCheck1 = Physics2D.Raycast(new Vector2( //The ﬁrst ground check performs a Raycast directly below the center of the the character (Transform.position.x), using a length equal to the value of rayCastLengthCheck which is defaulted to 0.005f — a very short raycast is therefore sent down from the bottom of the SoyBoy sprite.
            transform.position.x, transform.position.y - height),
            -Vector2.up, rayCastLengthCheck);
        bool groundCheck2 = Physics2D.Raycast(new Vector2(   // checks for accurate ground detection from left to center
            transform.position.x + (width - 0.2f),
            transform.position.y - height), -Vector2.up,
            rayCastLengthCheck);
        bool groundCheck3 = Physics2D.Raycast(new Vector2( // checks for accurate ground detection from right to center
            transform.position.x - (width - 0.2f),
            transform.position.y - height), -Vector2.up,
            rayCastLengthCheck);


        // 2   
        if (groundCheck1 || groundCheck2 || groundCheck3)// If any of the ground checks come back as true, then this method returns true to the caller. Otherwise, it will return false.
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsWallToLeftOrRight()
    {
        // 1
        bool wallOnleft = Physics2D.Raycast(new Vector2(     
            transform.position.x - width, transform.position.y),  
            -Vector2.right, rayCastLengthCheck);
        bool wallOnRight = Physics2D.Raycast(new Vector2( 
            transform.position.x + width, transform.position.y),
              Vector2.right, rayCastLengthCheck);
        
        // 2  
        if (wallOnleft || wallOnRight)// if any of the raycast hits an object it will return true otherwise it'll return false
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool PlayerIsTouchingGroundOrWall()// returns true if player is touching the ground or has a wall to the left or right of the player
    {
        if (PlayerIsOnGround() || IsWallToLeftOrRight())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetWallDirection()
    {
        bool isWallLeft = Physics2D.Raycast(new Vector2//This returns an integer based on whether the wall is left (-1), right (1), or neither (0).
            (transform.position.x - width, transform.position.y),
            -Vector2.right, rayCastLengthCheck);
        bool isWallRight = Physics2D.Raycast(new Vector2(transform.position.x + width, transform.position.y),
            Vector2.right, rayCastLengthCheck);
        if (isWallLeft)
        {
            return -1;
        }
        else if (isWallRight)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }


    // Update is called once per frame
    void Update()
    {
        // 1  
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Jump");

        // 2  
        if (input.x > 0f)
        {
            sr.flipX = false;
        }
        else if (input.x < 0f)
        {
            sr.flipX = true;
        }

        if (input.y >= 1f)
        {
            jumpDuration += Time.deltaTime;//As long as the jump button is held down, jumpDuration is counted up using the time the previous frame took to complete (Time.deltaTime).
        }

        else
        {
            isJumping = false;//If jump is released, jumpDuration is set back to 0 for the next time it needs to be timed
            jumpDuration = 0f;
        }

        if (PlayerIsOnGround() && isJumping == false)//  calls the PlayerIsOnGround() method to determine if the player is touching the ground or not (true or false). It also checks if the player is not already jumping. 
        {
            if (input.y > 0f)// performs an inner check to see if there is any input for jumping from the controls (input.y > 0f). 
            {
                isJumping = true;//If so, the isJumping boolean variable is set to true.
            }
        }
        if (jumpDuration > jumpDurationThreshold) input.y = 0f;//This checks for jumpDuration being longer than the jumpDurationThreshold (0.25 seconds)
    }

    void FixedUpdate()
    {
        // 1 
        var acceleration = 0f;//This code sets acceleration to 0 to start with (a default, initial value). 
        if (PlayerIsOnGround())// looks to see if the player is on the ground or not
        {
            acceleration = accel;
        }
        else
        {
            acceleration = airAccel;
        }

        var xVelocity = 0f;//This is an extra check to ensure that xVelocity is only set to 0 if the player is on the ground and not using left or right controls.

        // 2 
        if (PlayerIsOnGround() && input.x == 0)// If horizontal axis controls are neutral
        {
            xVelocity = 0f;//then xVelocity is set to 0
        }
        else
        {
            xVelocity = rb.velocity.x;//otherwise xVelocity is set to the current x velocity of the rb component
        }

        var yVelocity = 0f;//This ensures that the yVelocity value is set to the jump value of 14 when the character is jumping from the ground, or from a wall. 
        if (PlayerIsTouchingGroundOrWall() && input.y == 1)//Otherwise, it’s set to the current velocity of the rigidbody.
        {
            yVelocity = jump;
        }
        else
        {
            yVelocity = rb.velocity.y;
        }

        // 3 
        rb.AddForce(new Vector2(((input.x * speed) - rb.velocity.x)// Force is added to rb by calculating the current value of the horizontal axis controls multiplied by speed, which is in turn multiplied by acceleration.     
            * acceleration, 0));

        // 4  
        rb.velocity = new Vector2(xVelocity, yVelocity); //Velocity is reset on rb so it can stop Super Soy Boy from moving left or right when controls are in a neutral state. Otherwise, velocity is set to exactly what it’s currently at.

        if (IsWallToLeftOrRight() && !PlayerIsOnGround() && input.y == 1)//This checks to see if there is a wall to the left or right of the player, that they are not on the ground, and that they are currently jumping.  
        {
            rb.velocity = new Vector2(-GetWallDirection() * speed * 0.75f, rb.velocity.y);//If this is the case, the character’s Rigidbody velocity is set to a new velocity, using the current Y velocity, but with a change to the X (horizontal) velocity.
        }

        if (isJumping && jumpDuration < jumpDurationThreshold)//This gives Super Soy Boy’s Rigidbody a new velocity if the user has pressed the jump button for less than the jumpDurationThreshold.
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);// This equates to upward movement, forming a satisfying input-duration controlled jump! 
        }
    }
}   