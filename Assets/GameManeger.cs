using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManeger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public int pontuação;
    [SerializeField] private int numberEnemys;
    [SerializeField] public bool inimigoMorreu;
     public  bool fazeConcluida;
    [SerializeField]private int fazes;
    [SerializeField] private int registro;

    void Start()
    {
        pontuação = 0;
    }

    // Update is called once per frame
    void Update()
    {
        load();
        adicionarFasesConcluidas();
    }
   public  void  adicionarPontos()
    {
        pontuação = pontuação + 1;
    }
    public void adicionarFasesConcluidas()
    {
        if (fazeConcluida == true&& registro == 0)
        {
            fazes = fazes + 1;
            PlayerPrefs.SetInt("Fazes Concluidas", fazes);
            registro = registro + 1;
        }
    }
    public void load()
    {
        fazes = PlayerPrefs.GetInt("Fazes Concluidas",0);

    }
    public void delet()
    {
        PlayerPrefs.DeleteAll();
    }
}
