using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fase2Dialogo : MonoBehaviour
{
    public Text falaTexto;
    int numeroFala;
    bool falasRodando;
    float tempo = 0.0f;

    public GameObject AmyFalsa;
    public GameObject ZedFalso;
    public GameObject SlimeRabbitVerdadeiro;
    public GameObject SlimeRabbitFalso;

    public GameObject DialoguePanel;
    public Text NomeFalante_Text;
    public Image NomeFalante_Image;
    public Image Falante_Amy_Image;
    public Image Falante_Zed_Image;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        if (PlayerPrefs.GetInt("FASE2") == 0)
        {
            numeroFala = 0;

        }
        else if (PlayerPrefs.GetInt("FASE2") == 1)
        {
            numeroFala = 18;
            SlimeRabbitFalso.SetActive(false);
        }
        DialoguePanel.SetActive(false);
        falasRodando = true;
        falaTexto.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerPrefs.GetInt("FASE2") == 0)
        {
            ControleFalas();
        }
        else if(PlayerPrefs.GetInt("FASE2") == 1)
        {
            falasRodando = true;
            ControleFalas();
        }
        else
        {
            AcabouFalas();
        }
    }

    void ScriptFalas()
    {
        // Falas
        if (Input.GetMouseButtonDown(0))
        {
            if (tempo > 0.5f)
            {
                if (numeroFala == 0)
                {
                    if (tempo > 2f)
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
            falaTexto.text = "O que � aquilo?";
        }
        if (numeroFala == 1)
        {
            FalanteZed();
            AmyFalsa.transform.LookAt(SlimeRabbitFalso.transform.position);
            ZedFalso.transform.LookAt(SlimeRabbitFalso.transform.position);
            falaTexto.text = "Parece ser um monstro...";
        }
        if (numeroFala == 2)
        {
            falaTexto.text = "Bem que j� est� no hor�rio que come�am a aparecer por aqui.";
        }
        if (numeroFala == 3)
        {
            FalanteAmy();
            falaTexto.text = "E agora?!";
        }
        if (numeroFala == 4)
        {
            FalanteZed();
            falaTexto.text = "� nossa chance! Voc� n�o disse que queria usar a magia que voc� esteve aprendendo na pr�tica?";
        }
        if (numeroFala == 5)
        {
            FalanteAmy();
            falaTexto.text = "Bem, sim.... Agora � minha oportunidade...";
        }
        if (numeroFala == 6)
        {
            falaTexto.text = "Ou melhor, acho que precisaremos lutar para chegar em casa!";
        }
        if (numeroFala == 7)
        {
            FalanteZed();
            falaTexto.text = "Isso a�! Tamb�m ainda n�o usei minhas habilidades com a espada na pr�tica.";
        }
        if (numeroFala == 8)
        {
            FalanteAmy();
            falaTexto.text = "Pode deixar que vou nos proteger!";
        }
        if (numeroFala == 9)
        {
            FalanteZed();
            falaTexto.text = "Uhm.... Claro.";
        }
        if (numeroFala == 10)
        {
            FalanteNarrador();
            falaTexto.text = "<b> Ol� de novo! Deixa eu te falar mais algumas coisas.</b>";
        }
        if (numeroFala == 11)
        {
            FalanteNarrador();
            falaTexto.text = "<b>Al�m dos bot�es n�mericos para a��es, voc� pode usar o bot�o direito do mouse para usar seu escudo.</b>";
        }
        if (numeroFala == 12)
        {
            FalanteNarrador();
            falaTexto.text = "<b>Quanto mais voc� luta, mais experi�ncia ganha, para subir n�veis!</b>";
        }
        if (numeroFala == 13)
        {
            FalanteNarrador();
            falaTexto.text = "<b>Quanto maior seu n�veis, mais a��es ter�.</b>";
        }
        if (numeroFala == 13)
        {
            FalanteNarrador();
            falaTexto.text = "<b>Ah, e fique de olho em sua vida. Como Amy usa magia, monitore sua mana, e para o Zed olhe seu stamina tamb�m.</b>";
        }
        if (numeroFala == 14)
        {
            FalanteNarrador();
            falaTexto.text = "<b>Para come�ar, Zed consegue fazer um ataque b�sico de espada.</b>";
        }
        if (numeroFala == 15)
        {
            FalanteNarrador();
            falaTexto.text = "<b>J� a Amy, pode curar sua vida e do Zed, al�m de um pouco de stamina dele.</b>";
        }
        if (numeroFala == 16)
        {
            FalanteNarrador();
            falaTexto.text = "<b>Chega de papinho, n�? Boa sorte aventureiro!</b>";
        }

        // Caso o player volte a cena novamente
        if (numeroFala == 18)
        {
            FalanteZed();
            falaTexto.text = "Acho... que demos uma volta.";
        }
        if (numeroFala == 19)
        {
            FalanteAmy();
            falaTexto.text = "Hum... Tudo s� � bem parecido, se preocupa n�o.";
        }
        if (numeroFala == 20)
        {
            falaTexto.text = "Eu acho.";
        }
    }

    void ControleFalas()
    {
        
        if (falasRodando)
        {
            tempo += Time.deltaTime;
        }

        if (numeroFala == 0 || numeroFala == 18)
        {
            Debug.Log(tempo);
            if (tempo >= 1f)
            {
                DialoguePanel.SetActive(true);
                ScriptFalas();
            }
        }
        else if(numeroFala == 17)
        {
            AcabouFalas();
        }
        else if (numeroFala == 21)
        {
            PlayerPrefs.SetInt("FASE2", 2);
            AcabouFalas();
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

    void AcabouFalas() {
        ZedFalso.SetActive(false);
        AmyFalsa.SetActive(false);
        SlimeRabbitFalso.SetActive(false);
        DialoguePanel.SetActive(false);
        GetComponent<GerenciadorFase>().enabled = true;
        SlimeRabbitVerdadeiro.SetActive(true);
        this.enabled = false;
    }
}
