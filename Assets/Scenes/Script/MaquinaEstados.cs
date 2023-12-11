
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MaquinaEstados : MonoBehaviour
{
    public Transform pontoA;
    public Transform pontoB;
    private float distaciaPontoA, distaciaPontoB;
    // EnumeraÁ„o para representar os diferentes estados

    public enum Estado
    {
        Patrulha,
        Perseguicao,
        Ataque,
        Repouso
    }

    // Vari·vel para armazenar o estado atual
    private Estado estadoAtual;
    private float velocidadePatrulha = 2.0f;
    [SerializeField] float velocidadePersiguiÁ„o = 3.0f;
    private Rigidbody rb;
    public Transform player;
    public Collider vis„o;

    private float distaciandoPeEsquerdo, distaciandoPeDireito;
    [SerializeField] private bool prepararatk;
    [SerializeField] private GameObject arma;
    [SerializeField] private bool atacando, acordado, vericarDistancia;
    [SerializeField] private float timeAtk, timeAtkInicial, limiteTempo, tempoDescanco, tempoLimiteDescanco;
    public float anguloDeVisao = 45.0f;
    [SerializeField] private Transform PÈDoplayerDireito, PÈDoplayerEsquerdo;

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
        // Inicializar o estado para Patrulha no inÌcio
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


    }

    void Update()
    {
        

        // LanÁa um raio da posiÁ„o do inimigo em direÁ„o ao jogador
      
        Morte();
        // LÛgica da m·quina de estado
        switch (estadoAtual)
        {
            case Estado.Patrulha:
                Patrulhar();
                // LÛgica de patrulha aqui
                // TransiÁ„o para PerseguiÁ„o se uma condiÁ„o for atendida
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
                // LÛgica de perseguiÁ„o aqui
                perseguindoPlayer();
                // TransiÁ„o para Ataque se uma condiÁ„o for atendida
                if (CondicionalAtaque())
                {
                    estadoAtual = Estado.Ataque;
                }
                // TransiÁ„o de volta para Patrulha se necess·rio
                else if (CondicionalVoltarPatrulha())
                {

                    vericarDistancia = true;
                    estadoAtual = Estado.Patrulha;
                }
                break;

            case Estado.Ataque:
                ataque();
                // LÛgica de ataque aqui
                // TransiÁ„o para Repouso se uma condiÁ„o for atendida
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
                // LÛgica de repouso aqui
                // TransiÁ„o de volta para Patrulha se necess·rio
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

    // Exemplos de mÈtodos de condiÁ„o
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
        // Implemente a lÛgica para verificar se deve passar para o estado de ataque

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
        // Implemente a lÛgica para verificar se deve passar para o estado de repouso
        return false;
    }

    private bool CondicionalVoltarPatrulha()
    {
        if (vendoPlayer == false && prepararatk == false && atacando == false)
        {
            vericarDistancia = true;
            return true;

        }

        // Implemente a lÛgica para verificar se deve voltar para o estado de patrulha
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
            distaciandoPeDireito = Vector3.Distance(transform.position, PÈDoplayerDireito.position);
            distaciandoPeEsquerdo = Vector3.Distance(transform.position, PÈDoplayerEsquerdo.position);

            if (distaciandoPeDireito < distaciandoPeEsquerdo)
            {

                transform.Translate(velocidadePersiguiÁ„o * Time.deltaTime, 0, 0);
                zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, 90, zoombie.transform.rotation.z);

                if (PÈDoplayerDireito.position.x <= transform.position.x)
                {
                    prepararatk = true;
                }


            }
            else
            {
                transform.Translate(-velocidadePersiguiÁ„o * Time.deltaTime, 0, 0);
                zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, -90, zoombie.transform.rotation.z);

                if (PÈDoplayerEsquerdo.position.x >= transform.position.x)
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

        // Desenha o cone de vis„o no Scene View
        Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.Euler(0, direcaoDoAngulo, 0), Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, anguloDeVisao * 2, 1.0f, 0.0f, 10.0f); // Ajuste a dist‚ncia do cone conforme necess·rio

        // Desenha a esfera de detecÁ„o ao redor do inimigo
        Gizmos.DrawWireSphere(transform.position, 10.0f); // Ajuste o raio conforme necess·rio



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


            }

        }
    }
}
    //private bool CondicionalPerseguicao() { return false; }
    //private bool CondicionalAtaque() { return false; }
    //private bool CondicionalRepouso() { return false; }
    //private bool CondicionalVoltarPatrulha() { return false; }

