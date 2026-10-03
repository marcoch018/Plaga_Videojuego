using UnityEngine;

public class DisparoJugador : MonoBehaviour
{
    [Header("Configuración de Disparo")]
    public GameObject prefabProyectil;
    public Transform puntoDisparo; // Opcional: El punto exacto de donde sale la bala
    public float cadencia = 0.3f;
    
    private float tiempoProximoDisparo = 0f;

    void Update()
    {
        // Click izquierdo del ratón
        if (Input.GetButtonDown("Fire1") && Time.time >= tiempoProximoDisparo)
        {
            Disparar();
            tiempoProximoDisparo = Time.time + cadencia;
        }
    }

    void Disparar()
    {
        if (prefabProyectil == null) return;

        // 1. Obtener la posición del ratón en el mundo real 2D
        Vector3 posicionRaton = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        posicionRaton.z = 0f;

        // 2. Definir desde dónde sale la bala
        Vector3 origen = puntoDisparo != null ? puntoDisparo.position : transform.position;

        // 3. Calcular la dirección y el ángulo de rotación
        Vector2 direccion = (posicionRaton - origen).normalized;
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;

        // 4. Instanciar el proyectil con la rotación correcta
        Instantiate(prefabProyectil, origen, Quaternion.Euler(0f, 0f, angulo));
    }
}