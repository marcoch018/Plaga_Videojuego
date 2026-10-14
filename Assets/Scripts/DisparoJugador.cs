using UnityEngine;

public class DisparoJugador : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject prefabProyectil;
    public Transform puntoDisparo;
    public float cadencia = 0.3f;

    float tiempoProximoDisparo;

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= tiempoProximoDisparo)
        {
            Disparar();
            tiempoProximoDisparo = Time.time + cadencia;
        }
    }

    void Disparar()
    {
        if (prefabProyectil == null) return;

        Vector3 raton = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        raton.z = 0f;
        Vector3 origen = puntoDisparo ? puntoDisparo.position : transform.position;
        Vector2 dir = (raton - origen).normalized;
        float angulo = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Instantiate(prefabProyectil, origen, Quaternion.Euler(0f, 0f, angulo));
    }
}