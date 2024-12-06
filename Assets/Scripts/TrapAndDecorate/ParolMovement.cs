using UnityEngine;

public class ParolMovement : MonoBehaviour
{
    public Transform pointA; 
    public Transform pointB; 
    public float speed = 2f; 
    private bool movingToB = true;

    void Update()
    {
        if (movingToB)
        {
            MoveTo(pointB.position);
            if (Vector2.Distance(transform.position, pointB.position) < 0.1f)
            {
                movingToB = false;
            }
        }
        else
        {
            MoveTo(pointA.position);
            if (Vector2.Distance(transform.position, pointA.position) < 0.1f)
            {
                movingToB = true;
            }
        }
    }
    void MoveTo(Vector3 target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }
}
