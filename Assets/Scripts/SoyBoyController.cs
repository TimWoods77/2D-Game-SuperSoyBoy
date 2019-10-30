using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), // Validates that any GameObject attached to this script has the minimum necessary components required to work
    typeof(Animator))]

public class SoyBoyController : MonoBehaviour
{
    public float speed = 14f;//Holds pre-deﬁned values to use when calculating how much force to apply to Super Soy Boy’s Rigidbody.
    public float accel = 6f; //Holds pre-deﬁned values to use when calculating how much force to apply to Super Soy Boy’s Rigidbody.
    private Vector2 input;//Stores the controller’s current input values for x and y at any point in time. Negatives mean the controls are going left (-x) or down (-y), and positives mean right (x) or up (y). 
    private SpriteRenderer sr;// references SpriteRenderer
    private Rigidbody2D rb;// References Rigidbody 2D
    private Animator animator;// References Animator

    void Awake()// This ensures component references are cached when the game starts.
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
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
    } 

}
