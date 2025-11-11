using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PcPasswordPuzzle : MonoBehaviour
{
    [Header("Painéis")]
    public GameObject panelLogin;
    public GameObject panelCarta;

    [Header("Login UI")]
    public TMP_InputField inputSenha;
    public TextMeshProUGUI feedbackText;

    [Header("Config")]
    public string senhaCorreta = "1505";

    void Start()
    {
        if (panelLogin) panelLogin.SetActive(false);
        if (panelCarta) panelCarta.SetActive(false);
        if (feedbackText) feedbackText.text = "";
    }

    // Abre o painel de login (chamado ao clicar no PC)
    public void AbrirComputador()
    {
        if (panelCarta) panelCarta.SetActive(false);
        if (feedbackText) feedbackText.text = "";
        if (inputSenha) inputSenha.text = "";
        if (panelLogin) panelLogin.SetActive(true);
    }

    // Botão CONFIRMAR
    public void ConfirmarSenha()
    {
        if (inputSenha == null || feedbackText == null) return;

        if (inputSenha.text == senhaCorreta)
        {
            feedbackText.text = "Acesso permitido.";
            if (panelLogin) panelLogin.SetActive(false);
            if (panelCarta) panelCarta.SetActive(true);
            Debug.Log("Puzzle do PC concluído!");
        }
        else
        {
            feedbackText.text = "Senha incorreta.";
            Debug.Log("Senha incorreta.");
        }
    }

    // Botões FECHAR (fecha qualquer painel aberto)
    public void FecharTudo()
    {
        if (panelLogin) panelLogin.SetActive(false);
        if (panelCarta) panelCarta.SetActive(false);
    }
}
