using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{

    public GameObject playerDeathPrefab;
    public AudioClip deathClip;
    public Sprite hitSprite;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        // 1  
        if (coll.transform.tag == "Player")// check to ensure the colliding GameObject has the "Player" tag.
        {

            // 2   
            var audioSource = GetComponent<AudioSource>();//Next, determine if an AudioClip has been assigned to the script and that a valid Audio Source component exists.
            if (audioSource != null && deathClip != null) 
            {
                audioSource.PlayOneShot(deathClip);//If so, play the deathClip audio effect.
            }

            // 3  
            Instantiate(playerDeathPrefab, coll.contacts[0].point,//Instantiate the playerDeathPrefab at the collision point and swap the sprite of the saw blade with the hitSprite version.
                Quaternion.identity);
            spriteRenderer.sprite = hitSprite;

            // 4  
            Destroy(coll.gameObject);//Lastly, destroy the colliding object (the player).
        }
    }
}
