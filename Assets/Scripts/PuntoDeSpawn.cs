using UnityEngine;

public class PuntoDeSpawn : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject prefabEnemigo;
    public float intervaloSpawn = 2f;
    public float tiempoInicial = 1f;

    void Start()
    {
        if (prefabEnemigo) InvokeRepeating(nameof(Spawnear), tiempoInicial, intervaloSpawn);
    }

    void Update()
    {
        if (GameManager.Instancia != null && GameManager.Instancia.juegoTerminado)
            CancelInvoke(nameof(Spawnear));
    }

    void Spawnear()
    {
        if (GameManager.Instancia != null && GameManager.Instancia.juegoTerminado) return;
        Instantiate(prefabEnemigo, transform.position, Quaternion.identity);
    }
}