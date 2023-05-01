using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FaseInicial : MonoBehaviour
{
    public Text falaTexto;
    int numeroFala = 0;
    bool falasRodando;
    float tempo = 0.0f;

    public GameObject AmyFalsa;
    public GameObject ZedFalso;

    public GameObject DialoguePanel;
    public Text NomeFalante_Text;
    public Image NomeFalante_Image;
    public Image Falante_Amy_Image;
    public Image Falante_Zed_Image;

    // Start is called before the first frame update
    void Start()
    {
        DialoguePanel.SetActive(false);
        falasRodando = true;
        falaTexto.text = "";


        // Iniciar Amy como personagem ativa na primeira fase
        PlayerPrefs.SetInt("PERSONAGEM_ATIVO", 1);

        // Setar os stats iniciais dos personagens

        // Amy
        PlayerPrefs.SetInt("AMY_NIVEL", 1);
        PlayerPrefs.SetFloat("AMY_EXP", 0);
        PlayerPrefs.SetFloat("AMY_VIDA", 10);
        PlayerPrefs.SetFloat("AMY_MANA", 10);
        PlayerPrefs.SetInt("AMY_VIVO", 1);

        // Zed
        PlayerPrefs.SetInt("ZED_NIVEL", 1);
        PlayerPrefs.SetFloat("ZED_EXP", 0);
        PlayerPrefs.SetFloat("ZED_VIDA", 10);
        PlayerPrefs.SetFloat("ZED_STAMINA", 10);
        PlayerPrefs.SetInt("ZED_VIVO", 1);

        // Fases
        PlayerPrefs.SetInt("FASE1", 0);
        PlayerPrefs.SetInt("FASE2", 0);
        PlayerPrefs.SetInt("FASE3", 0);
        PlayerPrefs.SetInt("FASE4", 0);
        PlayerPrefs.SetInt("FASE5", 0);
        PlayerPrefs.SetInt("FASE6", 0);
        PlayerPrefs.SetInt("FASE7", 0);
        PlayerPrefs.SetInt("FASE8", 0);
        PlayerPrefs.SetInt("FASE9", 0);
        PlayerPrefs.SetInt("FASE10", 0);
    }

    // Update is called once per frame
    void Update()
    {
        ControleFalas();

        if (!falasRodando)
        {
            falaTexto.text = "";
        }
    }

    void ScriptFalas()
    {
        // Falas
        if (Input.GetMouseButtonDown(0))
        {
            if (tempo > 0.5f)
            {
                if(numeroFala == 0)
                { 
                    if(tempo > 2f)
                    {
                        tempo = 0.0f;
                        numeroFala++;
                    }
                }
                else
                {
                    tempo = 0.0f;
                    numeroFala++;
                }
            }
        }

        if (numeroFala == 0)
        {
            FalanteAmy();
            falaTexto.text = "Eu acho...";
        }
        if (numeroFala == 1)
        {
            falaTexto.text = "Que estamos perdidos.";
        }
        if (numeroFala == 2)
        {
            FalanteZed();
            falaTexto.text = "Como assim?!";
        }
        if (numeroFala == 3)
        {
            falaTexto.text = "Você não disse que já decorou os caminhos da floresta?";
        }
        if (numeroFala == 4)
        {
            FalanteAmy();
            falaTexto.text = "Eu disse? Não lembro.";
        }
        if (numeroFala == 5)
        {
            FalanteZed();
            falaTexto.text = "Mas cinco minutos atrás você disse que já estávamos chegando?";
        }
        if (numeroFala == 6)
        {
            FalanteAmy();
            falaTexto.text = "Aqui só é muito grande, tá?";
        }
        if (numeroFala == 7)
        {
            FalanteZed();
            falaTexto.text = "Sabia que eu não podia deixar isso com você.";
        }
        if (numeroFala == 8)
        {
            FalanteAmy();
            falaTexto.text = "Vem irmãozinho, é só andarmos um pouco que devemos chegar em casa.";
        }
        if (numeroFala == 9)
        {
            FalanteZed();
            falaTexto.text = "Ok...";
        }
        if (numeroFala == 10)
        {
            FalanteNarrador();
            falaTexto.text = "<b>Bem vindo(a)! Vou te ajudar aqui a conseguir guiar estes dois de volta para casa.</b>";
        }
        if (numeroFala == 11)
        {
            falaTexto.text = "<b>Você pode mover eles com um simples clique do mouse.</b>";
        }
        if (numeroFala == 12)
        {
            falaTexto.text = "<b>Para trocar o personagem ativo, aperte a barra de espaço.</b>";
        }
        if (numeroFala == 13)
        {
            falaTexto.text = "<b>Para usar as ações de cada, só usar as teclas númericas.</b>";
        }
        if (numeroFala == 14)
        {
            falaTexto.text = "<b>Vamos, tente aí!</b>";
        }
    }

    void ControleFalas()
    {
        if (falasRodando)
        {
            tempo += Time.deltaTime;
        }

        if (numeroFala == 0)
        {
            if (tempo >= 1f)
            {
                DialoguePanel.SetActive(true);
                ScriptFalas();
            }
        }
        else if(numeroFala == 15)
        {
            ZedFalso.SetActive(false);
            AmyFalsa. SetActive(false);
            DialoguePanel.SetActive(false);
            GetComponent<GerenciadorFase>().enabled = true;
            this.enabled = false;
        }
        else
        {
            ScriptFalas();
        }
    }

    void FalanteAmy()
    {
        NomeFalante_Text.text = "Amy";
        Falante_Zed_Image.gameObject.SetActive(false);
        Falante_Amy_Image.gameObject.SetActive(true);
        NomeFalante_Image.gameObject.SetActive(true);
    }
    void FalanteZed()
    {
        NomeFalante_Text.text = "Zed";
        Falante_Amy_Image.gameObject.SetActive(false);
        Falante_Zed_Image.gameObject.SetActive(true);
        NomeFalante_Image.gameObject.SetActive(true);
    }
    void FalanteNarrador()
    {
        NomeFalante_Text.text = "";
        Falante_Zed_Image.gameObject.SetActive(false);
        Falante_Amy_Image.gameObject.SetActive(false);
        NomeFalante_Image.gameObject.SetActive(false);
    }
}
