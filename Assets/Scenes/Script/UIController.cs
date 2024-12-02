
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
public class UIController : MonoBehaviour
{
    [SerializeField] private PlayerMovement player;
    [SerializeField] public Image fillbar, armaBarLeite, armaBarLaranja;
    [SerializeField] private Slider slider, sliderMusic, sliderSound;
    [SerializeField] private GameObject imagemMuni��oLeite, imagemMuni��oLaranja;
    [SerializeField] private GameObject MenuPause, MenuFim, Ui, MenuOp��es;
    [SerializeField] private RestaurantControler restaurantControler;
    [SerializeField] private WeaponController ArmaLeite;
    [SerializeField] private GameManeger gameManeger;
    [SerializeField] private TextMeshProUGUI pontos, pontosFim;
    [SerializeField] private WeaponController ArmaLaranja;
    [SerializeField] private string scena, scena1;
    [SerializeField] AudioMixer audioMixer;
    private bool isPause;
    [SerializeField] private bool op��oActive = false;
    const string MIXER_MUSIC = "MusicParam";
    const string MIXER_SFC = "SoundParam";
    // Start is called before the first frame update
    private void Awake()
    {
        sliderMusic.onValueChanged.AddListener(SetMusicValue);
        sliderSound.onValueChanged.AddListener(SetSoundValue);
        MenuPause.SetActive(false);
        Ui.SetActive(true);
        MenuOp��es.SetActive(false);
        Time.timeScale = 1.0f;
    }
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        if (restaurantControler.RestaurantDestruido == false)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (isPause && op��oActive == false)
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
                    MenuOp��es.SetActive(false);



                }


            }
            if (op��oActive == true)
            {
                MenuPause.SetActive(false);
                Ui.SetActive(false);
                MenuOp��es.SetActive(true);

            }

        }
        if (player.Hp <= 0)
        {

            Time.timeScale = 0f;
            MenuFim.SetActive(true);
            Ui.SetActive(false);
            MenuPause.SetActive(false);


        }
        if (restaurantControler.RestaurantDestruido == true)
        {

            Time.timeScale = 0f;
            Ui.SetActive(false);
            MenuFim.SetActive(true);
            MenuPause.SetActive(false);

            pontosFim.text = gameManeger.pontua��o.ToString() + "X";
            pontos.text = "";

        }
        fillbar.fillAmount = player.Hp / 100;
        if (player.armaAtual == 0)
        {
            imagemMuni��oLeite.SetActive(true);
            imagemMuni��oLaranja.SetActive(false);
            armaBarLeite.fillAmount = ArmaLeite.municaoDeLeite / 1000;

        }
        else if (player.armaAtual == 1)
        {
            imagemMuni��oLeite.SetActive(false);
            imagemMuni��oLaranja.SetActive(true);
            armaBarLaranja.fillAmount = ArmaLaranja.municaoDeLaraja / 300;

        }

        pontos.text = gameManeger.pontua��o.ToString() + "X";
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
    public void Op��es()
    {
        op��oActive = true;
    }
    public void Voltar()
    {
        op��oActive = false;
        MenuPause.SetActive(true);
        Ui.SetActive(false);
        MenuOp��es.SetActive(false);
    }
    public void Continuar()
    {
        isPause = false;
        Time.timeScale = 1f;
        MenuPause.SetActive(false);
        Ui.SetActive(true);

    }
    private void SetMusicValue(float value)
    {
        audioMixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
    }
    private void SetSoundValue(float value)
    {
        audioMixer.SetFloat(MIXER_SFC, Mathf.Log10(value) * 20);
    }
}
