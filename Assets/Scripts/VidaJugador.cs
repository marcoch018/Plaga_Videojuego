using UnityEngine;

public class VidaJugador : MonoBehaviour
{
    [Header("Vida")]
    public float vidaMaxima = 100f;
    public float vida = 100f;

    public void RecibirDano(float cantidad)
    {
        vida = Mathf.Max(0f, vida - cantidad);
        if (vida <= 0f) GameManager.Instancia?.GameOver();
    }
}