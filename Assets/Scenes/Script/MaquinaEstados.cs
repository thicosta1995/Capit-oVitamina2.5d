
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using DG.Tweening;
using System.Collections.Generic;

public class MaquinaEstados : MonoBehaviour
{
    public Transform pontoA;
    public Transform pontoB;
    private float distaciaPontoA, distaciaPontoB;

    public bool vision;
    // Enumeração para representar os diferentes estados

    public enum Estado
    {
        Patrulha,
        Perseguicao,
        Ataque,
        Repouso,
        ReceberAtk,
        VoltarnaZona
    }

    // Variável para armazenar o estado atual
   
    private Estado estadoAtual;
    [SerializeField]private Transform[] carros;
    private int contadorCarro;
    private bool carroNafrente;
    private float velocidadePatrulha = 2.0f;
    float timerDano;
    [SerializeField] private Animator animator;
    [SerializeField] private Animation animationAtk,animationIdle,animationWalk;
    [SerializeField] float velocidadePersiguição = 3.0f;
    [SerializeField] private bool frenezi;
    [SerializeField] private Transform playerPos;
    [SerializeField] float velocidadePadrão;
    [SerializeField] float timeFrenezi,timeFreneziMax;
    [SerializeField] Transform limiteA, limiteB;
    [SerializeField] private bool saiuLimite;
    private Rigidbody rb;
    [SerializeField]private bool atacado;
    public bool tomouTiro;
    public Collider visão;

    private float distaciandoPeEsquerdo, distaciandoPeDireito,distanciaCarro1,distanciaCarro2;
    [SerializeField] private bool prepararatk;
    [SerializeField] private GameObject arma;
    [SerializeField] private bool atacando, acordado, vericarDistancia;
    [SerializeField] private float timeAtk, timeAtkInicial, limiteTempo, tempoDescanco, tempoLimiteDescanco;
    public float anguloDeVisao = 45.0f;
    [SerializeField] private Transform PéDoplayerDireito, PéDoplayerEsquerdo;

    [SerializeField, Range(0.0f, 360.0f)] private float direcaoDoAngulo = 0.0f;

    [SerializeField] GameObject zoombie;
    public bool PontoA;
    public bool PontoB;
    [SerializeField] public bool vendoPlayer = false;
    public GameObject[] tipoInimigo;
    [SerializeField] private Slider slide;
    [SerializeField] private float maxHP;
    public BarraHpFlutuante VidaBarra;
    public bool morreu = false;
    public GameManeger gameManeger;
    public float VidaInimigo;
    [SerializeField] private Transform pontoOrigem; // ponto de origem para limitar a perseguição
    [SerializeField] private float limiteDistanciaPerseguicao = 10.0f;
    public int ValorPontos;
    public Transform posPlayerVision;
    public bool tomouDano;
    private float MaxTrowForce = 25;
    public float HistoricalTime = 1f;
    [Range(1, 100)]
    public int HistoricalResolution = 10;
    private Queue<Vector3> HistoricalPositions;
    private float HistoricalPositionInterval;
    private Rigidbody AttackProjectile;
    private float SpherecastRadius = 0.5f;
    private float LastAttackTime;
    [SerializeField]
    private float AttackDelay = 5f;
    public float timeReacao;
    
