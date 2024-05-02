using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class holeControl : MonoBehaviour
{
    [SerializeField]private GameObject player;
    [SerializeField] public Transform spawn;
    [SerializeField] private Transform buracoLimite;
    public voltarNoSpawm voltarNospawm;

    public PlayerMovement playerMovement;
    


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
 {
//        if (voltarNospawm.passou == true)
//        {

//            playerMovement.rb.useGravity = false;
//            player.transform.position = spawn.position;
            

//            Debug.Log(" Posição do spawn  " + player.transform.position);

//               // playerMovement.CameraCair.SetActive(true);
//            playerMovement.ResumeFocus();
//            voltarNospawm.passou = false;
//}
 }
    private void OnTriggerEnter(Collider other)
    {
        
   

    }
}
