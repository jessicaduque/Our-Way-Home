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
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().LoadFase(faseParaIr);
            }
        }
    }


}