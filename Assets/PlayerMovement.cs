

using DG.Tweening;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;


public class PlayerMovement : MonoBehaviour
{
    CharacterController controle;
    public float moveSpeed = 1f;
    public GameObject CameraCair;
    public float jumpForce = 5f;
    public Vector3 xVelocity;
    public Vector3 yVelocity;
    public Vector3 finalVelocity;
    public Rigidbody rb;
    public float gravity;
    public float timeCollider;
    private bool isJumping = false;
    public float maxHeight = 2f;
    public float jumpSpeed;
    public float timeToPeak = 1f;
    public float Hp;
    public CharacterController control;
    public bool jaCaiu;
    public WeaponController armaLaranja;
    public Collider colliderPlayer;
    public WeaponController armaLeite;
    public bool Ispouse;
    public voltarNoSpawm voltarNospawm;
    public Text Hptx;
    public GameObject[] armas; // Um array de GameObjects representando suas diferentes armas.
    public int armaAtual = 0; // O índice da arma atual.
    ParticleSystem leite;
    public float stopDuration = 4f; // Duração em segundos para parar o foco
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip HurtSound;
    [SerializeField]private AudioClip WalkSound;

    private AudioSource audioSource;

    public float horizontal;
    public CinemachineVirtualCamera virtualCamera;
    private void Awake()
    {

      

        rb = GetComponent<Rigidbody>();
        Hp = 100;
        gravity = 2 * maxHeight / Mathf.Pow(timeToPeak, 2);
        jumpSpeed = gravity * timeToPeak;
        control.enabled = true;
        audioSource = GetComponent<AudioSource>();
        audioSource.Stop();
    }
    private void Start()
    {
        animator.SetBool("leite", true);
        animator.SetBool("LaranjaAtivo", false);
        
        AtualizarArma();
        jaCaiu = false;
        //ResumeFocus();
        controle = GetComponent<CharacterController>();
        if (virtualCamera == null)
        {
            virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }
    }

