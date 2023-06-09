using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public AudioSource MusicaFundo;

    // Animador
    public Animator ControlAnim;

    // Stats
    public float hp = 30;
    float expDada = 20;
    bool vivo = true;

    // Ataques
    ///public GameObject MeuAtaque;
    public GameObject PontoSaida;
    public GameObject PrefabPedra;
    float tempoDPS = 0.0f;
    bool atacando;
    float tempoAtacando = 0.0f;

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

        atacando = true;
    }

    private void Update()
    {
        CoolDownAtaque();

        Player = GameObject.FindGameObjectWithTag("Player");
        transform.LookAt(new Vector3(Player.transform.position.x, 0, Player.transform.position.z));

        if (Player.GetComponent<Amy>())
        {
            if (Player.GetComponent<Amy>().vivo == 1)
            {
                // Movimentação
                NavMeshMover();

                // Ataques
                ControleAnimacaoAtaque();
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
            }
        }
    }
    void CoolDownAtaque()
    {
        if (atacando)
        {
            tempoAtacando += Time.deltaTime;
            if(tempoAtacando > 4)
            {
                atacando = false;
                tempoAtacando = 0.0f;
            }
        }
    }

    void NavMeshMover()
    {
        Destino = Player.transform.position;
        // A mudar para o ataque
        Agente.stoppingDistance = 2.2f;
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
                if (Player.GetComponent<Amy>().vivo == 1)
                {
                    if (!atacando)
                    {
                        ControlAnim.SetTrigger("Attack");
                    }
                }
            }
            else
            {
                if (Player.GetComponent<Zed>().vivo == 1)
                {
                    if (!atacando)
                    {
                        ControlAnim.SetTrigger("Attack");
                    }
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

                    if (tempoDPS > 1 && tempoDPS < 2)
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
        ControlAnim.SetTrigger("Damage");

        if (vivo)
        {
            hp -= danoALevar;
        }
        if (hp <= 0)
        {
            vivo = false;
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().InimigoMorreu();
            ControlAnim.SetBool("Death", true);
            Morrer();

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

    public void Morrer()
    {
        MusicaFundo.Stop();
        Destroy(this.gameObject);
    }

    public void Ataque()
    {
        atacando = true;
        PontoSaida.transform.LookAt(Player.transform.position);
        GameObject Pedra = Instantiate(PrefabPedra, PontoSaida.transform.position, Quaternion.identity);
        //***Som 
        //DisparoAguaAudio.Play(0);
    }
}