using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Amy : MonoBehaviour
{
    // Posi��o 
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
    bool metadeValorAtaque = false;


    void Start()
    {
        // Stats
        loadStats();

        // Inicio Posi��o
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
        Debug.Log(metadeValorAtaque);

        //// Mover
        NavMeshMover();
        ControleAnimacaoMover();

        //// Ataques
        // Controle de input e n�veis permitidos para ataques
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
        //***Atualizar barra mana

        // Enquanto o n�vel n�o for o n�vel m�ximo, o player aumenta de n�vel ao ter exp suficiente, e o exp necess�ria para o pr�ximo n�vel tamb�m aumenta
        if (exp >= expParaProxNivel && nivel != nivelMax)
        {
            if(exp > expParaProxNivel)
            {
                exp -= expParaProxNivel;
            }
            else
            {
                exp = 0;
            }
            nivel++;
            expParaProxNivel += 10;
        }
        else if(nivel == nivelMax)
        {
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
        // Se escudo n�o tiver ativado, outros ataques podem ser ativados
        else
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // Controle mana � um pois gasta 2 para a magia, e 3 � ganho. O resultado final assim � 1.
                AlteracaoMana(1);
                AlteracaoVida(3);
                //ControleStaminaZed(3);
                //ControleVidaZed(3);
                Destino = transform.position;
                ControlAnim.SetTrigger("Cura");
            }

            // A partir do n�vel 2
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

                // A partir do n�vel 4
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

                    // A partir do n�vel 5
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
    
    void AlteracaoMana(float alteracaoMana)
    {
        mana += alteracaoMana;
        //***Alterar barra de mana aqui
    }

    void AlteracaoVida(float alteracaoHP)
    {
        hp += alteracaoHP;
        //***Alterar barra de vida aqui
    }

    public void AlteracaoEXP(float alteracaoEXP)
    {
        exp += alteracaoEXP;
        //***Alterar barra de exp aqui
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
        AlteracaoMana(-2);
        GameObject DisparoAgua = Instantiate(DisparoAguaPrefab, PontoDeSaida.transform.position, Quaternion.identity);
        DisparoAgua.GetComponent<Rigidbody>().AddForce(transform.forward * 100);
        //***Som do disparo de �gua
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

    public void MetadeAtk(bool metade)
    {
        metadeValorAtaque = metade;
    }
}