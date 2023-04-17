using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorFase : MonoBehaviour
{
    // Variáveis para início de fase
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
        Amy.GetComponent<Amy>().MetadeAtk(false);
        Zed.SetActive(true);
        Amy.SetActive(false);
    }

    void AmyAtivo()
    {
        Amy.transform.position = Zed.transform.position;
        PersonagemAtivo = Amy;
        intPersonagemAtivo = 1;
        //Zed.GetComponent<Zed>().MetadeAtk(false);
        Zed.SetActive(false);
        Amy.SetActive(true);
    }

    void ControleAtaquesUI()
    {
        if(PlayerPrefs.GetInt("PERSONAGEM_ATIVO") == 0)
        {
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>().UIZed();
            //***Ataques de acordo com níveis do Zed
        }
        else
        {
            GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>().UIAmy();
            int nivel = PlayerPrefs.GetInt("AMY_NIVEL");
            if (nivel > 1 && nivel < 4)
            {
                //***Ativar botão atk água
            }
            else if(nivel < 5)
            {
                //***Ativar botão magia escudo
            }
            else if (nivel == 5)
            {
                //***Ativar botão atk fogo
            }
        }
    }

    public void LoadFase(int faseParaIr)
    {
        PlayerPrefs.SetInt("PERSONAGEM_ATIVO", intPersonagemAtivo);
        SceneManager.LoadScene(faseParaIr);
    }
}
