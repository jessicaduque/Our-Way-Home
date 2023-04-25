using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CanvasController : MonoBehaviour
{

    private GameObject Player;

    // Amy
    public GameObject Amy_Panel;
    public Image Amy_EXP;
    public TMP_Text Amy_Nivel;
    public Image Amy_Trocar;
    public Image Amy_AtkAgua;
    public Image Amy_MagiaEscudo;
    public Image Amy_AtkFogo;
    public Image Amy_Vida;
    public Image Amy_Mana;

    // Zed
    public GameObject Zed_Panel;
    public Image Zed_EXP;
    public TMP_Text Zed_Nivel;
    public Image Zed_Trocar;
    public Image Zed_Atk2;
    public Image Zed_Atk3;
    public Image Zed_Vida;
    public Image Zed_Stamina;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    public void UIZedDados()
    {
        float vida = Player.GetComponent<Zed>().hp / 10;
        int nivel = Player.GetComponent<Zed>().nivel;
        float exp = Player.GetComponent<Zed>().exp / (10 * nivel);
        float stamina = Player.GetComponent<Zed>().stamina / 10;

        Zed_Nivel.text = nivel.ToString();
        Zed_Stamina.fillAmount = stamina;
        Zed_Vida.fillAmount = vida;
        Zed_EXP.fillAmount = exp;

        UIZedAtivado(nivel);
    }

    void UIZedAtivado(int nivel)
    {
        Amy_Panel.SetActive(false);
        Zed_Panel.SetActive(true);

        if (nivel == 2)
        {
            Zed_Atk2.gameObject.SetActive(true);
        }
        else if (nivel == 5)
        {
            Zed_Atk3.gameObject.SetActive(true);
        }
    }

    public void UIAmyDados()
    {
        float vida = Player.GetComponent<Amy>().hp / 10;
        int nivel = Player.GetComponent<Amy>().nivel;
        float exp = Player.GetComponent<Amy>().exp / (10 * nivel);
        float mana = Player.GetComponent<Amy>().mana / 10;

        Amy_Nivel.text = nivel.ToString();
        Amy_Mana.fillAmount = mana;
        Amy_Vida.fillAmount = vida;
        Amy_EXP.fillAmount = exp;

        UIAmyAtivado(nivel);
    }
    void UIAmyAtivado(int nivel)
    {
        Zed_Panel.SetActive(false);
        Amy_Panel.SetActive(true);

        if (nivel == 2)
        {
            Amy_AtkAgua.gameObject.SetActive(true);
        }
        else if (nivel == 4)
        {
            Amy_MagiaEscudo.gameObject.SetActive(true);
        }
        else if (nivel == 5)
        {
            Amy_AtkFogo.gameObject.SetActive(true);
        }
    }
}
