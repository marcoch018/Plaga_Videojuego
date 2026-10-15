using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia;

    public bool juegoTerminado;

    void Awake()
    {
        if (Instancia == null) Instancia = this;
        else { Destroy(gameObject); return; }
    }

    public void GameOver()
    {
        if (juegoTerminado) return;
        juegoTerminado = true;
        GameOverUI.Instancia?.Mostrar();
    }
}