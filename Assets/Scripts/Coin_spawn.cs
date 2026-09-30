using UnityEngine;
using System.Collections;

public class Coin_spawn : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 startPosition;
    public AudioSource coinAudio;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
        StartCoroutine(Bounce());
    }

    void PlayCoinSound()
    {
        // play jump sound, ??coinAudio.PlayOneShot(coinSpawn);
        coinAudio.PlayOneShot(coinAudio.clip);
    }

    IEnumerator Bounce()
    {
        Vector2 topPosition = startPosition + Vector2.up * 0.5f;

        // Move up
        while (Vector2.Distance(rb.position, topPosition) > 0.01f)
        {
            rb.MovePosition(
                Vector2.MoveTowards(rb.position, topPosition, 1.5f * Time.fixedDeltaTime)
            );

            yield return new WaitForFixedUpdate();
        }

        // Move back down
        while (Vector2.Distance(rb.position, startPosition) > 0.01f)
        {
            rb.MovePosition(
                Vector2.MoveTowards(rb.position, startPosition, 1.5f * Time.fixedDeltaTime)
            );

            yield return new WaitForFixedUpdate();
        }

        rb.position = startPosition;

        Destroy(gameObject);
    }
}
