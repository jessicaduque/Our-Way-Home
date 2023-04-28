using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassarFase : MonoBehaviour
{
    public int faseParaIr;
    GerenciadorFase GerenciadorFase;
    bool podePassar;

    private void Start()
    {
        GerenciadorFase = GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>();
    }

    private void Update()
    {
        if (GerenciadorFase.faseCompleta)
        {
            podePassar = true;
        }
    }

    private void OnTriggerEnter(Collider colidiu)
    {
        if (colidiu.gameObject.tag == "Player")
        {
            if (podePassar)
            {
                ControleFaseStatus();
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().LoadFase(faseParaIr);
            }
        }
    }

    void ControleFaseStatus()
    {
        int faseAtual = GerenciadorFase.faseAtual;
        if (faseAtual == 1)
        {
            PlayerPrefs.SetInt("FASE1", 1);
        }
        if (faseAtual == 2)
        {
            if(PlayerPrefs.GetInt("FASE2") == 0)
            {
                PlayerPrefs.SetInt("FASE2", 1);
            }
            else
            {
                PlayerPrefs.SetInt("FASE2", 2);
            }
        }
        if (faseAtual == 3)
        {
            PlayerPrefs.SetInt("FASE3", 1);
        }
        if (faseAtual == 4)
        {
            PlayerPrefs.SetInt("FASE4", 1);
        }
        if (faseAtual == 5)
        {
            PlayerPrefs.SetInt("FASE5", 1);
        }
        if (faseAtual == 6)
        {
            PlayerPrefs.SetInt("FASE6", 1);
        }
        if (faseAtual == 7)
        {
            PlayerPrefs.SetInt("FASE7", 1);
        }
        if (faseAtual == 8)
        {
            PlayerPrefs.SetInt("FASE8", 1);
        }
        if (faseAtual == 9)
        {
            PlayerPrefs.SetInt("FASE9", 1);
        }
        if (faseAtual == 10)
        {
            PlayerPrefs.SetInt("FASE10", 1);
        }
    }
}