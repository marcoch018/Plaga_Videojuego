using UnityEngine;

public class Proyectil : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad = 15f;
    public float dano = 25f;
    public float tiempoVida = 3f;

    void Start()
    {
        // Destruye el proyectil después de un tiempo para liberar memoria
        Destroy(gameObject, tiempoVida);
    }

    void Update()
    {
        // Mueve el proyectil hacia la derecha local (la dirección a la que apunte)
        transform.Translate(Vector2.right * velocidad * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D otro)
    {
        // Verifica si el objeto impactado tiene el script MovimientoEnemigo
        MovimientoEnemigo enemigo = otro.GetComponent<MovimientoEnemigo>();
        if (enemigo != null)
        {
            enemigo.RecibirDano(dano); // Llama al método de daño del enemigo
            Destroy(gameObject); // Destruye el proyectil tras el impacto
        }
    }
}