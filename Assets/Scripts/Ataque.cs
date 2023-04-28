using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ataque : MonoBehaviour
{
    public string nome;
    public float dano;
    public bool DPS;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(nome == "CirculoFogo" || nome == "Slash" || nome == "Pedra")
        {
            Destroy(this.gameObject, 3f);
        }
    }

}
