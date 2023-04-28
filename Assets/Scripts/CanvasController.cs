using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CanvasController : MonoBehaviour
{

    private GameObject Player;
    public GameObject telaMortePanel;
    public AudioSource MusicaFundo;

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
        float vida = PlayerPrefs.GetFloat("ZED_VIDA") / 10;
        int nivel = PlayerPrefs.GetInt("ZED_NIVEL");
        float exp = PlayerPrefs.GetFloat("ZED_EXP") / (10 * nivel);
        float stamina = PlayerPrefs.GetFloat("ZED_STAMINA") / 10;

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

        if (nivel >= 2)
        {
            Zed_Atk2.gameObject.SetActive(true);
        }
        else if (nivel >= 5)
        {
            Zed_Atk3.gameObject.SetActive(true);
        }
    }

    public void UIAmyDados()
    {
        float vida = PlayerPrefs.GetFloat("AMY_VIDA") / 10;
        int nivel = PlayerPrefs.GetInt("AMY_NIVEL");
        float exp = PlayerPrefs.GetFloat("AMY_EXP") / (10 * nivel);
        float mana = PlayerPrefs.GetFloat("AMY_MANA") / 10;

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

        if (nivel >= 2)
        {
            Amy_AtkAgua.gameObject.SetActive(true);
        }
        else if (nivel >= 4)
        {
            Amy_MagiaEscudo.gameObject.SetActive(true);
        }
        else if (nivel >= 5)
        {
            Amy_AtkFogo.gameObject.SetActive(true);
        }
    }

    public void TelaMorte()
    {
        Amy_Panel.SetActive(false);
        Zed_Panel.SetActive(false);
        telaMortePanel.SetActive(true);
        MusicaFundo.enabled = false;
        Time.timeScale = 0;
    }

    public void ContinuarMorte()
    {
        // Amy
        PlayerPrefs.SetInt("AMY_VIVO", 1);
        PlayerPrefs.SetFloat("AMY_VIDA", 10);
        PlayerPrefs.SetFloat("AMY_MANA", 10);

        // Zed
        PlayerPrefs.SetInt("ZED_VIVO", 1);
        PlayerPrefs.SetFloat("ZED_VIDA", 10);
        PlayerPrefs.SetFloat("ZED_STAMINA", 10);
        

        int faseAtual = GameObject.FindGameObjectWithTag("GameController").GetComponent<GerenciadorFase>().faseAtual;
        SceneManager.LoadScene(faseAtual);
    }
}
