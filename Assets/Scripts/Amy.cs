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
    public float expParaProxNivel = 10;
    public int nivel = 1;
    int nivelMax = 5;
    public bool vivo = true;

    // Animador
    private Animator ControlAnim;

    // Ataques
    //public GameObject MeuAtaque;
    bool escudoMagiaAtivado = false;
    float tempoEscudoMagia = 0.0f;
    public GameObject DisparoAguaPrefab;
    public GameObject PontoDeSaida;


    void Start()
    {
        // Stats
        loadStats();

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
        //// Mover
        NavMeshMover();
        ControleAnimacaoMover();

        //// Ataques
        // Controle de input e níveis permitidos para ataques
        ControleAtaques();
        // Controle do cooldown de escudo quando ativado
        if (escudoMagiaAtivado)
        {
            CoolDownEscudoMagia();
        }

        //// Controle Status
        // Salvar stats constantemente
        SalvarStats();
        // Exp
        ControleNivel();
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
                else if (localTocou.collider.gameObject.tag == "Obstacule")
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

    void ControleNivel()
    {
        // Enquanto o nível não for o nível máximo, o player aumenta de nível ao ter exp suficiente, e o exp necessária para o próximo nível também aumenta
        if(exp >= expParaProxNivel && nivel != nivelMax)
        {
            nivel++;
            nivelMax += 10;
            exp = 0;
        }
    }


    void ControleAtaques()
    {
        // Ativar escudo
        if (Input.GetMouseButtonDown(1))
        {
            ControlAnim.SetTrigger("Escudo");
        }
        // Se escudo não tiver ativado, outros ataques podem ser ativados
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // Controle mana é um pois gasta 2 para a magia, e 3 é ganho. O resultado final assim é 1.
                ControleMana(1);
                ControleMana(3);
                //ControleStaminaZed(3);
                //ControleVidaZed(3);
                Destino = transform.position;
                ControlAnim.SetTrigger("Cura");
            }

            // A partir do nível 2
            if (nivel > 1)
            {
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    if(mana >= 2)
                    {
                        Destino = transform.position;
                        ControlAnim.SetTrigger("AtkAgua");
                    }
                }

                // A partir do nível 4
                if (nivel > 3)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        if (!escudoMagiaAtivado) 
                        {
                            Destino = transform.position;
                            ControlAnim.SetTrigger("Escudo");
                            escudoMagiaAtivado = true;
                            // ***Ativar aqui o gameobject da magia do escudo
                            
                        }
                    }

                    // A partir do nível 5
                    if (nivel > 4)
                    {
                        if (Input.GetKeyDown(KeyCode.Alpha4))
                        {
                            Destino = transform.position;
                            ControlAnim.SetTrigger("AtkFogo");
                        }
                    }
                }
            }
        }
    }
    
    void ControleMana(float alteracaoMana)
    {
        mana += alteracaoMana;
        //***Alterar barra de mana aqui
    }
    
    void loadStats()
    {
        nivel = PlayerPrefs.GetInt("AMY_NIVEL");
        exp = PlayerPrefs.GetFloat("AMY_EXP");
        hp = PlayerPrefs.GetFloat("AMY_VIDA");
        mana = PlayerPrefs.GetFloat("AMY_MANA");
    }

    public void SalvarStats()
    {
        PlayerPrefs.SetInt("AMY_NIVEL", nivel);
        PlayerPrefs.SetFloat("AMY_EXP", exp);
        PlayerPrefs.SetFloat("AMY_VIDA", hp);
        PlayerPrefs.SetFloat("AMY_MANA", mana);
    }

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

    void CoolDownEscudoMagia()
    {
        tempoEscudoMagia += Time.deltaTime;
        if(tempoEscudoMagia > 3)
        {
            // ***Desativar aqui o gameobject da magia do escudo
            tempoEscudoMagia = 0f;
            escudoMagiaAtivado = false;
        }
    }
    public void AtkAgua()
    {
        ControleMana(-2);
        GameObject DisparoAgua = Instantiate(DisparoAguaPrefab, PontoDeSaida.transform.position, Quaternion.identity);
        DisparoAgua.GetComponent<Rigidbody>().AddForce(transform.forward * 100);
        //***Som do disparo de água
        //DisparoAguaAudio.Play(0);
        Destroy(DisparoAgua, 1f);
    }
    
    private void OnTriggerEnter(Collider colidiu)
    {
        if (colidiu.gameObject.tag == "EnemyAtk")
        {
            if (vivo == true)
            {
                float danoALevar = colidiu.gameObject.GetComponent<Ataque>().dano;
                hp -= danoALevar;
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