using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorFase : MonoBehaviour
{
    // Vari�veis para in�cio de fase
    public Vector3 PosicaoInicial;
    public GameObject Amy;
    public GameObject Zed;
    public GameObject PersonagemAtivo;
    int intPersonagemAtivo;

    private void Start()
    {
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

        // Controle UI
        ControleAtaquesUI();
    }

    void ControlePersonagemAtivo()
    {
        if (Input.GetKeyDown("space"))
        {
            if (PersonagemAtivo == Amy)
            {
                ZedAtivo();
            }
            else
            {
                AmyAtivo();
            }
        }
    }

    void ZedAtivo()
    {
        Zed.transform.position = Amy.transform.position;
        PersonagemAtivo = Zed;
        intPersonagemAtivo = 0;
        Zed.SetActive(true);
        Amy.SetActive(false);
    }

    void AmyAtivo()
    {
        Amy.transform.position = Zed.transform.position;
        PersonagemAtivo = Amy;
        intPersonagemAtivo = 1;
        Zed.SetActive(false);
        Amy.SetActive(true);
    }

    void ControleAtaquesUI()
    {
        if(PlayerPrefs.GetInt("PERSONAGEM_ATIVO") == 0)
        {
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>().UIZed();
            //***Ataques de acordo com n�veis do Zed
        }
        else
        {
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>().UIAmy();
            int nivel = PlayerPrefs.GetInt("AMY_NIVEL");
            if (nivel > 1 && nivel < 4)
            {
                //***Ativar bot�o atk �gua
            }
            else if(nivel < 5)
            {
                //***Ativar bot�o magia escudo
            }
            else if (nivel == 5)
            {
                //***Ativar bot�o atk fogo
            }
        }
    }

    public void LoadFase(int faseParaIr)
    {
        PlayerPrefs.SetInt("PERSONAGEM_ATIVO", intPersonagemAtivo);
        SceneManager.LoadScene(faseParaIr);
    }
}
