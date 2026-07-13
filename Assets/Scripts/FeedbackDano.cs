using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FeedbackDano : MonoBehaviour
{
    public Image imagenDano;   // imagen roja 
    public float alphaMax = 0.4f; // que tan opaca se pone al recibir daño
    public float duracion = 0.3f; // segundos que tarda en desvanecerse

    private Coroutine parpadeoActual; // referencia a la corrutina activa

    
    public void MostrarDano()
    {
        // Si el jugador recibe dos golpes muy seguidos
        if (parpadeoActual != null)
        {
            StopCoroutine(parpadeoActual);
        }
        parpadeoActual = StartCoroutine(Parpadear());
    }

    IEnumerator Parpadear()
    {
        // pantalla roja 
        Color c = imagenDano.color;
        c.a = alphaMax;
        imagenDano.color = c;

        float tiempo = 0f;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime; // Time.deltaTime = segundos que pasaron desde el frame anterior
            float alpha = Mathf.Lerp(alphaMax, 0f, tiempo / duracion); // interpolar
            c.a = alpha;
            imagenDano.color = c;
            yield return null;
        }

        c.a = 0f;
        imagenDano.color = c;
    }
}
