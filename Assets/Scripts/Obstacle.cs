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
        Piston,
        Bumper
    }

    public ObstacleType obstacleType;

    public GameObject[] TrapPos;

    public float timer;

    [Header("Réglages Rotate")]
    public float rotationSpeed;

    [Header("Réglages Moving")]
    public float moveDistance = 2f;
    public float speed = 2f;

    

    private Quaternion initialRotation;

    private Vector3 startPos;

    [Header("Réglages Piston")]
    public float pistonForce = 10f;       // force appliquée aux billes
    public float pistonAmplitude = 1f;    // amplitude visuelle
    public Vector2 pistonDirection = Vector2.right; // direction de poussée

    [Header("Réglages bumper")]
    public float bumperForce = 15f;



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
                this.GetComponent<Collider2D>().isTrigger = true; // Ensure collider is set to trigger
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
            case ObstacleType.Bumper:
                this.GetComponent<SpriteRenderer>().color = Color.yellow; // Change color to indicate bumper activation
                break;

        }
    }

    public void RotateObstacle()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
   
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Marble") && obstacleType == ObstacleType.Trap)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (obstacleType == ObstacleType.Bumper && collision.collider.CompareTag("Marble"))
        {
            Rigidbody2D rb = collision.collider.GetComponent<Rigidbody2D>();
            if (rb != null && collision.contacts.Length > 0)
            {
                // direction du rebond = normale du contact
                Vector2 bounceDir = collision.contacts[0].normal;
                rb.AddForce(bounceDir * bumperForce, ForceMode2D.Impulse);
            }
        }
    }
}
