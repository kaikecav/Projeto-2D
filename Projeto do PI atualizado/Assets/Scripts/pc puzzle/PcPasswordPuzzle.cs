using System.Text;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PcPasswordPuzzle : MonoBehaviour
{
    [Header("Painéis")]
    public GameObject panelLogin;
    public GameObject panelCarta;

    [Header("Login UI")]
    public TMP_InputField inputSenha;
    public TextMeshProUGUI feedbackText;
    public Button btnConfirmar;

    [Header("Config")]
    public string senhaCorreta = "1234";

    // armazena o último valor real recebido pelo onValueChanged
    private string _ultimoValorDigitado = string.Empty;

    void Start()
    {
        if (panelLogin) panelLogin.SetActive(false);
        if (panelCarta) panelCarta.SetActive(false);
        if (feedbackText) feedbackText.text = "";

        if (inputSenha != null)
        {
            inputSenha.onValueChanged.RemoveListener(OnInputValueChanged);
            inputSenha.onValueChanged.AddListener(OnInputValueChanged);
            _ultimoValorDigitado = inputSenha.text ?? "";
        }

        if (btnConfirmar != null)
        {
            btnConfirmar.onClick.RemoveAllListeners();
            btnConfirmar.onClick.AddListener(ConfirmarSenha);
        }
    }

    void OnDestroy()
    {
        if (inputSenha != null)
            inputSenha.onValueChanged.RemoveListener(OnInputValueChanged);
        if (btnConfirmar != null)
            btnConfirmar.onClick.RemoveListener(ConfirmarSenha);
    }

    private void OnInputValueChanged(string value)
    {
        _ultimoValorDigitado = value;
        Debug.Log($"[onValueChanged] {_GetInputName()} -> [{value}] (len {value?.Length ?? 0})");
    }

    // =====================================
    // Abre o painel — agora evita reabrir/limpar indevidamente
    // =====================================
    public void AbrirComputador()
    {
        if (panelLogin == null) return;

        // Se já está aberto, não reabra nem limpe (evita apagar o que o jogador digitou)
        if (panelLogin.activeSelf)
        {
            Debug.Log("[PcPasswordPuzzle] panelLogin já está aberto — ignorando reabertura.");
            return;
        }

        if (panelCarta) panelCarta.SetActive(false);
        if (feedbackText) feedbackText.text = "";

        // Limpa o valor guardado (sem notificar onValueChanged) — só se você realmente quer limpar ao abrir.
        // Caso prefira preservar o que já estava, comente as duas linhas abaixo.
        if (inputSenha != null)
        {
            // Evita disparar onValueChanged e evita zero-width inserido em algumas versões do TMP
            inputSenha.SetTextWithoutNotify(string.Empty);
            _ultimoValorDigitado = string.Empty;
            inputSenha.ForceLabelUpdate();
        }

        panelLogin.SetActive(true);
        StartCoroutine(DelayFocus());
    }

    IEnumerator DelayFocus()
    {
        yield return new WaitForEndOfFrame();

        if (inputSenha)
        {
            inputSenha.Select();
            inputSenha.ActivateInputField();
            inputSenha.ForceLabelUpdate();

            // Garante que _ultimoValorDigitado reflita o estado atual (vazio ou não)
            _ultimoValorDigitado = inputSenha.text ?? "";
        }
    }

    // limpa caracteres invisíveis/control e trim
    private string CleanInvisibleAndTrim(string s)
    {
        if (string.IsNullOrEmpty(s)) return string.Empty;

        var sb = new StringBuilder(s.Length);
        foreach (char c in s)
        {
            var cat = CharUnicodeInfo.GetUnicodeCategory(c);
            if (cat == UnicodeCategory.Format || cat == UnicodeCategory.Control)
                continue;
            if (char.IsSurrogate(c)) continue;
            sb.Append(c);
        }
        return sb.ToString().Trim();
    }

    // PUBLIC: chamado pelo botão — inicia coroutine que aguarda 1 frame antes de confirmar
    public void ConfirmarSenha()
    {
        StartCoroutine(ConfirmAfterFrame());
    }

    // coroutine que espera o próximo frame (deixando TMP processar o último key event)
    private IEnumerator ConfirmAfterFrame()
    {
        // espera 1 frame para garantir que onValueChanged seja disparado
        yield return null;

        // força atualização visual do TMP (extra garantia)
        if (inputSenha != null)
            inputSenha.ForceLabelUpdate();

        // prioriza o valor capturado em tempo real, mas faz fallback para o texto atual do TMP
        string raw = _ultimoValorDigitado ?? "";
        if (string.IsNullOrEmpty(raw) && inputSenha != null)
        {
            raw = inputSenha.textComponent != null ? inputSenha.textComponent.text : inputSenha.text;
        }

        string digitado = CleanInvisibleAndTrim(raw);

        // logs para diagnóstico
        Debug.Log($"[DEBUG] ultimoValorDigitado (raw): '{raw}' (len {raw?.Length ?? 0}) -> codes: [{CodesOfString(raw)}]");
        Debug.Log($"[DEBUG] cleaned text: '{digitado}' (len {digitado.Length}) -> codes: [{CodesOfString(digitado)}]");
        Debug.Log($"[DEBUG] senhaCorreta: '{senhaCorreta}' -> codes: [{CodesOfString(senhaCorreta)}]");

        if (feedbackText == null)
        {
            Debug.LogError("[PcPasswordPuzzle] feedbackText não atribuído.");
            yield break;
        }

        if (digitado == senhaCorreta)
        {
            feedbackText.text = "Acesso permitido.";
            Debug.Log("✔ Puzzle do PC concluído!");

            if (panelLogin) panelLogin.SetActive(false);
            if (panelCarta) panelCarta.SetActive(true);
        }
        else
        {
            feedbackText.text = "Senha incorreta.";
            Debug.Log("✘ Senha incorreta!");
        }
    }

    private string CodesOfString(string s)
    {
        if (string.IsNullOrEmpty(s)) return "";
        var parts = new System.Collections.Generic.List<string>(s.Length);
        foreach (char c in s)
            parts.Add(((int)c).ToString());
        return string.Join(", ", parts);
    }

    public void FecharTudo()
    {
        if (panelLogin) panelLogin.SetActive(false);
        if (panelCarta) panelCarta.SetActive(false);
    }

    private string _GetInputName()
    {
        return inputSenha == null ? "InputSenha(NULL)" : inputSenha.gameObject.name;
    }
}
