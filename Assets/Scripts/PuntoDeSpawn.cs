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

    void Spawnear() => Instantiate(prefabEnemigo, transform.position, Quaternion.identity);
}