using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisãoInimigo : MonoBehaviour
{

    public MaquinaEstados maquinaEstados;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            maquinaEstados.vision = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            maquinaEstados.vision = false;
        }
    }
}
