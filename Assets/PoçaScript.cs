using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class PoçaScript : MonoBehaviour
{

    [SerializeField] private float hpMAX, hpAtual,DanoLeite;
    private Rigidbody rb;
    [SerializeField]public ParticleSystem part;
    private Animator animator;
    [SerializeField] BoxCollider triger;
    private bool isDeath;
   

    // Start is called before the first frame update
    void Start()
    {
        
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        hpAtual = hpMAX;
        

    }

    // Update is called once per frame
    void Update()
    {
        if(hpAtual==  600)
        {
            animator.SetBool("isMedia", true);
            animator.SetBool("isBaixa", false);
        }
          if(hpAtual == 200) 
        {
            animator.SetBool("isMedia", false);
            animator.SetBool("isBaixa", true);
        }
     if(isDeath)
        {
            gameObject.SetActive(false);
        }

    }
    public void TakeDamage(int damage)
    {
        hpAtual -= damage;
        if (hpAtual <= 0)
        {
           isDeath = true;
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.layer == 7)
        {
            Debug.Log("Particle Collision with " + other.name);
            if (!isDeath)
            {
                TakeDamage((int)DanoLeite);
                Debug.Log("Damage Taken from Particle: " + DanoLeite + " | Current HP: " + hpAtual);
            }
        }
    }
  

}


