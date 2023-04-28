using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zed : MonoBehaviour
{
    // Posição 
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
    bool levandoDano = false;
    public GameObject PontoSaidaAtk2;
    public GameObject[] PontoSaidaAtk3Lista;
    public GameObject prefabSlash;
    public GameObject EscudoMagia;

    void Start()
    {
        // Stats
        LoadStats();
        stamina = 10;

        // Inicio Posição
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
        levandoDano = false;
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

        //// Ataques
        // Controle de input e níveis permitidos para ataques
        ControleAtaques();

        // Atualizar UI
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasController>().UIZedDados();

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
        if (Input.GetMouseButtonDown(0) && vivo == 1)
        {
            clicou = 1;
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

        // Enquanto o nível não for o nível máximo, o player aumenta de nível ao ter exp suficiente, e o exp necessária para o próximo nível também aumenta
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
        // Se escudo não tiver ativado, outros ataques podem ser ativados
        else
        {
            invulneravel = false;
            ControlAnim.SetBool("Escudo", false);

            if (Input.GetKeyDown(KeyCode.Alpha1) && !estaAtacando)
            {
                Destino = transform.position;
                ControlAnim.SetTrigger("Atk1");
            }

            // A partir do nível 2
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

                // A partir do nível 3
                if (nivel > 2)
                {
                    /**
                    if (Input.GetKeyDown(KeyCode.Alpha3) && !estaAtacando)
                    {
                        // Checa se escudo já está ativado
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
                    
                        // CÓDIGO DE MELHORIA DA ESPADA
                    }

                    // A partir do nível 5
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
        // Se a alteração de hp for negativo (significando que o player levou o ataque, e não uma cura), verifica se o ataque deve ser ou não diminuído pela metade
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
        vivo = PlayerPrefs.GetInt("ZED_VIVO");
    }

    public void SalvarStats()
    {
        PlayerPrefs.SetInt("ZED_NIVEL", nivel);
        PlayerPrefs.SetFloat("ZED_EXP", exp);
        PlayerPrefs.SetFloat("ZED_VIDA", hp);
        PlayerPrefs.SetFloat("ZED_STAMINA", stamina);
        PlayerPrefs.SetInt("ZED_VIVO", vivo);
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

    
    public void Atk2()
    {
        AlteracaoStamina(-2);
        GameObject Slash = Instantiate(prefabSlash, PontoSaidaAtk2.transform.position, Quaternion.identity);
        Slash.transform.rotation = Quaternion.AngleAxis(90, Vector3.down) * transform.rotation;
        Slash.GetComponent<Rigidbody>().AddForce(transform.forward * 140);
        //***Som do disparo de água
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
        //***Som do disparo de água
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
            // Não destruir o ataque do cyclope pois esse não é um instantiate, ele é ligado e desligado
            if (colidiu.gameObject.GetComponent<Ataque>().nome != "MordidaCyclope")
            {
                Destroy(colidiu.gameObject);
            }
        }
    }


    public void MudarPlayerMorte()
    {
        if (PlayerPrefs.GetInt("ZED_VIVO") == 1)
        {
            GerenciadorFase.ZedAtivo();
        }
        else
        {
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasController>().TelaMorte();
        }
    }

    public void Morrer()
    {
        vivo = 0;
        ControlAnim.SetBool("Dead", true);
    }

    public void MetadeAtk(bool metade)
    {
        metadeValorAtaque = metade;
    }
}