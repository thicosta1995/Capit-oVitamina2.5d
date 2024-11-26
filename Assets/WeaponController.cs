using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine.Animations.Rigging;
using UnityEngine.Animations;

public class WeaponController : MonoBehaviour
{
    public List<Transform> firePoints;

    [SerializeField] private float rotationSmoothSpeed = 5f;
    public Camera mainCamera;
    private Vector3 lastDirection = Vector3.zero;
    [SerializeField] private float directionTolerance = 0.01f;
    [SerializeField] private Rig aimRig;
    [SerializeField] public Transform Player;
    [SerializeField] public ParticleSystem leite;
    [SerializeField] public bool viraDoDireita;
    [SerializeField] public bool viraDoEsquerda;
    [SerializeField] public float municaoDeLeite;
    [SerializeField] private float municaoDeLeiteMax = 10000;
    [SerializeField] private Transform pivot;
    [SerializeField] private float rotSpeed;
    public bool semLeite;
    private Transform esquerda, direita;
    public bool recarregarLeite;
    [SerializeField] PlayerMovement player;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public int numeroTiros = 3;
    public float intervaloTiros = 0.1f;
    public GameObject explosiveProjectilePrefab;
    [SerializeField] public float municaoDeLaraja;
    [SerializeField] private float municaoDeLarajaMax = 300;
    public bool recarregandoAlaranja;
    public bool semLaranja;
    private bool isFiring; // Indica se a arma está atirando
    public GameObject[] armas; // Um array de GameObjects representando suas diferentes armas.
    private int armaAtual = 0; // O índice da arma atual.
    [SerializeField] float posiçãoArmaAtual;
    [SerializeField] private AudioClip FruiFallSound;
    [SerializeField] private AudioClip FaucetSound;
    private InputMannegerControl control;
    private AudioSource audioSource;
    private float aimWeight;
    [SerializeField] bool soundPlay;
    [SerializeField] bool jaTocou;
    public LayerMask parede;
    public bool naoVira;



   


    private void Start()
    {

        viraDoDireita = true;
        viraDoEsquerda = false;
        municaoDeLaraja = municaoDeLarajaMax;
        semLaranja = false;
        recarregandoAlaranja = false;
        municaoDeLeite = municaoDeLeiteMax;
        semLeite = false;
        naoVira = false;
        recarregarLeite = false;
        aimWeight = 1.0f;
        soundPlay = false;

        audioSource = GetComponent<AudioSource>();

    }
    private void Update()
    {

        // Vector3 mousePositionScreen = Input.mousePosition;



        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        
        if (municaoDeLaraja <= 0)
        {
            semLaranja = true;
        }
        if (municaoDeLeite <= 0)
        {
            semLeite = true;
        }
        Vector3 aimPos = Vector3.zero;
        if (Physics.Raycast(ray, out RaycastHit hit,parede))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
            Vector3 direction = (hit.point - pivot.position).normalized;
            Debug.Log(direction.normalized);
            
            //pivot.forward = direction;
            //pivot.transform.rotation = Quaternion.LookRotation(direction).normalized;
            SetWeaponDirection(direction.normalized);
        }
        ////aimRig.weight = Mathf.Lerp(aimRig.weight,aimWeight,Time.deltaTime*20);

        if (InputMannegerControl.GetFireInput() && player.armaAtual == 1 && municaoDeLaraja > 0)
        {
            audioSource.clip = FruiFallSound;
            DispararTiros();
        }
        else if (InputMannegerControl.GetFireInput() && player.armaAtual == 1)
        {

        }
        if (InputMannegerControl.GetFireInput() && player.armaAtual == 0 && municaoDeLeite > 0)
        {
            audioSource.clip = FaucetSound;
            soundPlay = true;
            leite.Play();


        }
        else if (InputMannegerControl.GetFireInput() == false && player.armaAtual == 0)
        {
            leite.Stop();
            soundPlay = false;
        }
        if (posiçãoArmaAtual <= 81 || posiçãoArmaAtual >= -61 && viraDoDireita == true)
        {

            if (posiçãoArmaAtual >= 82 || posiçãoArmaAtual <= -61)
            {
                viraDoEsquerda = false;
                viraDoDireita = true;

                Debug.Log("Entrou no primeiro");
            }

            else if (posiçãoArmaAtual >= -61)
            {
                viraDoEsquerda = true;
                viraDoDireita = false;
                Debug.Log("Entrou no Segundo");
            }

            //    //Player.rotation = Quaternion.Euler(0f, 90, 0);

        }
        if (soundPlay == true)
        {


            audioSource.loop = true;


            if (jaTocou == false)
            {
                audioSource.Play();
                jaTocou = true;
            }
        }
        else
        {
            audioSource.Stop();
            jaTocou = false;



            audioSource.loop = false;


        }
        if (posiçãoArmaAtual >= 82 || posiçãoArmaAtual <= -62)
        {
            if (posiçãoArmaAtual < -180)
            {
                viraDoEsquerda = false;
                viraDoDireita = true;
                Debug.Log("Entrou no terceiro");
            }
            else if (posiçãoArmaAtual >= 81)
            {
                viraDoEsquerda = false;
                viraDoDireita = true;
                Debug.Log("Entrou no quarto");
            }
        }
        //else if( posiçãoArmaAtual >=-67 && viraDoDireita == true)
        //{
        //    viraDoEsquerda = true;
        //    viraDoDireita = false;
        //}
        //else if(posiçãoArmaAtual >= 90   && viraDoEsquerda == true)
        //{

