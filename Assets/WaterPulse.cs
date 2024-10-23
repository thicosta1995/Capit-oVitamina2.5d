using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPulse : MonoBehaviour
{
    [SerializeField]private Rigidbody rb;
    public Rigidbody rbHidrante;
    [SerializeField] private float velocidade;
    // Start is called before the first frame update
    void Start()
    {
   
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        { 
        //if(other.gameObject.CompareTag("Jogador"))
        //{

        //    rbHidrante = other.gameObject.GetComponent<Rigidbody>();
           
        //    rbHidrante.AddForce(Vector3.up*velocidade,ForceMode.Force);
            
        //    Debug.Log("entrou");

        }
    }
    
}
