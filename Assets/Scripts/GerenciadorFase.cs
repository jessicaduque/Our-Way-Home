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

    //public int faseAtual;

    //PlayerPrefs.SetInt("FASE", faseAtual);
    //int fSave = PlayerPrefs.GetInt("FASE");

    private void Start()
    {
        if(PlayerPrefs.GetInt("PERSONAGEMATIVO") == 0)
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
        ControlePersonagemAtivo();
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

    
    public void LoadFase(int faseParaIr)
    {
        PlayerPrefs.SetInt("PERSONAGEMATIVO", intPersonagemAtivo);
        SceneManager.LoadScene(faseParaIr);
    }

}