        //    Debug.Log(" entrou na Direita");
        //    //    //Player.rotation = Quaternion.Euler(0f, 90, 0);
        //    viraDoEsquerda = false;
        //    viraDoDireita = true;
        //}
        //else if( posiçãoArmaAtual <= -69  && viraDoEsquerda == true)
        //{
        //    viraDoEsquerda = false;
        //    viraDoDireita = true;

        //}


        if (recarregandoAlaranja == true)
        {
            municaoDeLaraja = municaoDeLarajaMax;
            semLaranja = false;
            recarregandoAlaranja = false;
        }
        if (recarregarLeite == true)
        {
            municaoDeLeite = municaoDeLeiteMax;
            semLeite = false;
            recarregarLeite = false;
        }

        animaçãoLeite();
        //virarPlayer();
    }


    //void virarPlayer()
    //{
    //    if(viraDoDireita == true)
    //    {

    //        Player.rotation = Quaternion.Euler(0f, 0, 0f);
    //    }
    //    if(viraDoEsquerda == true)
    //    {
    //        Player.rotation = Quaternion.Euler(0f, 180f, 0f);
    //    }
    //}
    void SetWeaponDirection(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.normalized.y, direction.normalized.x) * Mathf.Rad2Deg;
        // transform.rotation = Quaternion.Euler(0f, 0f, angle);
        posiçãoArmaAtual = angle;

        if (posiçãoArmaAtual > 60 && posiçãoArmaAtual < 120)
        {
            naoVira = true;
        }
        else
        {
            naoVira = false;
            pivot.rotation = Quaternion.Euler(0f, 0f, angle - 85);
        }
        


        // pivot.RotateAround(pivot.position,direction, angle-80);
        //  pivot.RotateAroundLocal(direction, angle);
        // pivot.eulerAngles =new Vector3(0f, 0f, angle);  


        //if (angle < 90 && angle > -81 && viraDoEsquerda == false)
        //{
        //    Player.localRotation= Quaternion.Euler(0f, 90f, 0f);
        //    //Player.rotation = Quaternion.Euler(0f, 90, 0);
        //    viraDoEsquerda = true;
        //    viraDoDireita = false;
        //}
        //if (angle < -90 && angle < -82 && viraDoDireita == false)
        //{
        //    // Player.rotation = Quaternion.Euler(0f, -90, 0);
        //    Player.localRotation = Quaternion.Euler(0f, -90f, 0f);
        //    viraDoDireita = true;
        //    viraDoEsquerda = false;
        //}
        //if (angle > -70 && angle < 89 && viraDoEsquerda == false)
        //{
        //    Player.localRotation = Quaternion.Euler(0f, 90f, 0f);
        //    //Player.rotation = Quaternion.Euler(0f, 90, 0);
        //    viraDoEsquerda = true;
        //    viraDoDireita = false;
        //}

        //if (angle > -89 && angle < 91 && viraDoDireita == false)
        //{
        //    Player.localRotation = Quaternion.Euler(0f,-90f, 0f);
        //   // Player.rotation = Quaternion.Euler(0f, -90, 0);
        //    viraDoDireita = true;
        //    viraDoEsquerda = false;
        //}
        



    }

    //void SetWeaponDirectionUp(Vector3 direction)
    //{
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    angle -= 90f;
    //    transform.rotation = Quaternion.Euler(0f, 0f, angle);
    //}

    //void SetWeaponDirectionRight(Vector3 direction)
    //{
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    angle -= 180f;
    //    transform.rotation = Quaternion.Euler(0f, 0f, angle);
    //}

    //void SetWeaponDirectionDown(Vector3 direction)
    //{
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    angle -= 270f;
    //    transform.rotation = Quaternion.Euler(0f, 0f, angle);
    //}

    //void SetWeaponDirectionLeft(Vector3 direction)
    //{
    //    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //    transform.rotation = Quaternion.Euler(0f, 0f, angle);
    //}

    void DispararTiros()
    {
        if (!isFiring)
        {
            isFiring = true;

            audioSource.Play();
            StartCoroutine(DispararTirosCoroutine());
        }
    }

    void DispararTirosExplosivos()
    {
        if (!isFiring)
        {
            isFiring = true;
            StartCoroutine(DispararTirosExplosivosCoroutine());
        }
    }
    public void animaçãoLeite()

    {
        if (player.armaAtual == 0)
        {
            if (leite.isPlaying && municaoDeLeite >= 0)
            {
                municaoDeLeite = municaoDeLeite - 1;
            }
            else if (municaoDeLeite <= 0)
            {
                leite.Stop();
            }
        }
    }
    IEnumerator DispararTirosCoroutine()
    {
        for (int i = 0; i < numeroTiros; i++)
        {
            foreach (Transform firePoint in firePoints)
            {

                GameObject projectileGO = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
                Rigidbody projectileRigidbody = projectileGO.GetComponent<Rigidbody>();
                projectileRigidbody.AddForce(-firePoint.forward * projectileSpeed, ForceMode.Impulse);
                municaoDeLaraja = municaoDeLaraja - 15;


            }

            yield return new WaitForSeconds(intervaloTiros);
        }

        isFiring = false;

    }

    IEnumerator DispararTirosExplosivosCoroutine()
    {
        for (int i = 0; i < numeroTiros; i++)
        {
            //foreach (Transform firePoint in firePoints)
            //{
            //    GameObject explosiveProjectileGO = Instantiate(explosiveProjectilePrefab, firePoint.position, firePoint.rotation);
            //    Rigidbody explosiveProjectileRigidbody = explosiveProjectileGO.GetComponent<Rigidbody>();
            //    explosiveProjectileRigidbody.AddForce(firePoint.right * projectileSpeed, ForceMode.Impulse);
            //}
            yield return new WaitForSeconds(intervaloTiros);
        }

        isFiring = false;
    }
    
}