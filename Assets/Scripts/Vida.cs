using UnityEngine;
using UnityEngine.UI;

public class Vida : MonoBehaviour
{
    public int vidaMax = 3;
    public bool esJugador = false;

    public FeedbackDano feedbackDano;

    public Text textoVida;

    private int vidaActual;

    void Start()
    {
        vidaActual = vidaMax;
        ActualizarVidaUI();
    }

    public void RecibirDano(int cantidad)
    {
        vidaActual -= cantidad;

        if (esJugador && feedbackDano != null)
        {
            feedbackDano.MostrarDano();
        }

        ActualizarVidaUI();

        if (vidaActual <= 0) Morir();
    }

    public void Curar(int cantidad)
    {
        vidaActual = Mathf.Min(vidaActual + cantidad, vidaMax);
        ActualizarVidaUI();
    }

    void Morir()
    {
        if (esJugador)
        {
            if (GameManager.instancia != null) GameManager.instancia.MostrarGameOver();
        }
        else
        {
            if (GameManager.instancia != null) GameManager.instancia.EnemigoMuerto();
            Destroy(gameObject);
        }
    }

    public int VidaActual()
    {
        return vidaActual;
    }

    void ActualizarVidaUI()
    {
        if (textoVida != null)
        {
            textoVida.text = "Vida: " + Mathf.Max(vidaActual, 0) + " / " + vidaMax;
        }
    }
}
