using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Amy : MonoBehaviour
{
    // Posi��o 
    GerenciadorFase GerenciadorFase;
    Vector3 frente;
    int clicou;

    // NavMesh
    public Vector3 Destino;
    private NavMeshAgent Agente;

    // Stats 
    public float hp;
    public float mana = 10;
    public float exp;
    public float expParaProxNivel = 10;
    public int nivel;
    int nivelMax = 5;
    public int vivo = 1;
    bool invulneravel = false;
    bool metadeValorAtaque = false;

    // Animador
    private Animator ControlAnim;

    // Ataques
    //public GameObject MeuAtaque;
    bool estaAtacando = false;
    //bool levandoDano = false;
    public GameObject FogoPrefab;
    public GameObject PontoDeSaidaFogo;
    public GameObject DisparoAguaPrefab;
    public GameObject PontoDeSaidaAgua;
    public GameObject EscudoMagia;

    // Inimigos
    private Transform nearestEnemy;

    void Start()
    {
        // Stats
        LoadStats();
        mana = 10;


        // Inicio Posi��o
        GerenciadorFase = GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>();
        if (PlayerPrefs.GetInt("PERSONAGEM_ATIVO") == 1)
        {
            transform.position = GerenciadorFase.PosicaoInicial;
            frente = transform.position + GerenciadorFase.frenteInicial;
            transform.LookAt(frente);

            // Nav Mesh
            Destino = GerenciadorFase.PosicaoInicial;
        }

        // NavMesh
        Agente = GetComponent<NavMeshAgent>();

        // Animador
        ControlAnim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        LoadStats();
    }

    void Update()
    {
        if (clicou == 0)
        {
            frente = transform.position + GerenciadorFase.frenteInicial;
            transform.LookAt(frente);
        }

        //// Mover
        NavMeshMover();
        ControleAnimacaoMover();

        // Olhar inimigos
        InimigoMaisPerto();

        //// Ataques
        // Controle de input e n�veis permitidos para ataques
        ControleAtaques();

        // Atualizar UI
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasController>().UIAmyDados();

        vivo = PlayerPrefs.GetInt("AMY_VIVO");
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

    void InimigoMaisPerto()
    {
        if (vivo == 1)
        {
            GameObject[] Inimigos;
            Inimigos = GameObject.FindGameObjectsWithTag("Enemy");

            float minimumDistance = Mathf.Infinity;

            nearestEnemy = null;

            foreach (GameObject enemy in Inimigos)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < minimumDistance)
                {
                    minimumDistance = distance;
                    nearestEnemy = enemy.transform;
                }
            }
            if (nearestEnemy != null)
            {
                if (Vector3.Distance(transform.position, nearestEnemy.transform.position) < 1.3f)
                {
                    transform.LookAt(nearestEnemy.transform.position);
                }
            }
        }
    }

    void NavMeshMover()
    {
        if (Input.GetMouseButtonDown(0) && vivo == 1)
        {
            clicou = 1;
            if(estaAtacando)
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
                        if (localTocou.collider.gameObject.GetComponent<Boss>())
                        {
                            Agente.stoppingDistance = 0.4f;
                        }
                        else
                        {
                            Agente.stoppingDistance = 0.1f;
                        }
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
            invulneravel = true;
        }
        // Se escudo n�o tiver ativado, outros ataques podem ser ativados
        else
        {
            ControlAnim.SetBool("Escudo", false);
            invulneravel = false;

            if (Input.GetKeyDown(KeyCode.Alpha1) && !estaAtacando)
            {
                // Controle mana � um pois gasta 2 para a magia, e 3 � ganho. O resultado final assim � 1.
                if (mana >= 2 && ((hp < 10 || mana < 10) || (PlayerPrefs.GetFloat("ZED_VIDA") < 10 || PlayerPrefs.GetFloat("ZED_STAMINA") < 10)))
                {
                    if(PlayerPrefs.GetFloat("ZED_VIDA") >= 7)
                    {
                        PlayerPrefs.SetFloat("ZED_VIDA", 10);
                    }
                    else
                    {
                        PlayerPrefs.SetFloat("ZED_VIDA", (PlayerPrefs.GetFloat("ZED_VIDA") + 3));
                    }

                    if (PlayerPrefs.GetFloat("ZED_STAMINA") >= 7)
                    {
                        PlayerPrefs.SetFloat("ZED_STAMINA", 10);
                    }
                    else
                    {
                        PlayerPrefs.SetFloat("ZED_STAMINA", (PlayerPrefs.GetFloat("ZED_STAMINA") + 3));
                    }

                    Destino = transform.position;
                    ControlAnim.SetTrigger("Cura");
                }
            }

            // A partir do n�vel 2
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

                // A partir do n�vel 4
                if (nivel > 3)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha3) && !estaAtacando)
                    {
                        // Checa se escudo j� est� ativado
                        if (!metadeValorAtaque) 
                        {
                            if (mana >= 3)
                            {
                                Destino = transform.position;
                                ControlAnim.SetTrigger("EscudoMagia");
                                metadeValorAtaque = true;
                            }
                        }
                    }

                    // A partir do n�vel 5
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

        // Se a altera��o de hp for negativo (significando que o player levou o ataque, e n�o uma cura), verifica se o ataque deve ser ou n�o diminu�do pela metade
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
        PlayerPrefs.SetFloat("AMY_VIDA", hp);


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

    void LoadStats()
    {
        nivel = PlayerPrefs.GetInt("AMY_NIVEL");
        exp = PlayerPrefs.GetFloat("AMY_EXP");
        hp = PlayerPrefs.GetFloat("AMY_VIDA");
        vivo = PlayerPrefs.GetInt("AMY_VIVO");

        //levandoDano = false;
        estaAtacando = false;
    }

    public void SalvarStats()
    {
        PlayerPrefs.SetInt("AMY_NIVEL", nivel);
        PlayerPrefs.SetInt("AMY_VIVO", vivo);
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

    /*
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
    */

    public void AtkAgua()
    {
        AlteracaoMana(-2);
        GameObject DisparoAgua = Instantiate(DisparoAguaPrefab, PontoDeSaidaAgua.transform.position, Quaternion.identity);
        DisparoAgua.GetComponent<Rigidbody>().AddForce(transform.forward * 90);
        //***Som do disparo de �gua
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
            if (vivo == 1 && !invulneravel)
            {
                float danoALevar = colidiu.gameObject.GetComponent<Ataque>().dano;
                AlteracaoVida(-danoALevar);
                ControlAnim.SetTrigger("Damage");
                if (hp <= 0)
                {
                    Morrer();
                }
            }
            // N�o destruir o ataque do cyclope pois esse n�o � um instantiate, ele � ligado e desligado
            if(colidiu.gameObject.GetComponent<Ataque>().nome != "MordidaCyclope")
            {
                Destroy(colidiu.gameObject);
            }
        }
    }

    public void Morrer()
    {
        vivo = 0;
        ControlAnim.SetBool("Dead", true);
    }

    public void MudarPlayerMorte()
    {
        if(PlayerPrefs.GetInt("ZED_VIVO") == 1)
        {
            PlayerPrefs.SetInt("AMY_VIVO", 0);
            GerenciadorFase.ZedAtivo();
        }
        else
        {
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasController>().TelaMorte();
        }
    }

    public void MetadeAtk(bool metade)
    {
        metadeValorAtaque = metade;
    }
}