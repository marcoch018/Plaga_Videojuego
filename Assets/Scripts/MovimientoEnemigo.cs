using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer))]
public class MovimientoEnemigo : MonoBehaviour
{
    public enum BehaviorMode { Seeking, Wandering, Combined }

    [Header("Configuración")]
    public BehaviorMode mode = BehaviorMode.Combined;
    public Transform goal;
    public float speed = 3f;
    public float maxForce = 12f;
    public float detectionRadius = 6f;
    public float slowRadius = 3f;
    public float stoppingDistance = 0.5f;

    [Header("Wandering")]
    public float wanderDistance = 3f;
    public float wanderRadius = 1.5f;
    public float wanderJitter = 4f;

    [Header("Vida")]
    public float vidaMaxima = 100f;
    public float vida = 100f;

    Vector2 velocity;
    float wanderAngle;
    SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        wanderAngle = Random.Range(0f, 360f);
    }

    void Start()
    {
        if (goal == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) goal = p.transform;
        }
        velocity = Random.insideUnitCircle.normalized * speed;
    }

    void Update()
    {
        if (goal == null) return;

        Vector2 acc = mode switch
        {
            BehaviorMode.Seeking   => SeekArrive(goal.position),
            BehaviorMode.Wandering => Wander(),
            _ => Vector2.Distance(transform.position, goal.position) <= detectionRadius
                 ? SeekArrive(goal.position)
                 : Wander()
        };

        velocity += acc * Time.deltaTime;
        if (velocity.magnitude > speed) velocity = velocity.normalized * speed;
        transform.position += (Vector3)(velocity * Time.deltaTime);

        if (sr != null && Mathf.Abs(velocity.x) > 0.01f)
            sr.flipX = velocity.x < 0f;
    }

    public void RecibirDano(float cantidad)
    {
        if (cantidad <= 0f) return;
        vida = Mathf.Max(0f, vida - cantidad);
        if (vida <= 0f) Destroy(gameObject);
    }

    Vector2 SeekArrive(Vector3 target)
    {
        Vector2 offset = (Vector2)target - (Vector2)transform.position;
        float dist = offset.magnitude;
        if (dist < 0.001f || dist < stoppingDistance) return -velocity;

        float targetSpeed = dist < slowRadius ? speed * (dist / slowRadius) : speed;
        Vector2 desired = offset.normalized * targetSpeed;
        return ClampForce(desired - velocity);
    }

    Vector2 Wander()
    {
        wanderAngle += Random.Range(-wanderJitter, wanderJitter) * Time.deltaTime;

        Vector2 forward = velocity.sqrMagnitude > 0.01f ? velocity.normalized : Vector2.right;
        Vector2 circleCenter = (Vector2)transform.position + forward * wanderDistance;
        Vector2 displacement = new Vector2(Mathf.Cos(wanderAngle * Mathf.Deg2Rad),
                                           Mathf.Sin(wanderAngle * Mathf.Deg2Rad)) * wanderRadius;
        Vector2 target = circleCenter + displacement;

        Vector2 desired = (target - (Vector2)transform.position).normalized * speed;
        return ClampForce(desired - velocity);
    }

    Vector2 ClampForce(Vector2 force)
    {
        return force.magnitude > maxForce ? force.normalized * maxForce : force;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, slowRadius);
    }
}