using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class voltarNoSpawm : MonoBehaviour
{
    public bool passou;
    public PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        passou = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.jaCaiu == true) 
        {
            player.jaCaiu = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Jogador"))
        {
            passou = true;
            Debug.Log("ele  " + passou);
        }
        
    }
}
