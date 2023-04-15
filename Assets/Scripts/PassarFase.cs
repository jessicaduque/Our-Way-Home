using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassarFase : MonoBehaviour
{
    public int faseParaIr;

    private void OnTriggerEnter(Collider colidiu)
    {
        if (colidiu.gameObject.tag == "Player")
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().LoadFase(faseParaIr);
        }
    }


}