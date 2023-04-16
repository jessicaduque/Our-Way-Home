using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaseInicial : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Setar os stats iniciais dos personagens
        PlayerPrefs.SetInt("AMY_NIVEL", 1);
        PlayerPrefs.SetFloat("AMY_EXP", 0);
        PlayerPrefs.SetFloat("AMY_VIDA", 10);
        PlayerPrefs.SetFloat("AMY_MANA", 10);

        PlayerPrefs.SetInt("ZED_NIVEL", 1);
        PlayerPrefs.SetFloat("ZED_EXP", 0);
        PlayerPrefs.SetFloat("ZED_VIDA", 10);
        PlayerPrefs.SetFloat("ZED_STAMINA", 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
