using UnityEngine;

public class VidaJugador : MonoBehaviour
{
    [Header("Vida")]
    public float vidaMaxima = 100f;
    public float vida = 100f;

    void Start()
    {
        vida = vidaMaxima;
        HUDManager.Instancia?.SetVida(vida, vidaMaxima);
    }

    public void RecibirDano(float cantidad)
    {
        if (GameManager.Instancia != null && GameManager.Instancia.juegoTerminado) return;
        if (cantidad <= 0f) return;

        vida = Mathf.Max(0f, vida - cantidad);
        HUDManager.Instancia?.SetVida(vida, vidaMaxima);
        HUDManager.Instancia?.MostrarDano();

        if (vida <= 0f)
        {
            HUDManager.Instancia?.MostrarDanoFatal();
            GameManager.Instancia?.GameOver();
        }
    }
}