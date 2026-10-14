using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovimientoJugador : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad = 5f;
    public float suavizado = 0.1f;

    Rigidbody2D rb;
    Vector2 input, velocidadActual;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.freezeRotation = true;
    }

    void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
    }

    void FixedUpdate()
    {
        velocidadActual = Vector2.Lerp(velocidadActual, input * velocidad, suavizado);
        rb.velocity = velocidadActual;
    }
}