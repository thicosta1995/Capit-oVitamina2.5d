
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MaquinaEstados : MonoBehaviour
{
    public Transform pontoA;
    public Transform pontoB;
    private float distaciaPontoA, distaciaPontoB;
    // Enumeração para representar os diferentes estados

    public enum Estado
    {
        Patrulha,
        Perseguicao,
        Ataque,
        Repouso
    }

    // Variável para armazenar o estado atual
    private Estado estadoAtual;
    private float velocidadePatrulha = 2.0f;
    [SerializeField] float velocidadePersiguição = 3.0f;
    [SerializeField] private bool frenezi;
    [SerializeField] float velocidadePadrão;
    [SerializeField] float timeFrenezi;
    private Rigidbody rb;
    [SerializeField]private bool atacado;
    public bool tomouTiro;
    public Collider visão;

    private float distaciandoPeEsquerdo, distaciandoPeDireito;
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
    public int ValorPontos;


    void Start()
    {
        // Inicializar o estado para Patrulha no início
        tipoInimigo[0].SetActive(true);
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

    }

    void Update()
    {

        if (atacado == true && timeFrenezi <= 8)
        {
            timeFrenezi = timeFrenezi + Time.deltaTime;
            velocidadePatrulha = velocidadePersiguição;
            frenezi = true;
        }

        else if (frenezi == true) 
        {
            velocidadePatrulha = velocidadePadrão;
            atacado = false;
            frenezi = false;
            timeFrenezi = 0;
        }
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
        }
        Debug.Log("valor do enum" + estadoAtual.ToString());
    }

    // Exemplos de métodos de condição
    private bool CondicionalPerseguicao()
    {
        if (vendoPlayer == true && acordado == true & prepararatk == false)
        {
            return true;
        }
        else
            return false;
    }

   
        private bool CondicionalAtaque()
    {
        if (prepararatk == true && atacando == false)
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
        if (vendoPlayer == false && prepararatk == false && atacando == false)
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


        if (vendoPlayer == true)
        {
            distaciandoPeDireito = Vector3.Distance(transform.position, PéDoplayerDireito.position);
            distaciandoPeEsquerdo = Vector3.Distance(transform.position, PéDoplayerEsquerdo.position);

            if (distaciandoPeDireito < distaciandoPeEsquerdo)
            {

                transform.Translate(velocidadePersiguição * Time.deltaTime, 0, 0);
                zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, 90, zoombie.transform.rotation.z);

                if (PéDoplayerDireito.position.x <= transform.position.x)
                {
                    prepararatk = true;
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
        if (vericarDistancia == true)
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



        if (transform.position.x >= pontoB.position.x && PontoB == true && PontoA == false)
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
        else if (transform.position.x <= pontoB.position.x && PontoB == true && PontoA == false)
        {
            zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, 90, zoombie.transform.rotation.z);
            transform.Translate(velocidadePatrulha * Time.deltaTime, 0, 0);
            if (transform.position.x >= pontoB.position.x)
            {
                PontoB = false;
                PontoA = true;
                Debug.Log("entando aqui1");



            }
        }

        if (transform.position.x <= pontoA.position.x && PontoA == true && PontoB == false)
        {

            Debug.Log("entando aqui2");
            zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, 90, zoombie.transform.rotation.z);
            transform.Translate(+velocidadePatrulha * Time.deltaTime, 0, 0);
            if (transform.position.x >= pontoA.position.x)
            {
                PontoB = true;
                PontoA = false;


            }
        }
        else if (transform.position.x >= pontoA.position.x && PontoA == true && PontoB == false)
        {
            Debug.Log("entando aqui2");
            zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, -90, zoombie.transform.rotation.z);
            transform.Translate(-velocidadePatrulha * Time.deltaTime, 0, 0);
            if (transform.position.x >= pontoA.position.x)
            {
                PontoB = true;
                PontoA = false;


            }
        }
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
        }
    }
    void Morte()
    {
        if (VidaInimigo <= 0)
        {
            gameManeger.adicionarPontos(ValorPontos);

            Destroy(gameObject);
        }
    }
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

