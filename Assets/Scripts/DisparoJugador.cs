using System.Collections;
using UnityEngine;

public class DisparoJugador : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject prefabProyectil;
    public GameObject prefabArea;

    [Header("Referencias")]
    public Transform armaHolder;
    public SpriteRenderer srArma;
    public SpriteRenderer srJugador;

    [Header("Sprites (orden: pistola, escopeta, rifle, lanzallamas)")]
    public Sprite[] sprites;
    public string[] nombres = { "PISTOLA", "ESCOPETA", "RIFLE", "LANZALLAMAS" };

    [Header("Cadencia / daño por arma")]
    public float[] cadencias   = { 0.30f, 0.80f, 0.60f, 0.15f };
    public float[] danos       = { 25f,   15f,   20f,   8f    };
    public float[] velocidades = { 15f,   12f,   18f,   10f   };
    public float[] vidas       = { 3f,    1.5f,  3f,    0.6f  };

    [Header("Disparo")]
    public int[] proyectilesPorDisparo = { 1, 8, 1, 1 };
    public float[] dispersiones        = { 0f, 40f, 0f, 0f };
    public int[] balasPorRafaga        = { 1, 1, 3, 1 };
    public float intervaloRafaga = 0.05f;

    [Header("Offset del cañón respecto al pivote (por arma)")]
    public float[] offsetCanon = { 0.45f, 0.85f, 1.10f, 0.95f };

    [Header("Volteo del arma")]
    public float offsetManoDerecha   = 1.5f;
    public float offsetManoIzquierda = -1.5f;
    public float offsetY = 5.5f;

    int armaActiva;
    float proximoDisparo;
    int municion = 30;
    const int MunicionMax = 30;

    void Start() => Equipar(0);

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) Equipar(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) Equipar(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) Equipar(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) Equipar(3);

        Vector3 raton = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        raton.z = 0f;
        Vector2 dir = (raton - armaHolder.position).normalized;
        float angulo = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bool mirandoIzquierda = dir.x < 0f;

        armaHolder.rotation = Quaternion.Euler(0f, 0f, angulo);
        srArma.flipY = mirandoIzquierda;

        if (srJugador != null) srJugador.flipX = mirandoIzquierda;

        Vector3 pos = armaHolder.localPosition;
        pos.x = mirandoIzquierda ? offsetManoIzquierda : offsetManoDerecha;
        pos.y = offsetY;
        armaHolder.localPosition = pos;

        if (Input.GetButton("Fire1") && Time.time >= proximoDisparo && municion > 0)
        {
            Disparar();
            municion--;
            HUDManager.Instancia?.SetMunicion(municion, MunicionMax);
            proximoDisparo = Time.time + cadencias[armaActiva];
        }

        if (Input.GetButtonUp("Fire1") && municion <= 0)
        {
            municion = MunicionMax;
            HUDManager.Instancia?.SetMunicion(municion, MunicionMax);
        }
    }

    void Equipar(int i)
    {
        armaActiva = i;
        srArma.sprite = sprites[i];
        municion = MunicionMax;
        HUDManager.Instancia?.SetArma(nombres[i]);
        HUDManager.Instancia?.SetIconoArma(sprites[i]);
        HUDManager.Instancia?.SetMunicion(municion, MunicionMax);
    }

    void Disparar()
    {
        if (armaActiva == 3) { DispararArea(); return; }
        if (balasPorRafaga[armaActiva] > 1) { StartCoroutine(Rafaga()); return; }
        if (proyectilesPorDisparo[armaActiva] > 1) { DispararEscopeta(); return; }
        DispararUno(DireccionRaton());
    }

    IEnumerator Rafaga()
    {
        int n = balasPorRafaga[armaActiva];
        for (int i = 0; i < n; i++)
        {
            DispararUno(DireccionRaton());
            yield return new WaitForSeconds(intervaloRafaga);
        }
    }

    void DispararEscopeta()
    {
        Vector2 baseDir = DireccionRaton();
        float baseAng = Mathf.Atan2(baseDir.y, baseDir.x) * Mathf.Rad2Deg;
        float mitad = dispersiones[armaActiva] * 0.5f;
        int n = proyectilesPorDisparo[armaActiva];

        for (int i = 0; i < n; i++)
        {
            float t = n == 1 ? 0.5f : (float)i / (n - 1);
            float ang = baseAng + Mathf.Lerp(-mitad, mitad, t);
            Vector2 dir = new Vector2(Mathf.Cos(ang * Mathf.Deg2Rad), Mathf.Sin(ang * Mathf.Deg2Rad));
            DispararUno(dir);
        }
    }

    void DispararUno(Vector2 dir)
    {
        Vector3 pos = ObtenerBocaCanon();
        float angulo = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var go = Instantiate(prefabProyectil, pos, Quaternion.Euler(0f, 0f, angulo));
        var p = go.GetComponent<Proyectil>();
        if (p != null)
        {
            p.dano = danos[armaActiva];
            p.velocidad = velocidades[armaActiva];
            p.tiempoVida = vidas[armaActiva];
        }
    }

    void DispararArea()
    {
        if (prefabArea == null) return;
        Vector2 dir = DireccionRaton();
        Vector2 centro = (Vector2)ObtenerBocaCanon() + dir * 1f;
        var areaGO = Instantiate(prefabArea, centro, Quaternion.identity);
        var a = areaGO.GetComponent<AreaDano>();
        if (a != null) a.Configurar(danos[armaActiva], 0.2f, 0.6f, 2f);
    }

    Vector3 ObtenerBocaCanon() => armaHolder.position + armaHolder.right * offsetCanon[armaActiva];

    Vector2 DireccionRaton()
    {
        Vector3 raton = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        raton.z = 0f;
        return ((Vector2)(raton - armaHolder.position)).normalized;
    }
}