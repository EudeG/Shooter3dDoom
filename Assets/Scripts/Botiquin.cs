using UnityEngine;

public class Botiquin : MonoBehaviour
{
    public int curacion = 3; 
    public AudioClip sonidoCuracion;

    private void OnTriggerEnter(Collider other)
    {
        Vida vidaJugador = other.GetComponentInParent<Vida>();

        if (vidaJugador != null && vidaJugador.esJugador)
        {
            vidaJugador.Curar(curacion);

            if (sonidoCuracion != null)
                AudioSource.PlayClipAtPoint(sonidoCuracion, transform.position);

            Destroy(gameObject);
        }
    }
}
