using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorFase : MonoBehaviour
{
    // Variáveis para início de fase
    public Vector3 PosicaoInicial;
    public Vector3 frenteInicial;
    public GameObject Amy;
    public GameObject Zed;
    public GameObject PersonagemAtivo;
    int intPersonagemAtivo;
    public int quantInimigosFase;
    public bool faseCompleta = false;
    public int faseAtual;

    private void Start()
    {
        Time.timeScale = 1;

        quantInimigosFase = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (PlayerPrefs.GetInt("PERSONAGEM_ATIVO") == 0)
        {
            ZedAtivo();
        }
        else
        {
            AmyAtivo();
        }
    }

    private void Update()
    {
        // Controle personagem ativo
        ControlePersonagemAtivo();

        // InimigosVivos
        ControleAvancarFase();
    }

    void ControleAvancarFase()
    {
        if(quantInimigosFase <= 0)
        {
            faseCompleta = true;
        }
    }

    public void InimigoMorreu()
    {
        quantInimigosFase--;
    }

    void ControlePersonagemAtivo()
    {
        if (Input.GetKeyDown("space"))
        {
            if (PersonagemAtivo == Amy && PlayerPrefs.GetInt("ZED_VIVO") == 1)
            {
                ZedAtivo();
            }
            else if(PersonagemAtivo == Zed && PlayerPrefs.GetInt("AMY_VIVO") == 1)
            {
                AmyAtivo();
            }
        }
    }

    public void ZedAtivo()
    {
        intPersonagemAtivo = 0;
        Zed.transform.position = Amy.transform.position;
        Zed.GetComponent<Zed>().Destino = Amy.GetComponent<Amy>().Destino;
        Zed.transform.LookAt(Amy.GetComponent<Amy>().Destino);
        PersonagemAtivo = Zed;
        Amy.GetComponent<Amy>().MetadeAtk(false);
        Amy.SetActive(false);
        Zed.SetActive(true);
    }

    public void AmyAtivo()
    {
        intPersonagemAtivo = 1;
        Amy.transform.position = Zed.transform.position;
        Amy.GetComponent<Amy>().Destino = Zed.GetComponent<Zed>().Destino;
        Amy.transform.LookAt(Zed.GetComponent<Zed>().Destino);
        PersonagemAtivo = Amy;
        Zed.GetComponent<Zed>().MetadeAtk(false);
        Zed.SetActive(false);
        Amy.SetActive(true);
    }

    public void LoadFase(int faseParaIr)
    {
        PlayerPrefs.SetInt("PERSONAGEM_ATIVO", intPersonagemAtivo);
        SceneManager.LoadScene(faseParaIr);
    }
}
