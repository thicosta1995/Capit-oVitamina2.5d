using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantControler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]private int hpRestaurante,HpMax;
    public bool RestaurantDestruido = false;
    [SerializeField] BarraHpFlutuante barraHpFlutuante;
    void Start()
    {
        barraHpFlutuante = GetComponentInChildren<BarraHpFlutuante>();
        hpRestaurante = HpMax;
        barraHpFlutuante.UpDateHealhBar(hpRestaurante, HpMax);
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
            barraHpFlutuante.UpDateHealhBar(hpRestaurante, HpMax);
        }
      
    }
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.layer == 7)
        {
            hpRestaurante = hpRestaurante - 10;
            barraHpFlutuante.UpDateHealhBar(hpRestaurante, HpMax);
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
