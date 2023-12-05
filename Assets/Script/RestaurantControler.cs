using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantControler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private int hpRestaurante;
    public bool RestaurantDestruido = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Morreu();
            
    }
    private void OnTriggerEnter(Collider other)
    {
         if(other.gameObject.CompareTag("C"))
        {
            hpRestaurante = hpRestaurante - 10;
        }
      
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.layer == 7)
        {
            hpRestaurante = hpRestaurante - 10;
        }
    }
    public void Morreu()
    {
        if(hpRestaurante <= 0)
        {
            RestaurantDestruido =true;
        }
    }
}
