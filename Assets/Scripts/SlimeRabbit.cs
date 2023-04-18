using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeRabbit : MonoBehaviour
{
    // Animador
    public Animator ControlAnim;

    // Stats
    public float hp = 5;
    float expDada = 4;

    // Ataques
    public GameObject MeuAtaque;
    public GameObject EfeitoAtaquePrefab;
    public GameObject PontoDeSaida;
    float tempoDPS = 0.0f;

    // NavMesh
    private Vector3 Destino;
    private GameObject Player;
    private NavMeshAgent Agente;

    private void Start()
    {
        // Animador
        ControlAnim = GetComponent<Animator>();

        // NavMesh
        Agente = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // Movimentação
        NavMeshMover();

        // Ataques
        ControleAnimacaoAtaque();
    }

    void NavMeshMover()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Destino = Player.transform.position;
        Agente.stoppingDistance = 0.8f;
        Agente.SetDestination(Destino);
    }

    void ControleAnimacaoAtaque()
    {
        if (Agente.velocity.magnitude > 0)
        {
            ControlAnim.SetBool("Attacking", false);
        }
        else
        {
            if (Player.GetComponent<Amy>())
            {
                if (Player.GetComponent<Amy>().vivo)
                {
                    ControlAnim.SetBool("Attacking", true);
                }
                else
                {
                    ControlAnim.SetBool("Attacking", false);
                }
            }
            else
            {
                /**
                if (Player.GetComponent<Zed>().vivo)
                {
                    ControlAnim.SetBool("Attacking", true);
                }
                else
                {
                    ControlAnim.SetBool("Attacking", false);
                }**/
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

                    if(tempoDPS > 1 && tempoDPS < 2)
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
                //Player.GetComponent<Zed>().AlteracaoEXP(expDada / 4);
            }
            else
            {
                Player.GetComponent<Amy>().AlteracaoEXP(expDada / 4);
                //Player.GetComponent<Zed>().AlteracaoEXP((expDada / 4) * 3);
            }
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().InimigoMorreu();
            ControlAnim.SetTrigger("Death");
            Destroy(this.gameObject, 1f);
        }
    }

    public void EfeitoAtaque()
    {
        GameObject Efeito = Instantiate(EfeitoAtaquePrefab, PontoDeSaida.transform.position, Quaternion.identity);
        Efeito.GetComponent<randomParticleRotation>().InimigoCriador = this.gameObject;
        //***Som do efeito
        //DisparoAguaAudio.Play(0);
    }

    public void Ataque()
    {
        Instantiate(MeuAtaque, Player.transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity);
    }
}