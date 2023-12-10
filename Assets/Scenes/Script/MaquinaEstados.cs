using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class MaquinaEstados : MonoBehaviour
{
    public Transform pontoA;
    public Transform pontoB;
    private float distaciaPontoA, distaciaPontoB;
    // Enumera��o para representar os diferentes estados

    public enum Estado
    {
        Patrulha,
        Perseguicao,
        Ataque,
        Repouso
    }

    // Vari�vel para armazenar o estado atual
    private Estado estadoAtual;
    private float velocidadePatrulha = 2.0f;
    [SerializeField] float velocidadePersigui��o = 3.0f;
    private Rigidbody rb;
    public Transform player;
    public Collider vis�o;

    private float distaciandoPeEsquerdo, distaciandoPeDireito;
    [SerializeField] private bool prepararatk;
    [SerializeField] private GameObject arma;
    [SerializeField] private bool atacando;
    [SerializeField] private float timeAtk,timeAtkInicial ,limiteTempo;
    public float anguloDeVisao = 45.0f;
    [SerializeField] private Transform P�DoplayerDireito, P�DoplayerEsquerdo;
  
    [SerializeField, Range(0.0f, 360.0f)] private float direcaoDoAngulo = 0.0f;

    [SerializeField] GameObject zoombie;
    bool PontoA;
    bool PontoB;
    [SerializeField]public bool vendoPlayer = false;

    void Start()
    {
        // Inicializar o estado para Patrulha no in�cio
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
        // L�gica da m�quina de estado
        switch (estadoAtual)
        {
            case Estado.Patrulha:
                Patrulhar();
                // L�gica de patrulha aqui
                // Transi��o para Persegui��o se uma condi��o for atendida
                if (CondicionalPerseguicao())
                {
                    estadoAtual = Estado.Perseguicao;
                }
                break;

            case Estado.Perseguicao:
                // L�gica de persegui��o aqui
                perseguindoPlayer();
                // Transi��o para Ataque se uma condi��o for atendida
                if (CondicionalAtaque())
                {
                    estadoAtual = Estado.Ataque;
                }
                // Transi��o de volta para Patrulha se necess�rio
                else if (CondicionalVoltarPatrulha())
                {
                    estadoAtual = Estado.Patrulha;
                }
                break;

            case Estado.Ataque:
                ataque();
                // L�gica de ataque aqui
                // Transi��o para Repouso se uma condi��o for atendida
                if (CondicionalRepouso())
                {
                    estadoAtual = Estado.Repouso;
                }
                break;

            case Estado.Repouso:
                // L�gica de repouso aqui
                // Transi��o de volta para Patrulha se necess�rio
                if (CondicionalVoltarPatrulha())
                {
                    estadoAtual = Estado.Patrulha;
                }
                break;
        }
    }

    // Exemplos de m�todos de condi��o
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
        // Implemente a l�gica para verificar se deve passar para o estado de ataque
  
        return false;
    }

    private bool CondicionalRepouso()
    {
        if(atacando == true)
        {
            return true;
        }
        // Implemente a l�gica para verificar se deve passar para o estado de repouso
        return false;
    }

    private bool CondicionalVoltarPatrulha()
    {
        // Implemente a l�gica para verificar se deve voltar para o estado de patrulha
        return false;
    }
    private void perseguindoPlayer()
    {
       

        if (vendoPlayer == true)
        {
            distaciandoPeDireito = Vector3.Distance(transform.position, P�DoplayerDireito.position);
            distaciandoPeEsquerdo = Vector3.Distance(transform.position,P�DoplayerEsquerdo.position);   

            if(distaciandoPeDireito<distaciandoPeEsquerdo)
            {
               
                    transform.Translate(velocidadePersigui��o * Time.deltaTime, 0, 0);
                zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, 90, zoombie.transform.rotation.z);

                if (P�DoplayerDireito.position.x <= transform.position.x)
                {
                    prepararatk = true;
                }


            }
            else
            {
                transform.Translate(-velocidadePersigui��o * Time.deltaTime, 0, 0);
                zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, -90, zoombie.transform.rotation.z);

                if (P�DoplayerEsquerdo.position.x >= transform.position.x)
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

        // Desenha o cone de vis�o no Scene View
        Gizmos.matrix = Matrix4x4.TRS(transform.position, Quaternion.Euler(0, direcaoDoAngulo, 0), Vector3.one);
        Gizmos.DrawFrustum(Vector3.zero, anguloDeVisao * 2, 1.0f, 0.0f, 10.0f); // Ajuste a dist�ncia do cone conforme necess�rio

        // Desenha a esfera de detec��o ao redor do inimigo
        Gizmos.DrawWireSphere(transform.position, 10.0f); // Ajuste o raio conforme necess�rio



    }
    //}
   
    private void OnTriggerExit(Collider other)
    {
       
        if (vis�o.CompareTag("Jogador"))
        {
            vendoPlayer = false; // Desativa a booleana quando o jogador sai da zona de detec��o
            // Pare a persegui��o ou realize outras a��es aqui
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