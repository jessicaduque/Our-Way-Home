using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zed : MonoBehaviour
{
    // Posi��o 
    GerenciadorFase GerenciadorFase;
    Vector3 frente;

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
    public bool vivo = true;

    // Animador
    private Animator ControlAnim;

    // Ataques
    public GameObject MeuAtaque;
    bool estaAtacando = false;
    bool levandoDano = false;
    /**
    public GameObject FogoPrefab;
    public GameObject PontoDeSaidaFogo;
    public GameObject DisparoAguaPrefab;
    public GameObject PontoDeSaidaAgua;
    **/
    public GameObject EscudoMagia;
    bool metadeValorAtaque = false;


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
        //// Mover
        NavMeshMover();
        ControleAnimacaoMover();

        //// Ataques
        // Controle de input e n�veis permitidos para ataques
        ControleAtaques();

        //// Controle Status
        // Salvar stats constantemente
        SalvarStats();
        // Exp
        ControleNivel();

        // Atualizar UI
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasController>().UIZedDados();

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
            if (estaAtacando || levandoDano)
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
        }
        // Se escudo n�o tiver ativado, outros ataques podem ser ativados
        else
        {
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

        if (hp <= 0)
        {
            Morrer();
        }

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
    }

    public void SalvarStats()
    {
        PlayerPrefs.SetInt("ZED_NIVEL", nivel);
        PlayerPrefs.SetFloat("ZED_EXP", exp);
        PlayerPrefs.SetFloat("ZED_VIDA", hp);
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

    public void AtivarAtk()
    {
        MeuAtaque.SetActive(true);
    }

    public void DesativarAtk()
    {
        MeuAtaque.SetActive(false);
    }

    /**
    public void AtkAgua()
    {
        AlteracaoStamina(-2);
        GameObject DisparoAgua = Instantiate(DisparoAguaPrefab, PontoDeSaidaAgua.transform.position, Quaternion.identity);
        DisparoAgua.GetComponent<Rigidbody>().AddForce(transform.forward * 90);
        //***Som do disparo de �gua
        //DisparoAguaAudio.Play(0);
        Destroy(DisparoAgua, 1f);
    }

    public void AtkFogo()
    {
        AlteracaoStamina(-4);
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
    **/

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
            // N�o destruir o ataque do cyclope pois esse n�o � um instantiate, ele � ligado e desligado
            if (colidiu.gameObject.GetComponent<Ataque>().nome != "MordidaCyclope")
            {
                Destroy(colidiu.gameObject);
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