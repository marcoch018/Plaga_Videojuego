using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class AreaDano : MonoBehaviour
{
    float dano, intervalo, duracion, proximo;
    readonly HashSet<MovimientoEnemigo> dentro = new();

    public void Configurar(float d, float i, float dur, float radio)
    {
        dano = d; intervalo = i; duracion = dur;
        var c = GetComponent<CircleCollider2D>();
        c.isTrigger = true; c.radius = 1f;
        transform.localScale = Vector3.one * radio;
        Destroy(gameObject, duracion);
    }

    void Update()
    {
        if (Time.time < proximo) return;
        proximo = Time.time + intervalo;

        var copia = new List<MovimientoEnemigo>(dentro);
        foreach (var e in copia)
            if (e) e.RecibirDano(dano);
    }
    void OnTriggerEnter2D(Collider2D o) { if (o.TryGetComponent(out MovimientoEnemigo e)) dentro.Add(e); }
    void OnTriggerExit2D(Collider2D o)  { if (o.TryGetComponent(out MovimientoEnemigo e)) dentro.Remove(e); }
}