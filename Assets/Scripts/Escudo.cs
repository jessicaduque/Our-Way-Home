using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escudo : MonoBehaviour
{
    GerenciadorFase GerenciadorFase;
    float tempo = 0.0f;

    void Start()
    {
        GerenciadorFase = GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>();
    }

    void Update()
    {
        transform.position = GerenciadorFase.PersonagemAtivo.transform.position + new Vector3(0, 0.22f, 0);
        tempo += Time.deltaTime;

        if(tempo > 10)
        {
            tempo = 0.0f;
            MetadeAtkLigado(false);
            gameObject.SetActive(false);
        }
        else
        {
            MetadeAtkLigado(true);
        }
    }

    void MetadeAtkLigado(bool ligado)
    {
        if (GerenciadorFase.PersonagemAtivo.GetComponent<Amy>())
        {
            GerenciadorFase.PersonagemAtivo.GetComponent<Amy>().MetadeAtk(ligado);
        }
        else
        {
            GerenciadorFase.PersonagemAtivo.GetComponent<Zed>().MetadeAtk(ligado);
        }
    }
}
