using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class visão : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    [SerializeField]private MaquinaEstados maquinaEstados;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if(other.CompareTag("Jogador"))
        {
            maquinaEstados.vendoPlayer = true;
        }
      
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Jogador"))
        {
            maquinaEstados.vendoPlayer = false; // Desativa a booleana quando o jogador sai da zona de detecção
            // Pare a perseguição ou realize outras ações aqui
        }
    }
}
