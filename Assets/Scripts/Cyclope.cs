using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cyclope : MonoBehaviour
{
    // Animador
    public Animator ControlAnim;

    // Stats
    public float hp = 8;
    float expDada = 5;

    // Ataques
    public GameObject MeuAtaque;
    float tempoDPS = 0.0f;
    int estadoAtaque = 0;
    float tempoCooldownAtaque = 0.0f;

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
            if (Player.GetComponent<Amy>().vivo == 1)
            {
                // Movimentação
                NavMeshMover();

                // Ataques
                ControleAnimacaoAtaque();
                ControleCooldownAtaque();
            }
        }
        else
        {
            if (Player.GetComponent<Zed>().vivo == 1)
            {
                // Movimentação
                NavMeshMover();

                // Ataques
                ControleAnimacaoAtaque();
                ControleCooldownAtaque();
            }
        }
    }

    void NavMeshMover()
    {
        if (estadoAtaque == 0)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            Destino = Player.transform.position;
            // A mudar para o ataque
            Agente.stoppingDistance = 0.2f;
            Agente.SetDestination(Destino);
        }
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
                if (Player.GetComponent<Amy>().vivo == 1)
                {
                    if (estadoAtaque == 0)
                    {
                        ControlAnim.SetTrigger("Attack");
                    }
                }
            }
            else
            {
                if (Player.GetComponent<Zed>().vivo == 1)
                {
                    if (estadoAtaque == 0)
                    {
                        ControlAnim.SetTrigger("Attack");
                    }
                }
            }
        }
    }

    void ControleCooldownAtaque()
    {
        if(estadoAtaque == 2)
        {
            tempoCooldownAtaque += Time.deltaTime;
            if(tempoCooldownAtaque > 1.5)
            {
                tempoCooldownAtaque = 0.0f;
                estadoAtaque = 0;
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
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().InimigoMorreu();
            ControlAnim.SetTrigger("Death");
            Destroy(this.gameObject, 1f);
        }
    }

    public void DarEXP()
    {
        float paraZed;
        float paraAmy;

        if (Player.GetComponent<Amy>())
        {
            paraZed = PlayerPrefs.GetFloat("ZED_EXP") + (expDada / 4);
            paraAmy = PlayerPrefs.GetFloat("AMY_EXP") + ((expDada / 4) * 3);
            Player.GetComponent<Amy>().AlteracaoEXP(paraAmy);
        }
        else
        {
            paraAmy = PlayerPrefs.GetFloat("AMY_EXP") + (expDada / 4);
            paraZed = PlayerPrefs.GetFloat("ZED_EXP") + ((expDada / 4) * 3);
            Player.GetComponent<Zed>().AlteracaoEXP(paraZed);
        }
        PlayerPrefs.SetFloat("ZED_EXP", paraZed);
        PlayerPrefs.SetFloat("AMY_EXP", paraAmy);
    }

    public void AtivarAtk()
    {
        MeuAtaque.SetActive(true);
        estadoAtaque = 1;
    }

    public void DesativarAtk()
    {
        MeuAtaque.SetActive(false);
        estadoAtaque = 2;
    }
    
}