using UnityEngine;

public class DisparoJugador : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject prefabProyectil;
    public Transform puntoDisparo;
    public float cadencia = 0.3f;
    public string nombreArma = "PISTOLA";
    public int municionMaxima = 30;

    int municionActual;
    float tiempoProximoDisparo;

    void Start()
    {
        municionActual = municionMaxima;
        HUDManager.Instancia?.SetArma(nombreArma);
        HUDManager.Instancia?.SetMunicion(municionActual, municionMaxima);
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= tiempoProximoDisparo && municionActual > 0)
        {
            Disparar();
            municionActual--;
            HUDManager.Instancia?.SetMunicion(municionActual, municionMaxima);
            tiempoProximoDisparo = Time.time + cadencia;
        }

        // Recarga automática al soltar el botón si está vacío
        if (Input.GetButtonUp("Fire1") && municionActual <= 0)
            Recargar();
    }

    void Recargar()
    {
        municionActual = municionMaxima;
        HUDManager.Instancia?.SetMunicion(municionActual, municionMaxima);
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