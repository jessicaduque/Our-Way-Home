using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Amy : MonoBehaviour
{
    // Posição 
    GerenciadorFase GerenciadorFase;
    Vector3 ondeOlhar;

    // NavMesh
    private Vector3 Destino;
    private NavMeshAgent Agente;

    // Stats 
    public float hp = 10;
    public float mana = 10;
    public float exp = 0;
    public int nivel = 1;
    public bool vivo = true;

    // Animador
    private Animator ControlAnim;

    // Ataques
    public GameObject MeuAtaque;


    void Start()
    {
        // Inicio Posição
        GerenciadorFase = GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>();
        transform.position = GerenciadorFase.PosicaoInicial;

        // NavMesh
        Destino = new Vector3(0, 0, 0);
        Agente = GetComponent<NavMeshAgent>();

        // Animador
        ControlAnim = GetComponent<Animator>();
}

    
    void Update()
    {
        // Mover
        NavMeshMover();
        ControleAnimacaoMover();

        //ControleAtaque();
    }

    void ControleAnimacaoMover()
    {
        if (Agente.velocity.magnitude > 0)
        {
            ControlAnim.SetBool("Move", true);
        }
        else
        {
            ControlAnim.SetBool("Move", false);
        }
    }
    void NavMeshMover()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousepoint = Input.mousePosition;
            Ray pontodesaida = Camera.main.ScreenPointToRay(mousepoint);
            RaycastHit localTocou;
            if (Physics.Raycast(pontodesaida, out localTocou, Mathf.Infinity))
            {
                if (localTocou.collider.gameObject.tag == "Enemy")
                {
                    Agente.stoppingDistance = 0.5f;
                    Destino = localTocou.transform.position;
                }
                else if(localTocou.collider.gameObject.tag == "Obstacule")
                {
                    Agente.stoppingDistance = 0.35f;
                    Destino = localTocou.transform.position;
                }
                else
                {
                    Agente.stoppingDistance = 0.1f;
                    Destino = localTocou.point;
                }

            }
        }

        Agente.SetDestination(Destino);
    }

    /*
    void ControleAtaque()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ControlAnim.SetTrigger("Ataque");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ControlAnim.SetTrigger("Disparo");
        }
    }
    */

    /*
    public void AtivarAtk()
    {
        MeuAtaque.SetActive(true);
    }

    public void DesativarAtk()
    {
        MeuAtaque.SetActive(false);
    }
    */

    
    void AtkDistancia()
    {
        RaycastHit meuAtkD;
        if (Physics.Raycast(MeuAtaque.transform.position, transform.forward, out meuAtkD, 10f))
        {

            if (meuAtkD.collider.gameObject.tag == "Enemy")
            {
                //meuAtkD.collider.gameObject.GetComponent<Enemy>().TomeiDano();
            }

        }
    }
    


    private void OnTriggerEnter(Collider colidiu)
    {
        if (colidiu.gameObject.tag == "EnemyAtk")
        {
            if (vivo == true)
            {
                hp--;
                ControlAnim.SetTrigger("Damage");
                if (hp <= 0)
                {
                    Morrer();
                }
            }

        }
    }


    public void Morrer()
    {
        vivo = false;
        ControlAnim.SetBool("Dead", true);
    }

}