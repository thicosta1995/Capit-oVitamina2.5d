using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

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
    private Rigidbody rb;
    public Transform player;
    public Collider visão;

    private float distaciandoPeEsquerdo, distaciandoPeDireito;
    [SerializeField] private bool prepararatk;
    [SerializeField] private GameObject arma;
    [SerializeField] private bool atacando;
    [SerializeField] private float timeAtk,timeAtkInicial ,limiteTempo;
    public float anguloDeVisao = 45.0f;
    [SerializeField] private Transform PéDoplayerDireito, PéDoplayerEsquerdo;
  
    [SerializeField, Range(0.0f, 360.0f)] private float direcaoDoAngulo = 0.0f;

    [SerializeField] GameObject zoombie;
    bool PontoA;
    bool PontoB;
    [SerializeField]public bool vendoPlayer = false;

    void Start()
    {
        // Inicializar o estado para Patrulha no início
        estadoAtual = Estado.Patrulha;
        rb = GetComponent<Rigidbody>();
        PontoB = false;
        PontoA = false;
       zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, -90, zoombie.transform.rotation.z);
        atacando = false;
        arma.SetActive(false);
        timeAtk = 0.0f;
    }

    void Update()
    {
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
                break;

            case Estado.Repouso:
                // Lógica de repouso aqui
                // Transição de volta para Patrulha se necessário
                if (CondicionalVoltarPatrulha())
                {
                    estadoAtual = Estado.Patrulha;
                }
                break;
        }
    }

    // Exemplos de métodos de condição
    private bool CondicionalPerseguicao()
    {
        if(vendoPlayer == true)
        {
            return true;
        }
        else
          return false;
    }



    private bool CondicionalAtaque()
    {
        if(prepararatk == true)
        {
            return true;
        }
        // Implemente a lógica para verificar se deve passar para o estado de ataque
  
        return false;
    }

    private bool CondicionalRepouso()
    {
        if(atacando == true)
        {
            return true;
        }
        // Implemente a lógica para verificar se deve passar para o estado de repouso
        return false;
    }

    private bool CondicionalVoltarPatrulha()
    {
        // Implemente a lógica para verificar se deve voltar para o estado de patrulha
        return false;
    }
    private void perseguindoPlayer()
    {
       

        if (vendoPlayer == true)
        {
            distaciandoPeDireito = Vector3.Distance(transform.position, PéDoplayerDireito.position);
            distaciandoPeEsquerdo = Vector3.Distance(transform.position,PéDoplayerEsquerdo.position);   

            if(distaciandoPeDireito<distaciandoPeEsquerdo)
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
        else
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

        distaciaPontoA = Vector3.Distance(transform.position, pontoA.transform.position);
        distaciaPontoB = Vector3.Distance(transform.position, pontoB.transform.position);
        if(distaciaPontoA < distaciaPontoB)
        {

            PontoA = true;
            PontoB = false;
            
        }
        else
        {
            PontoB = true;
            PontoA = false;
        }
        if (transform.position.x>= pontoB.position.x && PontoB== true)
        {
            transform.Translate(-velocidadePatrulha * Time.deltaTime, 0, 0);
            if(transform.position.x <= pontoB.position.x)
            {
                PontoB = false;
                PontoA = true;
                zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, 90, zoombie.transform.rotation.z);
            }
        }
        else if(transform.position.x <= pontoA.position.x &&PontoA == true)
        {

            transform.Translate(+velocidadePatrulha * Time.deltaTime, 0, 0);
            if (transform.position.x >= pontoA.position.x)
            {
                PontoB = true;
                PontoA = false;

                zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, -90, zoombie.transform.rotation.z);
            }
        }
    }
    private void ataque()
    {
       
            timeAtk = timeAtk + Time.deltaTime;
        if(timeAtk>= timeAtkInicial)
        {
            arma.SetActive(true);
           
        }
        if(timeAtk>= limiteTempo) 
        {
            arma.SetActive(false);
            atacando = true;
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
   
    private void OnTriggerExit(Collider other)
    {
       
        if (visão.CompareTag("Jogador"))
        {
            vendoPlayer = false; // Desativa a booleana quando o jogador sai da zona de detecção
            // Pare a perseguição ou realize outras ações aqui
        }
    }
    private void TrocarDestinoPatrulha(Vector3 novoDestino)
    {
        // Muda o destino de patrulha
        //pontoA.position = pontoB.position;
        //pontoB.position = novoDestino;
      
    }
    //private bool CondicionalPerseguicao() { return false; }
    //private bool CondicionalAtaque() { return false; }
    //private bool CondicionalRepouso() { return false; }
    //private bool CondicionalVoltarPatrulha() { return false; }

}