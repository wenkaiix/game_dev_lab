using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // for textmeshpro
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
    // public TextMeshProUGUI scoreText;
    // public GameObject enemies;
    public GameConstants gameConstants;
    float speed;
    
    float maxSpeed;
    float upSpeed;
    float deathImpulse;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    public Animator marioAnimator;
    public AudioSource marioAudio;
    public AudioSource marioDeathAudio;
    public Transform gameCamera;
    private Vector3 startPosition;
    // public float offset; //for the camera to be set correctly

    // state
    [System.NonSerialized]
    public bool alive = true;
    private bool onGroundState = true;

    private bool jumpedState = false;

    private bool moving = false;
    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7); // for collision 
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created. test
    void Start()
    {
        Debug.Log(gameConstants.speed);
        // Set constants
        speed = gameConstants.speed;
        maxSpeed = gameConstants.maxSpeed;
        deathImpulse = gameConstants.deathImpulse;
        upSpeed = gameConstants.upSpeed;
        //get starting position
        startPosition = transform.position;
        marioSprite = GetComponent<SpriteRenderer>();

        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();
        // update animator state
        marioAnimator.SetBool("onGround", onGroundState);
        // subscribe to scene manager scene change
        // SceneManager.activeSceneChanged += SetStartingPosition;
    }

    // Update is called once per frame
    void Update()
    {
      marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.linearVelocity.x));   
    }

    void FlipMarioSprite(int value)
    {
        if (value == -1 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
            if (marioBody.linearVelocity.x > 0.05f)
                marioAnimator.SetTrigger("onSkid");

        }

        else if (value == 1 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
            if (marioBody.linearVelocity.x < -0.05f)
                marioAnimator.SetTrigger("onSkid");
        }
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        // if (col.gameObject.CompareTag("Ground")) onGroundState = true;
        if (((collisionLayerMask & (1 << col.transform.gameObject.layer)) > 0) & !onGroundState)
        {
            onGroundState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);
        }
    }


    public void GameOver() // neeed to be called here so that we can put it at the end of our animation.so animation calls it 
    {
        GameManager.instance.GameOver(); // then calls game manager to signal to everyone else that gameover alr
    }

    

    // FixedUpdate is called 50 times a second
    void  FixedUpdate()
    {
        if (alive && moving)
        {
            Move(faceRightState == true ? 1 : -1);
        }
    }

    void Move(int value)
    {

        Vector2 movement = new Vector2(value, 0);
        // check if it doesn't go beyond maxSpeed
        if (marioBody.linearVelocity.magnitude < maxSpeed)
            marioBody.AddForce(movement * speed);
    }

    public void MoveCheck(int value)
    {
        if (value == 0)
        {
            moving = false;
        }
        else
        {
            FlipMarioSprite(value);
            moving = true;
            Move(value);
        }
    }

    public void Jump()
    {
        if (alive && onGroundState)
        {
            // jump
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            jumpedState = true;
            // update animator state
            marioAnimator.SetBool("onGround", onGroundState);

        }
    }

    public void JumpHold()
    {
        if (alive && jumpedState)
        {
            // jump higher
            marioBody.AddForce(Vector2.up * upSpeed * 30, ForceMode2D.Force);
            jumpedState = false;
        }
    }


    void PlayJumpSound()
    {
        // play jump sound
        marioAudio.PlayOneShot(marioAudio.clip);
    }


    void PlayDeathImpulse()
    {
        marioBody.AddForce(Vector2.up * deathImpulse, ForceMode2D.Impulse);
    }


        // public GameManager gameManager; 
        public void GameRestart()
    {
        // reset position
        marioBody.transform.position = startPosition;
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;

        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;

        // reset camera position
        // gameCamera.position = new Vector3(0, 0.47f, -1.09f);
        // gameCamera.position = new Vector3(marioBody.transform.position.x,marioBody.transform.position.y + offset, -1.09f);

    }    


    void Awake(){
        // other instructions
        // subscribe to Game Restart event
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }
}
