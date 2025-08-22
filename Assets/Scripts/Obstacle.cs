using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum ObstacleType
    {
        Rotate,
        Trap,
        MovingH,
        MovingV,
        Pendulum,
        Piston
    }

    public ObstacleType obstacleType;

    public GameObject[] TrapPos;

    public float rotationSpeed;
    public float moveDistance = 2f;
    public float speed = 2f;
    public float timer;

    private Quaternion initialRotation;

    private Vector3 startPos;

    public float pistonForce = 10f;       // force appliquée aux billes
    public float pistonAmplitude = 1f;    // amplitude visuelle
    public Vector2 pistonDirection = Vector2.right; // direction de poussée



    private void Start()
    {
        startPos = transform.position;
        initialRotation = transform.rotation;    
    }

    private void Update()
    {
        ExecuteObstacle();
    }

    public void ExecuteObstacle() 
    {
        timer += Time.deltaTime;

        switch (obstacleType)
        {
            case ObstacleType.Rotate:
                RotateObstacle();
                break;
            case ObstacleType.Trap:
                OnTriggerEnter2D(GetComponent<Collider2D>());
                this.GetComponent<SpriteRenderer>().color = Color.red; // Change color to indicate trap activation
                break;
            case ObstacleType.MovingH:
                MovingObstacle();
                break;
            case ObstacleType.MovingV:
                MovingV();
                break;
            case ObstacleType.Pendulum:
                PendulumObstacle();
                break;
            case ObstacleType.Piston:
                PistonObstacle();
                this.GetComponent<SpriteRenderer>().color = Color.black; // Change color to indicate piston activation
                break;
        }
    }

    public void RotateObstacle()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
   
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Marble"))
        {
                int randomIndex = Random.Range(0, TrapPos.Length);
                Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
                other.transform.position = TrapPos[randomIndex].transform.position;

                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0f;

        }   
    }
   

    public void MovingObstacle()
    {
        transform.position = startPos + Vector3.right * Mathf.Sin(Time.time * speed) * moveDistance;
    }
    void PendulumObstacle()
    {
        float angle = Mathf.Sin(timer * speed) * 60f;
        transform.rotation = initialRotation * Quaternion.Euler(0, 0, angle);
    }
    public void MovingV()
    {
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * speed) * moveDistance;
    }

    void PistonObstacle()
    {
        // Mouvement visuel du piston
        float offset = Mathf.Sin(timer * speed) * pistonAmplitude;
        transform.position = startPos + (Vector3)pistonDirection.normalized * offset;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (obstacleType == ObstacleType.Piston && other.CompareTag("Marble"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float forceValue = Mathf.Sin(timer * speed) * pistonForce;
                rb.AddForce(pistonDirection.normalized * forceValue, ForceMode2D.Force);
            }
        }
    }
}
