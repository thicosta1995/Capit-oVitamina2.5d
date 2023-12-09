using UnityEngine;

public class MaquinaEstados : MonoBehaviour
{
    public Transform pontoA;
    public Transform pontoB; // Enumeração para representar os diferentes estados

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
    private Rigidbody rb;
    [SerializeField] GameObject zoombie;
    bool PontoA;
    bool PontoB;


    void Start()
    {
        // Inicializar o estado para Patrulha no início
        estadoAtual = Estado.Patrulha;
        rb = GetComponent<Rigidbody>();
        PontoB = true;
        PontoA = false;
       zoombie.transform.rotation = Quaternion.Euler(zoombie.transform.rotation.x, -90, zoombie.transform.rotation.z);
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
        // Implemente a lógica para verificar se deve passar para o estado de perseguição
        return false;
    }

    private bool CondicionalAtaque()
    {
        // Implemente a lógica para verificar se deve passar para o estado de ataque
        return false;
    }

    private bool CondicionalRepouso()
    {
        // Implemente a lógica para verificar se deve passar para o estado de repouso
        return false;
    }

    private bool CondicionalVoltarPatrulha()
    {
        // Implemente a lógica para verificar se deve voltar para o estado de patrulha
        return false;
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

        if(transform.position.x>= pontoB.position.x && PontoB== true)
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
        //}
    
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