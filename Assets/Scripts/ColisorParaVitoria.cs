using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColisorParaVitoria : MonoBehaviour
{
    public GameObject VitoriaPanel;

    private void Update()
    {
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            VitoriaPanel.SetActive(true);
        }
    }
}
