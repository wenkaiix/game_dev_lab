using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    private float originalX;
    public float maxOffset = 1.0f;
    private float enemyPatroltime = 2.0f;
    private int moveRight = -1;
    private Vector2 velocity;

    private Rigidbody2D enemyBody;
    public Vector3 startPosition;
    public Animator goombaAnimator;
    public AudioSource goombaAudio;
    private bool isSquashed = false;
    GameManager gameManager;
    PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        gameManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
        goombaAudio = GetComponent<AudioSource>();
        enemyBody = GetComponent<Rigidbody2D>();
        // get the starting position
        Debug.Log(transform.position); 
        startPosition = transform.position;
        originalX = transform.position.x;
        ComputeVelocity();
    }
    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }
    void Movegoomba()
    {
        enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
    }

    // note that this is Update(), which still works but not ideal. See below.
    void Update()
    {
        if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
        {// move goomba
            Movegoomba();
        }
        else
        {
            // change direction
            moveRight *= -1;
            ComputeVelocity();
            Movegoomba();
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player") && !isSquashed && playerMovement.alive)
        {
            foreach (ContactPoint2D contact in col.contacts)
            {
                // Print how many points are colliding with this transform
                Debug.Log("Points colliding: " + col.contacts.Length);

                // Print the normal of the first point in the collision.
                Debug.Log("Normal of the first point: " + col.contacts[0].normal);

                // Draw a different colored ray for every normal in the collision
                foreach (var item in col.contacts)
                {
                    Debug.DrawRay(item.point, item.normal * 100, Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f), 10f);
                }

                // when mario lands on TOP of goomba
                if (contact.normal.y < -0.5f)
                {
                    isSquashed = true;
                    goombaAnimator.SetTrigger("onSquash");
                    goombaAudio.PlayOneShot(goombaAudio.clip);
                    gameManager.IncreaseScore(1);
                    break;
                }
                else
                {
                    // play death animation
                    playerMovement.marioAnimator.Play("Mario-die");
                    // play death animation
                    playerMovement.marioDeathAudio.PlayOneShot(playerMovement.marioDeathAudio.clip);
                    playerMovement.alive = false;
                    // im choosing to put this portion here because i feel like death condition for mario might change
                    // depending on what enemies he encounters?
                }
            }
        }
    }

    void squashToGround()
    {
        // bring the y position lower cuz when it changes sprite it floats in the air
        transform.position = new Vector3(gameObject.transform.position.x, -0.199f, gameObject.transform.position.z);
        // make it stop moveing when dead
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
    }

    public void selfDestruct()
    {
        gameObject.SetActive(false); // 
    }

  
    public void GameRestart()
    {
        gameObject.SetActive(true);
        enemyBody.constraints = RigidbodyConstraints2D.None;
        isSquashed = false;
        transform.localPosition = startPosition;
        originalX = transform.position.x;
        moveRight = -1;
        ComputeVelocity();
    }

    void Awake()
    {
        GameManager.instance.gameRestart.AddListener(GameRestart);
    }

}