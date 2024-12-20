
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    
    // Start is called before the first frame update
    [SerializeField] private string nomeDoLevelDoJogo;
    [SerializeField] private GameObject ImagemLogo, ImagemMenu;
    [SerializeField] private string fase1, fase2;
    [SerializeField] private GameObject PainelMenuPrincipal;
    [SerializeField] private GameObject painelOpcoes;
    [SerializeField] private GameObject painelCreditos;
    [SerializeField] private GameObject painelFase;
    [SerializeField] private GameObject painelSelecionarFase;
    [SerializeField] private GameObject painelHistoria;
    [SerializeField] private GameObject painelTutotial;
    [SerializeField] private GameObject MenuGeral;
    [SerializeField] private GameObject botãoFase2B, botãoFase2;
    private GameManeger gameManeger;
    public bool terminouFase1;
    public bool terminouFase2;
    public bool terminouFase3;
    public bool fazeconcluida;

    public void Start()
    {
        PainelMenuPrincipal.SetActive(true);
        painelOpcoes.SetActive(false);
        painelCreditos.SetActive(false);
        painelFase.SetActive(false);
        painelSelecionarFase.SetActive(false);
        painelHistoria.SetActive(false);
        painelTutotial.SetActive(false);
        MenuGeral.SetActive(true);
        ImagemLogo.SetActive(true);
        ImagemMenu.SetActive(true);
        gameManeger = GetComponent<GameManeger>();
        if(ControladorDeFaes.instace.fase1Concluida == true)
        {
            botãoFase2.SetActive(true);
            botãoFase2B.SetActive(false);
        }
        else
        {
            botãoFase2B.SetActive(true);
            botãoFase2.SetActive(false);
        }

    }
 
    public void Jogar()
    {
        SceneManager.LoadScene(nomeDoLevelDoJogo);
    }
    public void Fase1()
    {
        SceneManager.LoadScene(fase1);
    }
    public void OpcoesAbrir()
    {
        PainelMenuPrincipal.SetActive(false);
        painelOpcoes.SetActive(true);
        painelCreditos.SetActive(false);
        painelFase.SetActive(false);
        painelSelecionarFase.SetActive(false);
        painelHistoria.SetActive(false);
        painelTutotial.SetActive(false);
        MenuGeral.SetActive(true);
        ImagemLogo.SetActive(true);
        ImagemMenu.SetActive(true);

    }
    public void OpçãoJogar()
    {
        PainelMenuPrincipal.SetActive(false);
        painelOpcoes.SetActive(false );
        painelCreditos.SetActive(false);
        painelFase.SetActive(true);
        painelSelecionarFase.SetActive(false);
        painelHistoria.SetActive(false);
        painelTutotial.SetActive(false);
        MenuGeral.SetActive(true);
        ImagemLogo.SetActive(true);
        ImagemMenu.SetActive(true);

    }
    public void SelecionarFase()
    {
        PainelMenuPrincipal.SetActive(false);
        painelOpcoes.SetActive(false);
        painelCreditos.SetActive(false);
        painelFase.SetActive(false);
        painelSelecionarFase.SetActive(true);
        painelHistoria.SetActive(false);
        painelTutotial.SetActive(false);
        MenuGeral.SetActive(true);
        ImagemLogo.SetActive(true);
        ImagemMenu.SetActive(true);

    }
    public void OpcoesFechar()
    {

    }
    public void OpçãoHistoria()
    {
        PainelMenuPrincipal.SetActive(false);
        painelOpcoes.SetActive(false);
        painelCreditos.SetActive(false);
        painelFase.SetActive(false);
        painelSelecionarFase.SetActive(false);
        painelHistoria.SetActive(true );
        painelTutotial.SetActive(false );
        MenuGeral.SetActive(false);
        ImagemLogo.SetActive(false);
        ImagemMenu.SetActive(false);

    }
    public void OpçãoTutorial()
    {
        PainelMenuPrincipal.SetActive(false);
        painelOpcoes.SetActive(false);
        painelCreditos.SetActive(false);
        painelFase.SetActive(false);
        painelSelecionarFase.SetActive(false);
        painelHistoria.SetActive(false);
        painelTutotial.SetActive(true);
        MenuGeral.SetActive(false);
        ImagemLogo.SetActive(false);
        ImagemMenu.SetActive(false);


    }
    public void Creditos()
    {
        painelCreditos.SetActive (true);
        painelOpcoes.SetActive (false);
        PainelMenuPrincipal.SetActive (false);
        painelFase.SetActive(false);
        painelSelecionarFase.SetActive(false);
        painelHistoria.SetActive(false);
        painelTutotial.SetActive(false);
        MenuGeral.SetActive(true);
        ImagemLogo.SetActive(false);
        ImagemMenu.SetActive(false);

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
        painelFase.SetActive(false);
        painelSelecionarFase.SetActive(false);
        painelHistoria.SetActive(false);
        painelTutotial.SetActive(false);
        ImagemLogo.SetActive(true);
        ImagemMenu.SetActive(true);

    }
    public void Fase2()
    {
        SceneManager.LoadScene(fase2);
    }
}
