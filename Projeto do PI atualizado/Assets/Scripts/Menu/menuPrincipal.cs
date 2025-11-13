using UnityEngine;
using UnityEngine.SceneManagement;

public class menuPrincipal : MonoBehaviour
{
    //funções que criam os elementos do menu
    //SerializeField permite acessar essa função no espectro da unity, mas não permite editar ele com outros scripts
    [SerializeField] private GameObject painelmenuinicial;
    [SerializeField] private GameObject painelopcoes;
    [SerializeField] private GameObject MenudoNewGame;

    // define o estado inicial do menu
    void Start()
    {
        painelmenuinicial.SetActive(true);
        painelopcoes.SetActive(false);
        MenudoNewGame.SetActive(false);
    }

    //função que determina se o menu do jogo vai ser ativado
    public void Iniciar()
    {
        // igual ao vídeo, esconde o menu inicial
        painelmenuinicial.SetActive(false);

        MenudoNewGame.SetActive(true);
    }

    //função que carrega a fase escolhida caso o botão seja pressionado
    public void NovoJogo()
    {
        SceneManager.LoadScene("MapaTeste");        //LoadScene muda para a cena determinada
    }

    //função que permite retornar ao menu inicial
    public void VoltarAoMenu()
    {
        painelmenuinicial.SetActive(true);       //ativa o painel
        MenudoNewGame.SetActive(false);          //desativa a função menunewgame
        painelopcoes.SetActive(false);           //  garante que opções também feche
    }

    //função que permite acessar o menu de opções do jogo ao pressionar o botão
    public void AbrirOpcoes()
    {
        painelmenuinicial.SetActive(false);     //desativa o painel menuinicial
        painelopcoes.SetActive(true);           //ativa o painel
    }

    //função que fecha o menu de opções do jogo ao pressionar o botão
    public void FecharOpcoes()
    {
        painelmenuinicial.SetActive(true);      //ativa o painel
        painelopcoes.SetActive(false);          //desativa o painel de opções
    }

    //função que permite sair do jogo ao pressionar o botão
    public void Sair()
    {
        Debug.Log("Sair");      //escreve no console o que foi solicitado
        Application.Quit();     //fecha o jogo
    }
}

