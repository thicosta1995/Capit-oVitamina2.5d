
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string nomeDoLevelDoJogo;
    [SerializeField] private GameObject PainelMenuPrincipal;
    [SerializeField] private GameObject painelOpcoes;
    [SerializeField] private GameObject painelCreditos;


    public void Start()
    {
        PainelMenuPrincipal.SetActive(true);
        painelOpcoes.SetActive(false);
        painelCreditos.SetActive(false);
    }
    public void Jogar()
    {
        SceneManager.LoadScene(nomeDoLevelDoJogo);
    }

    public void OpcoesAbrir()
    {
        PainelMenuPrincipal.SetActive(false);
        painelOpcoes.SetActive(true);
        painelCreditos.SetActive(false);
    }
    public void OpcoesFechar()
    {

    }

    public void Creditos()
    {
        painelCreditos.SetActive (true);
        painelOpcoes.SetActive (false);
        PainelMenuPrincipal.SetActive (false);
    }
    public void Sair() 
    {
        Application.Quit();
    }
    public void Voltar()
    {

        PainelMenuPrincipal.SetActive(true);
        painelOpcoes.SetActive(false);
        painelCreditos.SetActive(false);
    }
}
