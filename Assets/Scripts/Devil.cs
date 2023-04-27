using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Devil : MonoBehaviour
{
    // Animador
    public Animator ControlAnim;

    // Stats
    public float hp = 12;
    float expDada = 7;

    // Ataques
    ///public GameObject MeuAtaque;
    public GameObject PontoSaida;
    public GameObject PrefabFogo;
    float tempoDPS = 0.0f;

    // NavMesh
    private Vector3 Destino;
    private GameObject Player;
    private NavMeshAgent Agente;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        // Animador
        ControlAnim = GetComponent<Animator>();

        // NavMesh
        Agente = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        transform.LookAt(Player.transform.position);

        if (Player.GetComponent<Amy>())
        {
            if (Player.GetComponent<Amy>().vivo)
            {
                // Movimentação
                NavMeshMover();

                // Ataques
                ControleAnimacaoAtaque();
            }
        }
        else
        {
            if (Player.GetComponent<Zed>().vivo)
            {
                // Movimentação
                NavMeshMover();

                // Ataques
                ControleAnimacaoAtaque();
            }
        }
    }

    void NavMeshMover()
    {
        Destino = Player.transform.position;
        // A mudar para o ataque
        Agente.stoppingDistance = 1.5f;
        Agente.SetDestination(Destino);
    }

    void ControleAnimacaoAtaque()
    {
        if (Agente.velocity.magnitude > 0)
        {
            ControlAnim.SetBool("Move", true);
        }
        else
        {
            ControlAnim.SetBool("Move", false);

            if (Player.GetComponent<Amy>())
            {
                if (Player.GetComponent<Amy>().vivo)
                {
                    
                    ControlAnim.SetTrigger("Attack");
                    
                }
            }
            else
            {
                if (Player.GetComponent<Zed>().vivo)
                {
                    ControlAnim.SetTrigger("Attack");
                }
            }
        }
    }

    private void OnTriggerEnter(Collider colidiu)
    {
        if (colidiu.gameObject.tag == "Attack")
        {
            if (!colidiu.gameObject.GetComponent<Ataque>().DPS)
            {
                float danoALevar = colidiu.gameObject.GetComponent<Ataque>().dano;
                hp -= danoALevar;
                ControlAnim.SetTrigger("Damage");
                TomeiDano();
                Destroy(colidiu.gameObject);
            }
        }
    }

    private void OnTriggerStay(Collider colidiu)
    {
        if (colidiu.gameObject.tag == "Attack")
        {
            if (colidiu.gameObject.GetComponent<Ataque>().DPS)
            {
                float danoALevar = colidiu.gameObject.GetComponent<Ataque>().dano;

                if (tempoDPS == 0)
                {
                    hp -= danoALevar;
                    ControlAnim.SetTrigger("Damage");
                    TomeiDano();
                    tempoDPS += Time.deltaTime;
                }
                else
                {
                    tempoDPS += Time.deltaTime;

                    if (tempoDPS > 1 && tempoDPS < 2)
                    {
                        hp -= danoALevar;
                        ControlAnim.SetTrigger("Damage");
                        TomeiDano();
                        tempoDPS = 2f;
                    }
                    else if (tempoDPS > 3)
                    {
                        tempoDPS = 0.0f;
                    }
                }
            }
        }
    }

    public void TomeiDano()
    {
        if (hp <= 0)
        {
            if (Player.GetComponent<Amy>())
            {
                Player.GetComponent<Amy>().AlteracaoEXP((expDada / 4) * 3);
                Player.GetComponent<Zed>().AlteracaoEXP(expDada / 4);
            }
            else
            {
                Player.GetComponent<Amy>().AlteracaoEXP(expDada / 4);
                Player.GetComponent<Zed>().AlteracaoEXP((expDada / 4) * 3);
            }
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().InimigoMorreu();
            ControlAnim.SetTrigger("Death");
            Destroy(this.gameObject, 1f);
        }
    }

    public void Ataque()
    {
        GameObject Fogo = Instantiate(PrefabFogo, PontoSaida.transform.position, Quaternion.identity);
        Fogo.GetComponent<Rigidbody>().AddForce(transform.forward * 100);
        //***Som 
        //DisparoAguaAudio.Play(0);
        Destroy(Fogo, 1f);
    }
}