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
    public float hp;
    public float mana;
    public float exp;
    public float expParaProxNivel = 10;
    public int nivel;
    int nivelMax = 5;
    public bool vivo = true;

    // Animador
    private Animator ControlAnim;

    // Ataques
    //public GameObject MeuAtaque;
    bool estaAtacando = false;
    bool levandoDano = false;
    public GameObject FogoPrefab;
    public GameObject PontoDeSaidaFogo;
    public GameObject DisparoAguaPrefab;
    public GameObject PontoDeSaidaAgua;
    public GameObject EscudoMagia;
    bool metadeValorAtaque = false;


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

        
        if (GameObject.FindGameObjectWithTag("Enemy")) 
        {
            GameObject Enemy = GameObject.FindGameObjectWithTag("Enemy");
            if (Vector3.Distance(Enemy.transform.position, transform.position) < 1.3f)
            {
                transform.LookAt(Enemy.transform.position);
            }
            
        } 
    }
    void NavMeshMover()
    {
        if (Input.GetMouseButtonDown(0) && vivo)
        {
            if(estaAtacando || levandoDano)
            {
                Destino = transform.position;
            }
            else
            {
                Vector3 mousepoint = Input.mousePosition;
                Ray pontodesaida = Camera.main.ScreenPointToRay(mousepoint);
                RaycastHit localTocou;
                if (Physics.Raycast(pontodesaida, out localTocou, Mathf.Infinity))
                {
                    if (localTocou.collider.gameObject.tag == "Enemy")
                    {
                        Agente.stoppingDistance = 0.1f;
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
        }

        Agente.SetDestination(Destino);
    }

    void ControleNivel()
    {

        expParaProxNivel = nivel * 10;

        //***Atualizar barra  nivel

        // Enquanto o nível não for o nível máximo, o player aumenta de nível ao ter exp suficiente, e o exp necessária para o próximo nível também aumenta
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
        }
        else if(nivel == nivelMax)
        {
            exp = 0;
        }
    }

    void ControleAtaques()
    {
        // Ativar escudo
        if (Input.GetMouseButton(1))
        {
            Destino = transform.position;
            ControlAnim.SetBool("Escudo", true);
        }
        // Se escudo não tiver ativado, outros ataques podem ser ativados
        else
        {
            ControlAnim.SetBool("Escudo", false);

            if (Input.GetKeyDown(KeyCode.Alpha1) && !estaAtacando)
            {
                // Controle mana é um pois gasta 2 para a magia, e 3 é ganho. O resultado final assim é 1.
                if(mana >= 2 && (hp < 10 || mana < 10))
                {
                    //ControleStaminaZed(3);
                    //ControleVidaZed(3);
                    Destino = transform.position;
                    ControlAnim.SetTrigger("Cura");
                }
            }

            // A partir do nível 2
            if (nivel > 1)
            {
                if (Input.GetKeyDown(KeyCode.Alpha2) && !estaAtacando)
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
                    if (Input.GetKeyDown(KeyCode.Alpha3) && !estaAtacando)
                    {
                        // Checa se escudo já está ativado
                        if (!metadeValorAtaque) 
                        {
                            if (mana >= 3)
                            {
                                Destino = transform.position;
                                ControlAnim.SetTrigger("EscudoMagia");
                                metadeValorAtaque = true;
                                // ***Ativar aqui o gameobject da magia do escudo
                            }
                        }
                    }

                    // A partir do nível 5
                    if (nivel > 4)
                    {
                        if (Input.GetKeyDown(KeyCode.Alpha4) && !estaAtacando)
                        {
                            if(mana >= 4)
                            {
                                ControlAnim.SetTrigger("AtkFogo");
                            }
                        }
                    }
                }
            }
        }
    }
    
    void AlteracaoMana(float alteracaoMana)
    {
        mana += alteracaoMana;

        if(mana > 10)
        {
            mana = 10;
        }
        //***Alterar barra de mana aqui
    }

    void AlteracaoVida(float alteracaoHP)
    {


        // Se a alteração de hp for negativo (significando que o player levou o ataque, e não uma cura), verifica se o ataque deve ser ou não diminuído pela metade
        if (metadeValorAtaque && alteracaoHP < 0)
        {
            hp += (alteracaoHP / 2f);
        }
        else
        {
            hp += alteracaoHP;

        }

        if(hp > 10)
        {
            hp = 10;
        }

        if(hp <= 0)
        {
            Morrer();
        }

        //***Alterar barra de vida aqui

    }

    public void AlteracaoEXP(float alteracaoEXP)
    {
        exp += alteracaoEXP;
        //***Alterar barra de exp aqui
    }

    public void AtivarEscudoMagia()
    {
        EscudoMagia.gameObject.SetActive(true);
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

    public void EstaAtacando(int atacando)
    {
        if(atacando == 0)
        {
            estaAtacando = true;
        }
        else
        {
            estaAtacando = false;
        }
    }

    public void LevandoDano(int dano)
    {
        if (dano == 0)
        {
            levandoDano = true;
        }
        else
        {
            levandoDano = false;
        }
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
    public void AtkAgua()
    {
        AlteracaoMana(-2);
        GameObject DisparoAgua = Instantiate(DisparoAguaPrefab, PontoDeSaidaAgua.transform.position, Quaternion.identity);
        DisparoAgua.GetComponent<Rigidbody>().AddForce(transform.forward * 90);
        //***Som do disparo de água
        //DisparoAguaAudio.Play(0);
        Destroy(DisparoAgua, 1f);
    }

    public void AtkFogo()
    {
        AlteracaoMana(-4);
        if (GameObject.FindGameObjectWithTag("Enemy"))
        {
            GameObject Enemy = GameObject.FindGameObjectWithTag("Enemy");
            Instantiate(FogoPrefab, Enemy.transform.position, Quaternion.identity);
        }
        else
        {
            Instantiate(FogoPrefab, PontoDeSaidaFogo.transform.position, Quaternion.identity);
        }
        
        //***Som do fogo
        //DisparoAguaAudio.Play(0);
    }

    private void OnTriggerEnter(Collider colidiu)
    {
        if (colidiu.gameObject.tag == "EnemyAttack")
        {
            if (vivo == true)
            {
                float danoALevar = colidiu.gameObject.GetComponent<Ataque>().dano;
                AlteracaoVida(-danoALevar);
                ControlAnim.SetTrigger("Damage");
                if (hp <= 0)
                {
                    Morrer();
                }
            }
            Destroy(colidiu.gameObject);

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