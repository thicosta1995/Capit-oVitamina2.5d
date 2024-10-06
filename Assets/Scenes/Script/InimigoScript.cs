
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using DG.Tweening;
using System.Collections.Generic;
using DG.Tweening.Core.Easing;

public class InimigoScript : MonoBehaviour
{
    public float pontoAX; // Posição X do ponto A
    public float pontoBX; // Posição X do ponto B
    public float velocidadePatrulha = 2f; // Ajuste a velocidade conforme necessário
    private bool indoParaPontoB = true; // Começa indo para o ponto B
    private Animator animator; // O Animator do inimigo

    private void Start()
    {
        animator = GetComponent<Animator>(); // Obtém o Animator do inimigo
    }

    private void Update()
    {
        Patrulhar();
    }

    private void Patrulhar()
    {
        // Determina a posição de destino
        float destinoX = indoParaPontoB ? pontoBX : pontoAX;
        Vector3 destino = new Vector3(destinoX, transform.position.y, transform.position.z);

        // Move em direção ao destino
        Vector3 direcao = (destino - transform.position).normalized;
        transform.Translate(direcao * velocidadePatrulha * Time.deltaTime);

        // Verifica se chegou ao destino
        if (Vector3.Distance(transform.position, destino) < 0.1f)
        {
            // Troca o ponto de destino
            indoParaPontoB = !indoParaPontoB;
            // Atualiza o Animator
            animator.SetBool("Espera", true);
            animator.SetBool("Ataque", false);
        }
        else
        {
            // Atualiza o Animator para patrulhar
            animator.SetBool("Espera", false);
            animator.SetBool("Ataque", false);
        }

        // Atualiza a rotação do inimigo
        if (indoParaPontoB)
        {
            transform.rotation = Quaternion.Euler(0, 90, 0); // Rotação quando vai para o ponto B
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, -90, 0); // Rotação quando vai para o ponto A
        }
    }
}

