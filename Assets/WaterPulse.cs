using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPulse : MonoBehaviour
{
    [SerializeField]private Rigidbody rb;
    public Rigidbody rbHidrante;
    [SerializeField] private float velocidade;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private float timeActive, timeAtual;
    [SerializeField]private Animator animator;
    private bool rodando;
    
   
    // Start is called before the first frame update
    void Start()
    {

        animator = GetComponent<Animator>();
        timeAtual = 0;
        rodando = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (rodando == false)
        {
            timeAtual = timeAtual + Time.deltaTime;
            if (timeAtual >= timeActive)
            {
                animator.SetBool("LigaAgua", true);
                rodando = true;
               
            }
        }

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
    private void LiguaMangueira()
    {
        particleSystem.Play();
        timeAtual = 0;
    }
    private void DesligarMangueira()
    {
 
        particleSystem.Stop();
        rodando = false;
        animator.SetBool("LigaAgua", false);
        

    }
    
}
