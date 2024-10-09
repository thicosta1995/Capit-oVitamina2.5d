using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoçaDano : MonoBehaviour
{

   [SerializeField]private PlayerMovement player;


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
        if(other.gameObject.CompareTag("Jogador"))
        {
            player.Hp = player.Hp -0.1f;
        }
    }
}
