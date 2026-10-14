using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Proyectil : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad = 15f;
    public float dano = 25f;
    public float tiempoVida = 3f;

    void Awake()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.freezeRotation = true;

        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = transform.right * velocidad;
        Destroy(gameObject, tiempoVida);
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        if (otro.TryGetComponent(out MovimientoEnemigo enemigo))
        {
            enemigo.RecibirDano(dano);
            Destroy(gameObject);
        }
    }
}