    void Start()
    {
        // Inicializar o estado para Patrulha no início
        tipoInimigo[0].SetActive(true);

        animator.SetBool("Espera", true);
        animator.SetBool("Ataque", false);
        estadoAtual = Estado.Patrulha;
        rb = GetComponent<Rigidbody>();
        PontoB = false;
        PontoA = false;
        zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, -90, zoombie.transform.rotation.z);
        atacando = false;
        arma.SetActive(false);
        timeAtk = 0.0f;
        acordado = true;
        vericarDistancia = true;
        VidaInimigo = maxHP;
        VidaBarra = GetComponentInChildren<BarraHpFlutuante>();
        VidaBarra.UpDateHealhBar(VidaInimigo, maxHP);
        velocidadePadrão = velocidadePatrulha;
        saiuLimite = false;
        contadorCarro = carros.Length;
        //AttackProjectile.useGravity = false;
        //AttackProjectile.isKinematic = true;
        //SpherecastRadius = AttackProjectile.GetComponent<SphereCollider>().radius;
        LastAttackTime = Random.Range(0, 5);
        int capacity = Mathf.CeilToInt(HistoricalResolution * HistoricalTime);
        //HistoricalPositions = new Queue<Vector3>(capacity);
        //for (int i = 0; i < capacity; i++)
        //{
        //    HistoricalPositions.Enqueue(playerPos.position);
        //}
        HistoricalPositionInterval = HistoricalTime / HistoricalResolution;

    }

    void Update()
    {




        if (contadorCarro > 0)
        {
            distanciaCarro1 = Vector3.Distance(transform.position, carros[0].position);

            if (contadorCarro == 2)
            {
                distanciaCarro2 = Vector3.Distance(transform.position, carros[1].position);
            }
        }
        //if (Time.time > LastAttackTime + AttackDelay
        //   && Physics.SphereCast(
        //       transform.position,
        //       SpherecastRadius,
        //       (playerPos.transform.position + Vector3.up - transform.position).normalized,
        //       out RaycastHit hit,
        //       float.MaxValue,
        //       SightLayers)
        //   && hit.transform == playerPos)
        //{
        //    LastAttackTime = Time.time;
        //    AttackProjectile.transform.SetParent(transform, true);
        //    AttackProjectile.transform.localPosition = new Vector3(0, 0, 1f);
        //    AttackProjectile.useGravity = false;
        //    AttackProjectile.velocity = Vector3.zero;
        //    StartCoroutine(Attack());
        //}
        if (atacado == true && timeFrenezi < timeFreneziMax)
        {
            timeFrenezi = timeFrenezi + Time.deltaTime;
            velocidadePatrulha = velocidadePersiguição;
            frenezi = true;
        }
        else
        {
            velocidadePatrulha = velocidadePadrão;
            atacado = false;
            frenezi = false;
            timeFrenezi = 0;
        }

      
        if(transform.position.x > limiteA.position.x)
        {
            saiuLimite = true;
            Debug.Log("saiuDoA");
        }
        else
          saiuLimite=false;
        if (transform.position.x < limiteB.position.x)
        {
            saiuLimite = true;
            Debug.Log("saiuDoB");
        }
        else
            saiuLimite = false;
        Debug.Log(estadoAtual);
        // Lança um raio da posição do inimigo em direção ao jogador

        Morte();
        // Lógica da máquina de estado
        switch (estadoAtual)
        {
            case Estado.Patrulha:
                Patrulhar();
                // Lógica de patrulha aqui
                // Transição para Perseguição se uma condição for atendida
                if (CondicionalPerseguicao())
                {

                    estadoAtual = Estado.Perseguicao;
                }
                else if (CondicionalAtaque())
                {
                    estadoAtual = Estado.Ataque;
                }
               else if(CondicionalparaSeVirar())
                {
                    estadoAtual = Estado.ReceberAtk;
                }
           
                
                break;

            case Estado.Perseguicao:
                // Lógica de perseguição aqui
                perseguindoPlayer();
                // Transição para Ataque se uma condição for atendida
                if (CondicionalAtaque())
                {
                    estadoAtual = Estado.Ataque;
                }
                // Transição de volta para Patrulha se necessário
                else if (CondicionalVoltarPatrulha())
                {

                    vericarDistancia = true;
                    estadoAtual = Estado.Patrulha;
                }
                break;

            case Estado.Ataque:
                ataque();
                // Lógica de ataque aqui
                // Transição para Repouso se uma condição for atendida
                if (CondicionalRepouso())
                {
                    estadoAtual = Estado.Repouso;
                }
                else if (CondicionalVoltarPatrulha())
                {

                    vericarDistancia = true;
                    estadoAtual = Estado.Patrulha;
                }
                break;

            case Estado.Repouso:
                descanco();
                // Lógica de repouso aqui
                // Transição de volta para Patrulha se necessário
                if (CondicionalVoltarPatrulha())
                {
                    vericarDistancia = true;
                    estadoAtual = Estado.Patrulha;
                }
                else if (CondicionalPerseguicao())
                {
                    estadoAtual = Estado.Perseguicao;
                }
                else if (CondicionalAtaque())
                {
                    estadoAtual = Estado.Ataque;
                }
                break;

            case Estado.ReceberAtk:
                tomarDano();
               
                        if (CondicionalPerseguicao())
                        {
                            estadoAtual = Estado.Perseguicao;
                        }
                       
                        else if (CondicionalVoltarPatrulha())
                        {
                            vericarDistancia = true;
                            estadoAtual = Estado.Patrulha;
                        }
                    
                
                    break;
                
        }
     
        if(vision ==true) 
        {
            posPlayerVision = playerPos;
        }
    }

    // Exemplos de métodos de condição
    private bool CondicionalPerseguicao()
    {
        if (vendoPlayer == true && acordado == true & prepararatk == false && saiuLimite == false )
        {
            return true;
        }
        else
            return false;
    }

   private bool CondicionalparaSeVirar()
    {
        if(vendoPlayer == false && atacado == true && prepararatk == false && atacando ==false && saiuLimite ==false) 
        {
            return true; 
        }
        else
            return false;
    }
        private bool CondicionalAtaque()
    {
        if (prepararatk == true && atacando == false && vendoPlayer ==true && saiuLimite ==false)
        {
            return true;
        }
        // Implemente a lógica para verificar se deve passar para o estado de ataque

        return false;
    }

    private bool CondicionalRepouso()
    {
        if (atacando == true)
        {
            atacando = false;
            acordado = false;
            return true;

        }
        // Implemente a lógica para verificar se deve passar para o estado de repouso
        return false;
    }


    private bool CondicionalVoltarPatrulha()
    {
        if (vendoPlayer == false && prepararatk == false && atacando == false  && atacado == false)
        {
            vericarDistancia = true;
            return true;

        }
        if(carroNafrente == true && prepararatk == false && atacando == false && atacado == false)
        {
            vericarDistancia = true;
            return true;
        }

        // Implemente a lógica para verificar se deve voltar para o estado de patrulha
        return false;
    }
    private bool CondicionalVoltarAcordar()
    {
        if (acordado == true && vendoPlayer != true)
        {
            return true;
        }
        else if (acordado == true && vendoPlayer == true)
        {
            return false;
        }
        return false;
    }
    
    private void descanco()
    {
        tempoDescanco = tempoDescanco + Time.deltaTime;
        if (tempoDescanco >= tempoLimiteDescanco)
        {
            acordado = true;
            tempoDescanco = 0;
        }
    }
    private void perseguindoPlayer()
    {
        float distanciaDoOrigem = Vector3.Distance(transform.position, pontoOrigem.position);


        if (vendoPlayer == true && saiuLimite == false && distanciaDoOrigem <= limiteDistanciaPerseguicao && atacando == false)
        {
            distaciandoPeDireito = Vector3.Distance(transform.position, PéDoplayerDireito.position);
            distaciandoPeEsquerdo = Vector3.Distance(transform.position, PéDoplayerEsquerdo.position);


            if (distaciandoPeDireito < distaciandoPeEsquerdo )
            {
                
                if(contadorCarro>0)
                {
                    if(distanciaCarro1 <distaciandoPeDireito)
                    {
                        carroNafrente = true;
                    }
                  
                    else
                    {
                        carroNafrente = false;
                        transform.Translate(velocidadePersiguição * Time.deltaTime, 0, 0);
                        zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, 90, zoombie.transform.rotation.z);

                        if (PéDoplayerDireito.position.x <= transform.position.x)
                        {
                            prepararatk = true;
                        }
                    }

                }else
                {
                    transform.Translate(velocidadePersiguição * Time.deltaTime, 0, 0);
                    zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, 90, zoombie.transform.rotation.z);

                    if (PéDoplayerDireito.position.x <= transform.position.x)
                    {
                        prepararatk = true;
                    }

                }


            }
            else
            {
                if (contadorCarro > 0)
                {
                    if (distanciaCarro1 < distaciandoPeEsquerdo)
                    {
                        carroNafrente = true;
                    }
                  
                    else
                    {
                        carroNafrente = false;
                        transform.Translate(-velocidadePersiguição * Time.deltaTime, 0, 0);
                        zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, -90, zoombie.transform.rotation.z);

                        if (PéDoplayerEsquerdo.position.x >= transform.position.x)
                        {
                            prepararatk = true;
                        }
                    }

                }
                else
                {
                    transform.Translate(-velocidadePersiguição * Time.deltaTime, 0, 0);
                    zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, -90, zoombie.transform.rotation.z);

                    if (PéDoplayerEsquerdo.position.x >= transform.position.x)
                    {
                        prepararatk = true;
                    }
                }
         


            }


        }
        else
        {
            // Se o inimigo passou do limite, ele interrompe a perseguição e volta à patrulha
            vendoPlayer = false;
            estadoAtual = Estado.Patrulha;
            voltarParaOrigem();
        }

    }
    private void voltarParaOrigem()
    {
        // Move o inimigo de volta ao ponto de origem
        Vector3 direcaoDeVolta = (pontoOrigem.position - transform.position).normalized;
        transform.Translate(direcaoDeVolta * velocidadePatrulha * Time.deltaTime);
        // Rotaciona o inimigo de acordo com a direção que ele está voltando
        if (direcaoDeVolta.x > 0)
        {
            zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, 90, zoombie.transform.rotation.z);
        }
        else
        {
            zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, -90, zoombie.transform.rotation.z);
        }

        // Se o inimigo voltou para o ponto de origem, ele retoma a patrulha
        if (Vector3.Distance(transform.position, pontoOrigem.position) < 0.1f)
        {
            estadoAtual = Estado.Patrulha;
        }
    }
    private void Patrulhar()
    {
        //Vector3 direcao = (pontoB.position - pontoA.position).normalized;
        //rb.velocity = new Vector3(direcao.x * velocidadePatrulha, rb.velocity.y, direcao.z * velocidadePatrulha);

        //// Se estiver indo de A para B e atingir ou ultrapassar o ponto B, troca para o ponto A
        //if (Vector3.Distance(transform.position, pontoA.position) < 0.1f && rb.velocity.x > 0)
        //{
        //    TrocarDestinoPatrulha(pontoB.position);
        //}
        //// Se estiver indo de B para A e atingir ou ultrapassar o ponto A, troca para o ponto B
        //else if (Vector3.Distance(transform.position, pontoB.position) < 0.1f && rb.velocity.x < 0)
        //{
        //    TrocarDestinoPatrulha(pontoA.position);
     
       

        acordado = true;
        if (vericarDistancia == true && acordado ==true && vendoPlayer == false)
        {
            distaciaPontoA = Vector3.Distance(transform.position, pontoA.transform.position);
            distaciaPontoB = Vector3.Distance(transform.position, pontoB.transform.position);
            if (distaciaPontoA < distaciaPontoB)
            {

                PontoA = true;
                PontoB = false;

                Debug.Log("entando aqui");

            }
            else
            {
                PontoB = true;
                PontoA = false;



            }
            vericarDistancia = false;
        }

      


        if (transform.position.x >= pontoB.position.x && PontoB == true && PontoA == false && atacando == false )
        {
            zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, -90, zoombie.transform.rotation.z);
            transform.Translate(-velocidadePatrulha * Time.deltaTime, 0, 0);
            if (transform.position.x <= pontoB.position.x)
            {
                PontoB = false;
                PontoA = true;
                Debug.Log("entando aqui1");

            }

        }
        else if (transform.position.x <= pontoB.position.x && PontoB == true && PontoA == false && atacando == false )
        {
            zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, 90, zoombie.transform.rotation.z);
            transform.Translate(velocidadePatrulha * Time.deltaTime, 0, 0);
            if (transform.position.x >= pontoB.position.x)
            {
                PontoB = false;
                PontoA = true;
              



            }
        }

        if (transform.position.x <= pontoA.position.x && PontoA == true && PontoB == false && atacando == false)
        {

            
            zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, 90, zoombie.transform.rotation.z);
            transform.Translate(+velocidadePatrulha * Time.deltaTime, 0, 0);
            if (transform.position.x >= pontoA.position.x)
            {
                PontoB = true;
                PontoA = false;


            }
        }
        else if (transform.position.x >= pontoA.position.x && PontoA == true && PontoB == false && atacando == false)
        {
           
            zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, -90, zoombie.transform.rotation.z);
            transform.Translate(-velocidadePatrulha * Time.deltaTime, 0, 0);
            if (transform.position.x >= pontoA.position.x)
            {
                PontoB = true;
                PontoA = false;


            }
        }
        animator.SetBool("Espera", false);
        animator.SetBool("Ataque", false);
    }

    private void ataque()
    {
        if (atacando == false)
        {
            timeAtk = timeAtk + Time.deltaTime;
            if (timeAtk >= timeAtkInicial)
            {
                arma.SetActive(true);

            }
            if (timeAtk >= limiteTempo)
            {
                arma.SetActive(false);
                atacando = true;
                prepararatk = false;

                timeAtk = 0;
            }
            animator.SetBool("Espera", false);
            animator.SetBool("Ataque", true);
        }
    }
    void Morte()
    {
        if (VidaInimigo <= 0)
        {
            gameManeger.adicionarPontos();

            Destroy(gameObject);
        }
    }
    void tomarDano()
    {
        if(atacado == true && vendoPlayer == false && prepararatk ==false)
        {
            if(playerPos.position.x <= transform.position.x)
            {
                zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, -90, zoombie.transform.rotation.z);

               
            }
            if(playerPos.position.x >= transform.position.x)
            {
                zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, 90, zoombie.transform.rotation.z);
            }
        }
    }
    // void arremeçarProjetil(Vector3 TargetPosition, Vector3 StartPosition)
    //{
    //    Vector3 displacemente = new Vector3(TargetPosition.x,StartPosition.y,TargetPosition.z) -StartPosition;
    //    float deltaY = TargetPosition.y - StartPosition.y;
    //    float deltaXZ =displacemente.magnitude;
    //    Debug.Log($"Delta:({deltaXZ},{deltaY})");

    //    Debug.Log($"Gravity: {Physics.gravity.y}\r\nDeltaY: { deltaY}\r\nDeltaXZ: {deltaXZ}");
    //    float gravity = Mathf.Abs(Physics.gravity.y);
    //    float trowStrength = Mathf.Clamp(gravity * (deltaY + Mathf.Sqrt(Mathf.Pow(deltaY, 2) + Mathf.Pow(deltaXZ, 2))), 0.01f, MaxTrowForce);



    //}
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        // Desenha o cone de visão no Scene View
        Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.Euler(0, direcaoDoAngulo, 0), Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, anguloDeVisao * 2, 1.0f, 0.0f, 10.0f); // Ajuste a distância do cone conforme necessário

        // Desenha a esfera de detecção ao redor do inimigo
        Gizmos.DrawWireSphere(transform.position, 10.0f); // Ajuste o raio conforme necessário



    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (tipoInimigo[0].name == "inimigoC")
        {
            if (other.tag == "C")
            {
                
                if (VidaInimigo >= 0)
                {
                    VidaInimigo = VidaInimigo - 10;
                    VidaBarra.UpDateHealhBar(VidaInimigo, maxHP);
                    atacado = true;
                }
               
                Destroy(other.gameObject);
                atacado = true;
            }
        }



        if (tipoInimigo[0].name == "Z.A")
        {
            if (other.tag == "B")
            {
                if (VidaInimigo >= 0)
                {
                    VidaInimigo = VidaInimigo - 10;
                    VidaBarra.UpDateHealhBar(VidaInimigo, maxHP);
                }


                Destroy(other.gameObject);
            }
        }
    }
    private void OnParticleCollision(GameObject other)
    {
        if (tipoInimigo[0].name == "inimigoB")
        {
            if (other.gameObject.layer == 7)
            {
                if (VidaInimigo >= 0)
                {
                    VidaInimigo = VidaInimigo - 10;
                    VidaBarra.UpDateHealhBar(VidaInimigo, maxHP);
                }
                
               
                atacado = true;
              
            }

        }
    }
}
    //private bool CondicionalPerseguicao() { return false; }
    //private bool CondicionalAtaque() { return false; }
    //private bool CondicionalRepouso() { return false; }
    //private bool CondicionalVoltarPatrulha() { return false; }

