using UnityEngine;

public class PatrolPlatform : MonoBehaviour
{
    private Transform player;
    private bool playerOnPlatform = false;
    private Vector3 previousPosition;

    void Start()
    {
        previousPosition = transform.position;
    }

    void Update()
    {
        if (playerOnPlatform && player != null)
        {
            Vector3 movement = transform.position - previousPosition;
            player.position += movement;
        }
        previousPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.transform;
            playerOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = null;
            playerOnPlatform = false;
        }
    }
}
