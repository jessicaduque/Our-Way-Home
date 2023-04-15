using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GerenciadorFase : MonoBehaviour
{
    public Vector3 PosicaoInicial;
    public GameObject Amy;
    public GameObject Zed;
    public GameObject PersonagemAtivo;

    //public int faseAtual;

    //PlayerPrefs.SetInt("FASE", faseAtual);
    //int fSave = PlayerPrefs.GetInt("FASE");

    private void Start()
    {
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
                Zed.transform.position = Amy.transform.position;
                PersonagemAtivo = Zed;
                Zed.SetActive(true);
                Amy.SetActive(false);
            }
            else
            {
                Amy.transform.position = Zed.transform.position;
                PersonagemAtivo = Amy;
                Zed.SetActive(false);
                Amy.SetActive(true);
            }
        }

    }

    public void LoadFase(int faseParaIr)
    {
        SceneManager.LoadScene(faseParaIr);
    }

}
