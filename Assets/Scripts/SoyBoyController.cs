using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), // Validates that any GameObject attached to this script has the minimum necessary components required to work
    typeof(Animator))]

public class SoyBoyController : MonoBehaviour
{
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
        var acceleration = accel;//  Assign the value of accel — the public ﬂoat ﬁeld — to a private variable named acceleration.
        var xVelocity = 0f;

        // 2 
        if (input.x == 0)// If horizontal axis controls are neutral
        {
            xVelocity = 0f;//then xVelocity is set to 0
        }
        else
        {
            xVelocity = rb.velocity.x;//otherwise xVelocity is set to the current x velocity of the rb component
        }

        // 3 
        rb.AddForce(new Vector2(((input.x * speed) - rb.velocity.x)// Force is added to rb by calculating the current value of the horizontal axis controls multiplied by speed, which is in turn multiplied by acceleration.     
            * acceleration, 0));

        // 4  
        rb.velocity = new Vector2(xVelocity, rb.velocity.y);//Velocity is reset on rb so it can stop Super Soy Boy from moving left or right when controls are in a neutral state. Otherwise, velocity is set to exactly what it’s currently at.

        if (isJumping && jumpDuration < jumpDurationThreshold)//This gives Super Soy Boy’s Rigidbody a new velocity if the user has pressed the jump button for less than the jumpDurationThreshold.
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);// This equates to upward movement, forming a satisfying input-duration controlled jump! 
        }
    }
}   