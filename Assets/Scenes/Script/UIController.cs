
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] public Image fillbar, armaBarLeite, armaBarLaranja;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject imagemMuniçãoLeite, imagemMuniçãoLaranja;
    [SerializeField] private GameObject MenuPause,MenuFim,Ui;
    [SerializeField] private RestaurantControler restaurantControler;
    [SerializeField] private WeaponController ArmaLeite;
    [SerializeField] private GameManeger gameManeger;
    [SerializeField] private TextMeshProUGUI pontos,pontosFim;
    [SerializeField] private WeaponController ArmaLaranja;
    [SerializeField]private string scena, scena1;

    private bool isPause;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;

    }

    // Update is called once per frame
    void Update()
    {
        if (restaurantControler.RestaurantDestruido == false)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (isPause)
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
                }

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
}
