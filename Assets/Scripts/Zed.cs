using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zed : MonoBehaviour
{
    // Posi��o 
    GerenciadorFase GerenciadorFase;
    Vector3 frente;
    int clicou = 0;

    // NavMesh
    public Vector3 Destino;
    private NavMeshAgent Agente;

    // Stats 
    public float hp;
    public float stamina = 10;
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
    public GameObject MeuAtaque;
    bool estaAtacando = false;
    //bool levandoDano = false;
    public GameObject PontoSaidaAtk2;
    public GameObject[] PontoSaidaAtk3Lista;
    public GameObject prefabSlash;
    public GameObject EscudoMagia;

    // Inimigos
    private Transform nearestEnemy;

    void Start()
    {
        // Stats
        LoadStats();
        stamina = 10;

        // Inicio Posi��o
        GerenciadorFase = GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>();
        if(PlayerPrefs.GetInt("PERSONAGEM_ATIVO") == 0)
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
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasController>().UIZedDados();

        vivo = PlayerPrefs.GetInt("ZED_VIVO");
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
        if(vivo == 1)
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
            if (estaAtacando)
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

        // Enquanto o n�vel n�o for o n�vel m�ximo, o player aumenta de n�vel ao ter exp suficiente, e o exp necess�ria para o pr�ximo n�vel tamb�m aumenta
        if (exp >= expParaProxNivel && nivel != nivelMax)
        {
            if (exp > expParaProxNivel)
            {
                exp -= expParaProxNivel;
            }
            else
            {
                exp = 0;
            }
            nivel++;
        }
        else if (nivel == nivelMax)
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
            invulneravel = false;
            ControlAnim.SetBool("Escudo", false);

            if (Input.GetKeyDown(KeyCode.Alpha1) && !estaAtacando)
            {
                Destino = transform.position;
                ControlAnim.SetTrigger("Atk1");
            }

            // A partir do n�vel 2
            if (nivel > 1)
            {
                if (Input.GetKeyDown(KeyCode.Alpha2) && !estaAtacando)
                {
                    if (stamina >= 2)
                    {
                        Destino = transform.position;
                        ControlAnim.SetTrigger("Atk2");
                    }
                }

                // A partir do n�vel 3
                if (nivel > 2)
                {
                    /**
                    if (Input.GetKeyDown(KeyCode.Alpha3) && !estaAtacando)
                    {
                        // Checa se escudo j� est� ativado
                        if (!metadeValorAtaque)
                        {
                            if (stamina >= 3)
                            {
                                Destino = transform.position;
                                ControlAnim.SetTrigger("EscudoMagia");
                                metadeValorAtaque = true;
                                // ***Ativar aqui o gameobject da magia do escudo
                            }
                        }**/ 
                    
                        // C�DIGO DE MELHORIA DA ESPADA
                    }

                    // A partir do n�vel 5
                    if (nivel > 4)
                    {
                        if (Input.GetKeyDown(KeyCode.Alpha3) && !estaAtacando)
                        {
                            if (stamina >= 3)
                            {
                                ControlAnim.SetTrigger("Atk3");
                            }
                        }
                    }
                //}
            }
        }
    }

    public void AlteracaoStamina(float alteracaoStamina)
    {
        stamina += alteracaoStamina;

        if (stamina > 10)
        {
            stamina = 10;
        }
    }

    public void AlteracaoVida(float alteracaoHP)
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

        if (hp > 10)
        {
            hp = 10;
        }

        PlayerPrefs.SetFloat("ZED_VIDA", hp);

    }

    public void AlteracaoEXP(float alteracaoEXP)
    {
        exp += alteracaoEXP;
        //***Alterar barra de exp aqui
    }

    void LoadStats()
    {
        nivel = PlayerPrefs.GetInt("ZED_NIVEL");
        exp = PlayerPrefs.GetFloat("ZED_EXP");
        hp = PlayerPrefs.GetFloat("ZED_VIDA");
        stamina = PlayerPrefs.GetFloat("ZED_STAMINA");
        vivo = PlayerPrefs.GetInt("ZED_VIVO");

        //levandoDano = false;
        estaAtacando = false;
    }

    public void SalvarStats()
    {
        PlayerPrefs.SetInt("ZED_NIVEL", nivel);
        PlayerPrefs.SetFloat("ZED_STAMINA", stamina);
    }

    public void EstaAtacando(int atacando)
    {
        if (atacando == 0)
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

    public void AtivarAtk()
    {
        MeuAtaque.SetActive(true);
    }

    public void DesativarAtk()
    {
        MeuAtaque.SetActive(false);
    }

    
    public void Atk2()
    {
        AlteracaoStamina(-2);
        GameObject Slash = Instantiate(prefabSlash, PontoSaidaAtk2.transform.position, Quaternion.identity);
        Slash.transform.rotation = Quaternion.AngleAxis(90, Vector3.down) * transform.rotation;
        Slash.GetComponent<Rigidbody>().AddForce(transform.forward * 140);
        //***Som do disparo de �gua
        //DisparoAguaAudio.Play(0);
    }

    public void Atk3()
    {
        AlteracaoStamina(-4);
        for (int i = 0; i < PontoSaidaAtk3Lista.Length; i++)
        {
            GameObject Slash = Instantiate(prefabSlash, PontoSaidaAtk3Lista[i].transform.position, Quaternion.identity);
            Slash.transform.rotation = Quaternion.AngleAxis(90, Vector3.down) * PontoSaidaAtk3Lista[i].transform.rotation;
            Slash.GetComponent<Rigidbody>().AddForce(PontoSaidaAtk3Lista[i].transform.forward * 140);
        }
        //***Som do disparo de �gua
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
            if (colidiu.gameObject.GetComponent<Ataque>().nome != "MordidaCyclope")
            {
                Destroy(colidiu.gameObject);
            }
        }
    }


    public void MudarPlayerMorte()
    {
        if (PlayerPrefs.GetInt("AMY_VIVO") == 1)
        {
            PlayerPrefs.SetInt("ZED_VIVO", 0);
            GerenciadorFase.AmyAtivo();
        }
        else
        {
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasController>().TelaMorte();
        }
    }

    public void Morrer()
    {
        vivo = 0;
        PlayerPrefs.SetInt("ZED_VIVO", 0);
        ControlAnim.SetBool("Dead", true);
    }

    public void MetadeAtk(bool metade)
    {
        metadeValorAtaque = metade;
    }
}