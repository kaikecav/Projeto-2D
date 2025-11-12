using UnityEngine;
using UnityEngine.UI;

public class FoboExpressionsUI : MonoBehaviour
{
    
    //VARIÁVEIS
    [Header("Referências")]
    public SanityManager sanityManager;
    private Image image;

    [Header("Sprites / Estados")]
    public Sprite normalSprite;
    public Sprite tensoSprite;
    public Sprite superTensoSprite;
    public Sprite mortoSprite;

    private bool entrouEmMorto = false; //registra se ativou o estado morto
    private FoboState estadoAtual = FoboState.Normal;   //determina o estado do Fobo, iniciando no estado normal

    void Start()
    {
        //Coloca a imagem do Fobo
        image = GetComponent<Image>();
        if (image == null)
        {
            Debug.LogError("Nenhum Image encontrado!");
            return;
        }

        if (sanityManager == null)
            Debug.LogWarning("SanityManager não atribuído!");

        //iniciando no estado normal
        SetState(FoboState.Normal);
    }

    void Update()
    {
        if (sanityManager == null || sanityManager.sanitySlider == null)
            return;

        float currentSanity = sanityManager.sanitySlider.value;

        //Determina que se a sanidade chegar ao valor 10
        if (currentSanity == 10f)
        {
            if (!entrouEmMorto)
            {
                //Coloca uma chance de 5% dele entrar no estado morto
                if (Random.value <= 0.05f)
                {
                    entrouEmMorto = true;
                    SetState(FoboState.Morto);
                    estadoAtual = FoboState.Morto;
                    return;
                }
            }

            //Os outros 95% mantem o estado de SuperTenso
            SetState(FoboState.SuperTenso);
            estadoAtual = FoboState.SuperTenso;
            return;
        }

        //Atualiza o Estado do Fobo de acordo com o valor da sanidade
        FoboState novoEstado;

        if (currentSanity > 6600f)
            novoEstado = FoboState.Normal;
        else if (currentSanity > 3300f)
            novoEstado = FoboState.Tenso;
        else
            novoEstado = FoboState.SuperTenso;

        //Substitui o Estado atual pelo novo Estado
        if (novoEstado != estadoAtual)
        {
            estadoAtual = novoEstado;
            SetState(novoEstado);
        }
    }

    //Lista dos estados
    enum FoboState { Normal, Tenso, SuperTenso, Morto }

    //FUNÇÃO PARA PEGAR CADA ESTADO
    void SetState(FoboState state)
    {
        if (image == null)
            return;

        switch (state)
        {
            case FoboState.Normal: image.sprite = normalSprite; break;
            case FoboState.Tenso: image.sprite = tensoSprite; break;
            case FoboState.SuperTenso: image.sprite = superTensoSprite; break;
            case FoboState.Morto: image.sprite = mortoSprite; break;
        }
    }
}
