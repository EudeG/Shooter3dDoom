using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instancia;

    [Header("UI")]
    public Text textoEnemigos;      // texto de cant de enemigos
    public GameObject panelGameOver;
    public GameObject panelVictoria;
    public Text textoMensaje;       // texto para avisos temporales 

    [Header("Nivel")]
    public MetaNivel puertaMeta;    // el bloque de pared que hace de salida, cambiado de puerta abierta para que el jugador no caiga

    private int enemigosRestantes;
    private bool jugadorEnMeta = false;
    private Coroutine mensajeActual;

    void Awake()
    {
        instancia = this;
    }

    void Start()
    {
        // cantidad de enemigos con vida
        Vida[] todasLasVidas = FindObjectsByType<Vida>(FindObjectsSortMode.None);
        enemigosRestantes = 0;
        foreach (Vida v in todasLasVidas)
        {
            if (!v.esJugador) enemigosRestantes++;
        }
        ActualizarUIEnemigos();

        Time.timeScale = 1f;
        if (panelGameOver != null) panelGameOver.SetActive(false);
        if (panelVictoria != null) panelVictoria.SetActive(false);
        if (textoMensaje != null) textoMensaje.text = "";
    }

    public void EnemigoMuerto()
    {
        enemigosRestantes--;
        if (enemigosRestantes < 0) enemigosRestantes = 0;
        ActualizarUIEnemigos();

        // cuando cae el ultimo enemigo se abre la puerta de salida
        if (enemigosRestantes <= 0 && puertaMeta != null)
        {
            puertaMeta.Abrir();
        }

        VerificarVictoria();
    }

    // jugador atraviesa la puerta abierta
    public void JugadorLlegoAMeta()
    {
        jugadorEnMeta = true;
        VerificarVictoria();
    }

    void VerificarVictoria()
    {
        if (jugadorEnMeta && enemigosRestantes <= 0)
        {
            Victoria();
        }
    }

    void Victoria()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (panelVictoria != null) panelVictoria.SetActive(true);
    }

    public void MostrarGameOver()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (panelGameOver != null) panelGameOver.SetActive(true);
    }

    public void Reintentar()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Muestra un texto en pantalla por unos segundos y despues desaparece
    public void MostrarMensaje(string mensaje, float duracion = 2f)
    {
        if (textoMensaje == null) return;
        if (mensajeActual != null) StopCoroutine(mensajeActual);
        mensajeActual = StartCoroutine(MensajeTemporal(mensaje, duracion));
    }

    IEnumerator MensajeTemporal(string mensaje, float duracion)
    {
        textoMensaje.text = mensaje;
        yield return new WaitForSeconds(duracion);
        textoMensaje.text = "";
    }

    void ActualizarUIEnemigos()
    {
        if (textoEnemigos != null)
            textoEnemigos.text = "Enemigos: " + enemigosRestantes;
    }
}
