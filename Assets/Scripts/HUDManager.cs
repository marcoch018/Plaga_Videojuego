using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instancia;

    [Header("Referencias UI")]
    public TMP_Text textoVida;
    public TMP_Text textoVidaMax;
    public TMP_Text textoBajas;
    public TMP_Text textoTiempo;
    public TMP_Text textoArma;
    public TMP_Text textoMunicion;
    public Image barraVida;
    public Image barraVidaSuave;
    public Image barraXP;
    public Image indicadorDano;

    [Header("Colores de Vida")]
    public Color colorVidaAlta  = new Color(0.55f, 0.80f, 0.35f);
    public Color colorVidaMedia = new Color(0.95f, 0.72f, 0.20f);
    public Color colorVidaBaja  = new Color(0.85f, 0.20f, 0.15f);

    float vidaMaxima = 100f;
    float vidaActual = 100f;
    int bajas;
    string armaActual = "PISTOLA";
    float tiempoInicio;
    float vidaMostrada = 100f;

    void Awake()
    {
        if (Instancia == null) Instancia = this;
        else { Destroy(gameObject); return; }
    }

    void Start()
    {
        tiempoInicio = Time.time;
        vidaMostrada = vidaActual;
        SetVida(vidaActual, vidaMaxima);
        AddBaja(0);
        SetArma(armaActual);
        SetMunicion(30, 30);
    }

    void Update()
    {
        float t = Time.time - tiempoInicio;
        if (textoTiempo)
            textoTiempo.text = $"{Mathf.FloorToInt(t / 60f):00}:{Mathf.FloorToInt(t % 60f):00}";

        if (barraVidaSuave)
        {
            vidaMostrada = Mathf.Lerp(vidaMostrada, vidaActual, Time.deltaTime * 4f);
            float pctSuave = vidaMaxima > 0f ? vidaMostrada / vidaMaxima : 0f;
            barraVidaSuave.fillAmount = pctSuave;
        }
    }

    public void SetVida(float actual, float maxima = -1f)
    {
        vidaActual = Mathf.Max(0f, actual);
        if (maxima > 0f) vidaMaxima = maxima;

        float pct = vidaMaxima > 0f ? vidaActual / vidaMaxima : 0f;
        Color c = pct > 0.6f ? colorVidaAlta : pct > 0.3f ? colorVidaMedia : colorVidaBaja;

        if (textoVida)    textoVida.text = $"{Mathf.CeilToInt(vidaActual):000}";
        if (textoVidaMax) textoVidaMax.text = $"/ {Mathf.CeilToInt(vidaMaxima):000}";

        if (barraVida)
        {
            barraVida.fillAmount = pct;
            barraVida.color = c;
        }
    }

    public void AddBaja(int cantidad = 1)
    {
        bajas += cantidad;
        if (textoBajas) textoBajas.text = $"{bajas:000}";
    }

    public void SetArma(string nombre)
    {
        armaActual = nombre.ToUpper();
        if (textoArma) textoArma.text = armaActual;
    }

    public void SetMunicion(int actual, int maxima)
    {
        if (textoMunicion) textoMunicion.text = $"{actual:00} <size=60%>/ {maxima:00}</size>";
    }

    public void SetXP(float pct)
    {
        if (barraXP) barraXP.fillAmount = Mathf.Clamp01(pct);
    }
    public void MostrarDano()
    {
        if (indicadorDano == null) return;
        StopAllCoroutines();
        StartCoroutine(FadeVineta(0.35f, 0.25f));
    }

    public void MostrarDanoFatal()
    {
        if (indicadorDano == null) return;
        StopAllCoroutines();
        StartCoroutine(FadeVineta(0.6f, 0.6f));
    }

    System.Collections.IEnumerator FadeVineta(float alphaMax, float duracion)
    {
        float t = 0f;
        Color c = indicadorDano.color;
        while (t < duracion)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(alphaMax, 0f, t / duracion);
            indicadorDano.color = c;
            yield return null;
        }
        c.a = 0f;
        indicadorDano.color = c;
    }
}