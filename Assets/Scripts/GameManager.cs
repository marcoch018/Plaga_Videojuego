using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instancia;

    [Header("Estado")]
    public int bajas;
    public int bajasParaGanar = 30;

    void Awake()
    {
        if (Instancia == null) Instancia = this;
        else Destroy(gameObject);
    }

    public void RegistrarBaja()
    {
        bajas++;
        if (bajas >= bajasParaGanar) Debug.Log("¡Victoria!");
    }

    public void GameOver()
    {
        Debug.Log("Game Over - Bajas: " + bajas);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}