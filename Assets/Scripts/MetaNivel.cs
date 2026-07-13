using UnityEngine;

public class MetaNivel : MonoBehaviour
{
    private Collider colisionador;
    private bool abierta = false;

    void Start()
    {
        colisionador = GetComponent<Collider>();
     
        colisionador.isTrigger = false;
    }

    public void Abrir()
    {
        abierta = true;
        colisionador.isTrigger = true; 
    }

    
    public void IntentarCruzar()
    {
        if (!abierta && GameManager.instancia != null)
        {
            GameManager.instancia.MostrarMensaje("Elimina a todos los enemigos primero");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PrimeraPersona>() != null)
        {
            if (GameManager.instancia != null)
                GameManager.instancia.JugadorLlegoAMeta();
        }
    }
}
