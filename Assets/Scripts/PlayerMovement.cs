using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // for textmeshpro


public class PlayerMovement : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject enemies;
    public float speed = 10;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    public Animator marioAnimator;
    public AudioSource marioAudio;
    public AudioClip marioDeath;
    public float deathImpulse = 15;
    public Transform gameCamera;

    // state
    [System.NonSerialized]
    public bool alive = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created. test
    void Start()
    {
        marioSprite = GetComponent<SpriteRenderer>();

        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();
        // update animator state
        marioAnimator.SetBool("onGround", onGroundState);
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


    public float maxSpeed = 20;
    public float upSpeed = 2;
    private bool onGroundState = true;



    int collisionLayerMask = (1 << 3) | (1 << 6) | (1 << 7);
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

      void OnTriggerEnter2D(Collider2D other)
  {
      if (other.gameObject.CompareTag("Enemy") && alive)
      {
          Debug.Log("Collided with goomba!");
          //Time.timeScale = 0.0f;
          // play death animation
          marioAnimator.Play("Mario-die");
          marioAudio.PlayOneShot(marioDeath);
          alive = false;
          //GameOver();
          
      }
  }


    private bool moving = false;
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

    public void RestartButtonCallback(int input)
    {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
    }

    private bool jumpedState = false;
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


    public JumpOverGoomba jumpOverGoomba;

    private void ResetGame()
    {
        // reset position
        marioBody.transform.position = new Vector3(-0.273f, 0.104f, 0.0f);
        // reset camera position
        gameCamera.position = new Vector3(0, 0.47f, -1.09f);
        // reset sprite direction
        faceRightState = true;
        marioSprite.flipX = false;
        // reset score
        scoreText.text = "Score: 0";
        // reset Goomba
        foreach (Transform eachChild in enemies.transform)
        {
            eachChild.transform.position = eachChild.GetComponent<EnemyMovement>().startPosition;

        }
        jumpOverGoomba.score = 0;
        // reset animation
        marioAnimator.SetTrigger("gameRestart");
        alive = true;
    }

    public GameOverScreen GameOverScreen;
    public void GameOver(){ 
        Time.timeScale = 0.0f;
        GameOverScreen.Setup(jumpOverGoomba.score);

    }

    
}
