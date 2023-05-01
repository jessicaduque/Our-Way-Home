using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeRabbit : MonoBehaviour
{
    // Animador
    public Animator ControlAnim;

    // Stats
    public float hp = 4;
    float expDada = 4;
    bool vivo = true;

    // Ataques
    public GameObject MeuAtaque;
    public GameObject EfeitoAtaquePrefab;
    public GameObject PontoDeSaida;
    float tempoDPS = 0.0f;
    bool atacando;
    float tempoAtacando = 0.0f;

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
        Player = GameObject.FindGameObjectWithTag("Player");
        if (vivo)
        {
            transform.LookAt(Player.transform.position);
            // Movimentação
            NavMeshMover();

            // Ataques
            ControleAnimacaoAtaque();
            CoolDownAtaque();
        }
    }

    void NavMeshMover()
    {
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
                if (Player.GetComponent<Amy>().vivo == 1)
                {
                    if (!atacando)
                    {
                        ControlAnim.SetBool("Attacking", true);
                    }
                }
                else
                {
                    ControlAnim.SetBool("Attacking", false);
                }
            }
            else
            {
                if (Player.GetComponent<Zed>().vivo == 1)
                {
                    if (!atacando)
                    {
                        ControlAnim.SetBool("Attacking", true);
                    }
                }
                else
                {
                    ControlAnim.SetBool("Attacking", false);
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
                TomeiDano(danoALevar);
                if (colidiu.gameObject.GetComponent<Ataque>().nome == "AtkAgua")
                {
                    Destroy(colidiu.gameObject);
                }

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
                    TomeiDano(danoALevar);
                    tempoDPS += Time.deltaTime;
                }
                else
                {
                    tempoDPS += Time.deltaTime;

                    if(tempoDPS > 1 && tempoDPS < 2)
                    {
                        TomeiDano(danoALevar);
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

    public void TomeiDano(float danoALevar)
    {
        if (vivo)
        {
            hp -= danoALevar;
            ControlAnim.SetTrigger("Damage");
            
        }
        if (hp <= 0)
        {
            vivo = false;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().InimigoMorreu();
            ControlAnim.SetBool("Death", true);
            
        }
    }
    public void DarEXP()
    {
        float paraZed;
        float paraAmy;

        if (Player.GetComponent<Amy>())
        {
            paraZed = PlayerPrefs.GetFloat("ZED_EXP") + ((expDada / 8) * 3);
            paraAmy = PlayerPrefs.GetFloat("AMY_EXP") + ((expDada / 8) * 5);
            Player.GetComponent<Amy>().AlteracaoEXP(paraAmy);
        }
        else
        {
            paraAmy = PlayerPrefs.GetFloat("AMY_EXP") + ((expDada / 8) * 3);
            paraZed = PlayerPrefs.GetFloat("ZED_EXP") + ((expDada / 8) * 5);
            Player.GetComponent<Zed>().AlteracaoEXP(paraZed);
        }

        PlayerPrefs.SetFloat("ZED_EXP", paraZed);
        PlayerPrefs.SetFloat("AMY_EXP", paraAmy);
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
        atacando = true;
        Instantiate(MeuAtaque, Player.transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity);
    }

    void CoolDownAtaque()
    {
        if (atacando)
        {
            tempoAtacando += Time.deltaTime;
            if(tempoAtacando > 0.6f)
            {
                ControlAnim.SetBool("Attacking", false);
            }
            if (tempoAtacando > 5)
            {
                atacando = false;
                tempoAtacando = 0.0f;
            }
        }
    }

    public void Morrer()
    {
        Destroy(this.gameObject);
    }
}