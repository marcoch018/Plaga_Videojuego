using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D), typeof(Collider2D))]
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
    public float danoAlJugador = 10f;

    [Header("Feedback")]
    public Color colorDano = Color.red;
    public float tiempoFlash = 0.1f;

    Rigidbody2D rb;
    SpriteRenderer sr;
    Vector2 velocity;
    float wanderAngle;
    float flashTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0f;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.freezeRotation = true;

        var col = GetComponent<Collider2D>();
        col.isTrigger = false;

        wanderAngle = Random.Range(0f, 360f);
    }

    void Start()
    {
        if (goal == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) goal = p.transform;
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

        if (sr && Mathf.Abs(velocity.x) > 0.01f) sr.flipX = velocity.x < 0f;

        if (flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;
            sr.color = Color.Lerp(Color.white, colorDano, flashTimer / tiempoFlash);
        }
    }

    void FixedUpdate()
    {
        rb.velocity = velocity;
    }

    public void RecibirDano(float cantidad)
    {
        if (cantidad <= 0f) return;
        vida = Mathf.Max(0f, vida - cantidad);
        sr.color = colorDano;
        flashTimer = tiempoFlash;

        if (vida <= 0f)
        {
            GameManager.Instancia?.RegistrarBaja();
            Destroy(gameObject);
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
            col.gameObject.SendMessage("RecibirDano", danoAlJugador, SendMessageOptions.DontRequireReceiver);
    }

    Vector2 SeekArrive(Vector3 target)
    {
        Vector2 offset = (Vector2)target - (Vector2)transform.position;
        float dist = offset.magnitude;
        if (dist < 0.001f || dist < stoppingDistance) return -velocity;
        float targetSpeed = dist < slowRadius ? speed * (dist / slowRadius) : speed;
        return ClampForce(offset.normalized * targetSpeed - velocity);
    }

    Vector2 Wander()
    {
        wanderAngle += Random.Range(-wanderJitter, wanderJitter) * Time.deltaTime;
        Vector2 forward = velocity.sqrMagnitude > 0.01f ? velocity.normalized : Vector2.right;
        Vector2 circleCenter = (Vector2)transform.position + forward * wanderDistance;
        Vector2 displacement = new Vector2(Mathf.Cos(wanderAngle * Mathf.Deg2Rad),
                                           Mathf.Sin(wanderAngle * Mathf.Deg2Rad)) * wanderRadius;
        Vector2 target = circleCenter + displacement;
        return ClampForce((target - (Vector2)transform.position).normalized * speed - velocity);
    }

    Vector2 ClampForce(Vector2 f) => f.magnitude > maxForce ? f.normalized * maxForce : f;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;    Gizmos.DrawWireSphere(transform.position, slowRadius);
    }
}