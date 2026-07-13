using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemigoIA : MonoBehaviour
{
    [Header("Movimiento")]
    public Transform jugador;         
    public float rangoDeteccion = 15f; // distancia a la que el enemigo detecta y persigue

    [Header("Ataque")]
    public float rangoAtaque = 10;   // distancia minima para empezar a disparar
    public float cadenciaDisparo = 1f;
    public int dano = 1;
    public AudioClip sonidoDisparo;

    private NavMeshAgent agente; // el componente de Unity que mueve al enemigo por la NavMesh
    private AudioSource fuente;
    private float proximoDisparo = 0f;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        fuente = GetComponent<AudioSource>();


        if (jugador == null)
        {
            PrimeraPersona pj = FindFirstObjectByType<PrimeraPersona>();
            if (pj != null) jugador = pj.transform;
        }
    }

    void Update()
    {
        if (jugador == null) return; 

        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia <= rangoDeteccion)
        {
            //calcula el camino mas corto mientras el jugador se mueve para que lo siga en vivo
            agente.SetDestination(jugador.position);
        }
        else
        {
            // cancela el camino actual y se queda quieto
            agente.ResetPath();
        }

        // si esta lo bastante cerca dispara
        if (distancia <= rangoAtaque && Time.time >= proximoDisparo)
        {
            proximoDisparo = Time.time + cadenciaDisparo;
            Disparar();
        }
    }

    void Disparar()
    {
        Vector3 origen = transform.position + Vector3.up * 1.5f;
        Vector3 destino = jugador.position + Vector3.up * 0.6f;
        Vector3 direccion = (destino - origen).normalized;

        if (sonidoDisparo != null && fuente != null) fuente.PlayOneShot(sonidoDisparo);

        // si lo encuentra le aplica daño.
        if (Physics.Raycast(origen, direccion, out RaycastHit hit, rangoAtaque + 5f))
        {
            Vida v = hit.collider.GetComponentInParent<Vida>();
            // verifica que sea vida del jugador para que un enemigo no le pegue por error a otro enemigo que se le cruce en la mira
            if (v != null && v.esJugador)
            {
                v.RecibirDano(dano);
            }
        }
    }
}