    private void Update()
    {
        if (control.enabled == true)
        {


            float horizontal = InputMannegerControl.GetMovementInput().x;
            xVelocity = moveSpeed * horizontal * Vector3.right;
            animator.SetFloat("movimento", horizontal);

            yVelocity += gravity * Time.deltaTime * Vector3.down;
            if (controle.isGrounded)
            {
                yVelocity = Vector3.down;
            }

            if (InputMannegerControl.GetJumpInput() && controle.isGrounded)
            {
                yVelocity = jumpSpeed * Vector3.up;
            }
            else if (Input.GetKeyDown(KeyCode.Space) && controle.isGrounded)
            {
                yVelocity = jumpSpeed * Vector3.up;
            }
            finalVelocity = -xVelocity + yVelocity;
            controle.Move(finalVelocity * Time.deltaTime);
           
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                TrocarArma();

            }
            if(horizontal !=0f)
            {
                audioSource.clip = WalkSound;
                audioSource.Play();

            }
            virarPlayerLeite();
            virarPlayerLaranja();
        }

      


    }



    void TrocarArma()
    {
        // Desativa a arma atual.
        armas[armaAtual].SetActive(false);

        // Muda para a próxima arma, ou volta para a primeira se atingir o final do array.
        armaAtual = (armaAtual + 1) % armas.Length;

        // Ativa a nova arma.
        AtualizarArma();
    }
    void AtualizarArma()
    {
        armas[armaAtual].SetActive(true);

        if (armaAtual == 0)
        {
            animator.SetBool("leite", true);
            animator.SetBool("LaranjaAtivo", false);
        }
        if (armaAtual == 1)
        {

            armaLeite.leite.Stop();
            armaLeite.animaçãoLeite();
            animator.SetBool("leite", false);
            animator.SetBool("LaranjaAtivo", true);
        }

    }
    public void ResetForces()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
    }
    //private void FixedUpdate()
    //{
    //    if (control.enabled == false)
    //    {
    //        timeCollider += Time.deltaTime;

    //        Debug.Log("time " + timeCollider);

    //        if (timeCollider >= 0.8)
    //        {
    //            rb.useGravity = true;
    //            colliderPlayer.enabled = true;

    //            Debug.Log("entrou no 1");

    //        }

    //        if (timeCollider >= 2)
    //        {


    //            control.enabled = true;
    //            Debug.Log("entrou no 2");
    //            CameraCair.active = true;
    //            timeCollider = 0;
    //        }
    //        if (timeCollider >= 3)
    //        {


    //            Debug.Log("time " + timeCollider);
    //            timeCollider = 0;
    //            CameraCair.active = true;
    //            Debug.Log("entrou no 3");
    //        }
    //    }

    //    float moveInput = Input.GetAxis("Horizontal");
    //    Move(-moveInput);

    //    if (Input.GetButtonDown("Jump") && IsGrounded())
    //    {
    //        Jump();
    //    }
    //    float xInput = Input.GetAxis("Horizontal");
    //    xVelocity = moveSpeed * xInput * Vector3.right;

    //    yVelocity += gravity * Time.deltaTime * Vector3.down;
    //    if (controle.isGrounded)
    //    {
    //        yVelocity = Vector3.down;
    //    }

    //    if (Input.GetKeyDown(KeyCode.W) && controle.isGrounded)
    //    {
    //        yVelocity = jumpSpeed * Vector3.up;
    //    }
    //    finalVelocity = -xVelocity + yVelocity;
    //    controle.Move(finalVelocity * Time.deltaTime);
    //    Hptx.text = Hp.ToString();
    //}

    //public void StopFocusTemporarily()
    //{
    //    if (virtualCamera != null)
    //    {
    //        virtualCamera.enabled = false; // Desativa o componente CinemachineVirtualCamera
    //        Invoke("ResumeFocus", stopDuration); // Chama o método para retomar o foco após a duração especificada
    //    }
    //}
    public void ResumeFocus()
    {
        if (virtualCamera != null)
        {
            virtualCamera.enabled = true; // Reativa o componente CinemachineVirtualCamera
        }
    }
    void virarPlayerLeite()
    {
        if (armaAtual == 0)
        {
            if (armaLeite.viraDoDireita == true)
            {

                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            if (armaLeite.viraDoEsquerda == true)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }

    }
    void virarPlayerLaranja()
    {
        if (armaAtual == 1)
        {
            if (armaLaranja.viraDoDireita == true)
            {

                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
            if (armaLaranja.viraDoEsquerda == true)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            }
        }
    }
    //private void Move(float inputValue)
    //{
    //    Vector3 movement = new Vector3(inputValue * moveSpeed , 0f, 0f);
    //    rb.MovePosition(transform.position + movement);
    //}

    //private void Jump()
    //{
    //    rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
    //    isJumping = true;
    //}

    //private bool IsGrounded()
    //{
    //    RaycastHit hit;
    //    float raycastDistance = 1f; // Aumentamos a distância do raio para garantir que atinja o chão adequadamente
    //    Vector3 raycastOrigin = transform.position + Vector3.up * 0.1f; // Adicionamos uma pequena elevação ao ponto de origem do raio para evitar colisões imediatas
    //    if (Physics.Raycast(raycastOrigin, Vector3.down, out hit, raycastDistance))
    //    {
    //        isJumping = false;
    //        return true;
    //    }
    //    return false;
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
        if (collision.gameObject.CompareTag("inimigoArC"))
        {
            Hp = Hp - 20;
            audioSource.clip = HurtSound;
            audioSource.Play();
        }
        if (collision.gameObject.CompareTag("inimigoArB"))
        {
            Hp = Hp - 20;
            audioSource.clip = HurtSound;
            audioSource.Play();
        }
        if (collision.gameObject.CompareTag("D"))
        {
            Hp = Hp - 20;
            audioSource.clip = HurtSound;
            audioSource.Play();
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Hambuger"))
        {
            Hp = Hp - 20;
            audioSource.clip = HurtSound;
            audioSource.Play();
        }
        if (other.gameObject.CompareTag("B"))
        {
            Hp = Hp - 20;
            audioSource.clip = HurtSound;
            audioSource.Play();

        }

        if (other.gameObject.CompareTag("inimigoArC"))
        {
            Hp = Hp - 20;
            audioSource.clip = HurtSound;
            audioSource.Play();
        }
        if (other.gameObject.CompareTag("inimigoArB"))
        {
            Hp = Hp - 20;
            audioSource.clip = HurtSound;
            audioSource.Play();
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("CaixaLaranja"))
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                armaLaranja.recarregandoAlaranja = true;
            }
        }
        if (other.gameObject.CompareTag("CaixaLeite"))
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                armaLeite.recarregarLeite = true;
            }
        }
        //if (other.gameObject.CompareTag("Buraco"))
        //{
        //    colliderPlayer.enabled = false;
        //    control.enabled = false;

        ////    CameraCair.active = false;
        //    StopFocusTemporarily();
        //    ResetForces();
        //}
    }

}
