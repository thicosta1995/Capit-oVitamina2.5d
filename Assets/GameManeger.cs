using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManeger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public int pontuação;
    [SerializeField] private int numberEnemys;
    [SerializeField] public bool inimigoMorreu;

    void Start()
    {
        pontuação = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
       
    }
   public  void  adicionarPontos(int pontos)
    {
        pontuação = pontuação + pontos;
    }
}
