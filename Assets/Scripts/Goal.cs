using System.Collections;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public AudioClip goalClip;

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Player")//Here, you check for player collisions with the Goal GameObject. 
        {
            var audioSource = GetComponent<AudioSource>();
            if (audioSource != null && goalClip != null)
            {
                audioSource.PlayOneShot(goalClip);//If this happens, the player has reached the goal so you play the goalClip audio clip and restart the level after a 0.5 second delay.
            }
            GameManager.instance.RestartLevel(0.5f);

            // 1
            var timer = FindObjectOfType<Timer>();

            // 2 
            GameManager.instance.SaveTime(timer.time);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
