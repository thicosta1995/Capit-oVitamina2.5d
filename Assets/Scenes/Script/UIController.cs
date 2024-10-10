
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] public Image fillbar, armaBarLeite, armaBarLaranja;
    [SerializeField] private Slider slider,sliderMusic,sliderSound;
    [SerializeField] private GameObject imagemMuniçãoLeite, imagemMuniçãoLaranja;
    [SerializeField] private GameObject MenuPause,MenuFim,Ui,MenuOpções;
    [SerializeField] private RestaurantControler restaurantControler;
    [SerializeField] private WeaponController ArmaLeite;
    [SerializeField] private GameManeger gameManeger;
    [SerializeField] private TextMeshProUGUI pontos,pontosFim;
    [SerializeField] private WeaponController ArmaLaranja;
    [SerializeField]private string scena, scena1;

    private bool isPause;
    [SerializeField]private bool opçãoActive;
    // Start is called before the first frame update
    void Start()
    {
        MenuPause.SetActive(false);
        Ui.SetActive(true);
        MenuOpções.SetActive(false);
        Time.timeScale = 1.0f;

    }

    // Update is called once per frame
    void Update()
    {
        if (restaurantControler.RestaurantDestruido == false)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (isPause && opçãoActive == false)
                {
                    isPause = false;
                    Time.timeScale = 1f;
                    MenuPause.SetActive(false);
                    Ui.SetActive(true);
                    

                }

                else
                {
                   
                        isPause = true;
                        Time.timeScale = 0f;
                        MenuPause.SetActive(true);
                        Ui.SetActive(false);
                        MenuOpções.SetActive(false);
                    
                   

                }
               

            }
            if (opçãoActive == true)
            {
                MenuPause.SetActive(false);
                Ui.SetActive(false);
                MenuOpções.SetActive(true);

            }
            else
            {
                MenuPause.SetActive(true);
                Ui.SetActive(false);
                MenuOpções.SetActive(false);
            }
        }
        if(player.Hp<=0)
        {

            Time.timeScale = 0f;
            MenuFim.SetActive(true);
            MenuPause.SetActive(false);

        }
        if (restaurantControler.RestaurantDestruido == true)
        {
           
                Time.timeScale = 0f;
                Ui.SetActive(false);
                MenuFim.SetActive(true);
                MenuPause.SetActive(false);

            pontosFim.text = gameManeger.pontuação.ToString() + "X";
            pontos.text = "";

        }
        fillbar.fillAmount = player.Hp / 100;
        if(player.armaAtual==0)
        {
            imagemMuniçãoLeite.SetActive(true);
            imagemMuniçãoLaranja.SetActive(false);
            armaBarLeite.fillAmount = ArmaLeite.municaoDeLeite / 1000;

        }
        else if(player.armaAtual==1) 
        {
            imagemMuniçãoLeite.SetActive(false);
            imagemMuniçãoLaranja.SetActive(true);
            armaBarLaranja.fillAmount = ArmaLaranja.municaoDeLaraja / 300;
           
        }

        pontos.text = gameManeger.pontuação.ToString() +"X";
    }
    public void BottaoMenu()
    {
        SceneManager.LoadScene(scena);
    }
    public void BottaoReset()
    {
        SceneManager.LoadScene(scena1);
    }
    public void Sair()
    {
        Application.Quit();
    }
    public void Opções()
    {
        opçãoActive = true; 
    }
    public void Voltar()
    {
        opçãoActive = false;
    }
}
