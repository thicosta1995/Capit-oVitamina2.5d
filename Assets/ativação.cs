using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ativação : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Image R;

    void Start()
    {
        R.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Jogador"))
        {
            R.enabled = true;
        }
        
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Jogador"))
        {
            R.enabled = false;
        }

    }
}
