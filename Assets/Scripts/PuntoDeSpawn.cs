using UnityEngine;

public class PuntoDeSpawn : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject prefabEnemigo;
    public float intervaloSpawn = 2f;
    public float tiempoInicial = 1f;

    void Start()
    {
        InvokeRepeating(nameof(Spawnear), tiempoInicial, intervaloSpawn);
    }

    void Spawnear()
    {
        if (prefabEnemigo == null) return;
        Instantiate(prefabEnemigo, transform.position, Quaternion.identity);
    }
}