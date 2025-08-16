using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum ObstacleType
    {
        Rotate,
        Trap,
        Moving,
        Pendulum
    }

    public ObstacleType obstacleType;

    public GameObject[] TrapPos;

    public float rotationSpeed;
    public float moveDistance = 2f;
    public float speed = 2f;
    public float timer;

    private Quaternion initialRotation;

    private Vector3 startPos;

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
                break;
            case ObstacleType.Moving:
                MovingObstacle();
                break;
            case ObstacleType.Pendulum:
                PendulumObstacle();
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
}
