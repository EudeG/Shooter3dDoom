using UnityEngine;
using UnityEngine.UI;

public class Disparar : MonoBehaviour
{
    public Camera camara;
    public int dano = 2;
    public float alcance = 100f;
    public float cadencia = 0.5f;
    public AudioClip sonidoDisparo;
    public GameObject muzzle;

    [Header("Municion")]
    public int municionMax = 12;
    public float tiempoRecarga = 1.5f;
    public Text textoMunicion;

    private int municionActual;
    private bool recargando = false;
    private AudioSource fuente;
    private float proximo = 0f;

    void Start()
    {
        fuente = GetComponent<AudioSource>();
        if (muzzle != null) muzzle.SetActive(false);
        municionActual = municionMax;
        ActualizarUI();
    }

    void Update()
    {
        // Disparar solo si hay balas
        if (!recargando && municionActual > 0 && Input.GetMouseButtonDown(0) && Time.time >= proximo)
        {
            proximo = Time.time + cadencia;
            Disparo();
        }

        // Recarga manual con R
        if (!recargando && Input.GetKeyDown(KeyCode.R) && municionActual < municionMax)
        {
            StartCoroutine(Recargar());
        }
    }

    void Disparo()
    {
        municionActual--;
        ActualizarUI();

        if (sonidoDisparo != null) fuente.PlayOneShot(sonidoDisparo);
        if (muzzle != null)
        {
            muzzle.SetActive(true);
            Invoke("ApagarMuzzle", 0.05f);
        }

        Ray ray = camara.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit, alcance))
        {
            Vida v = hit.collider.GetComponentInParent<Vida>();
            // Evita que el jugador se pueda hacer daño a si mismo
            if (v != null && !v.esJugador) v.RecibirDano(dano);
        }
    }

    System.Collections.IEnumerator Recargar()
    {
        recargando = true;
        ActualizarUI();

        yield return new WaitForSeconds(tiempoRecarga);

        municionActual = municionMax;
        recargando = false;
        ActualizarUI();
    }

    void ActualizarUI()
    {
        if (textoMunicion == null) return;
        textoMunicion.text = recargando ? "Recargando..." : $"{municionActual} / {municionMax}";
    }

    void ApagarMuzzle()
    {
        if (muzzle != null)
        {
            muzzle.SetActive(false);
        }
    }
}
