using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad = 5f;
    public float suavizado = 0.1f;

    private Rigidbody2D rb;
    private Vector2 input;
    private Vector2 velocidadActual;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;
    }

    void FixedUpdate()
    {
        velocidadActual = Vector2.Lerp(velocidadActual, input * velocidad, suavizado);
        rb.velocity = velocidadActual;
    }
}