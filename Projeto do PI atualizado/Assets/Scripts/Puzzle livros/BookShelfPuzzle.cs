using UnityEngine;
using System.Collections.Generic;

public class BookShelfPuzzle : MonoBehaviour
{
    [Header("Livros na estante (arraste TODOS aqui em qualquer ordem)")]
    public List<Transform> livros;

    [Header("Ordem correta (arraste os livros NA ORDEM CERTA)")]
    public List<Transform> ordemCorreta;

    [Header("Folha de respostas que aparece quando completa")]
    public GameObject folhaRespostas;

    private BookClick primeiroSelecionado;
    private BookClick segundoSelecionado;

    // posição original antes de puxar o livro
    private Vector3 posOriginalPrimeiro;

    // intensidade do puxão para frente
    public float puxarOffset = 0.25f;

    void Start()
    {
        if (folhaRespostas != null)
            folhaRespostas.SetActive(false);
    }

    // Chamado pelo BookClick quando o livro é clicado
    public void SelecionarLivro(BookClick livroClicado)
    {
        // Primeiro clique
        if (primeiroSelecionado == null)
        {
            primeiroSelecionado = livroClicado;

            // salva posição original
            posOriginalPrimeiro = primeiroSelecionado.transform.position;

            // puxa o livro para frente (EIXO Z)
            primeiroSelecionado.transform.position =
                new Vector3(
                    posOriginalPrimeiro.x,
                    posOriginalPrimeiro.y,
                    posOriginalPrimeiro.z - puxarOffset
                );

            Debug.Log($"Selecionou {livroClicado.name}");
            return;
        }

        // Segundo clique (não pode clicar o mesmo)
        if (livroClicado != primeiroSelecionado)
        {
            segundoSelecionado = livroClicado;
            Debug.Log($"Selecionou {livroClicado.name} para trocar");
            TrocarLivros();
        }
    }

    void TrocarLivros()
    {
        // volta o livro selecionado para sua posição original
        primeiroSelecionado.transform.position = posOriginalPrimeiro;

        // troca posições
        Vector3 posTemp = primeiroSelecionado.transform.position;
        primeiroSelecionado.transform.position = segundoSelecionado.transform.position;
        segundoSelecionado.transform.position = posTemp;

        Debug.Log($"Trocou {primeiroSelecionado.name} com {segundoSelecionado.name}");

        // limpando seleção
        primeiroSelecionado = null;
        segundoSelecionado = null;

        // verifica se o puzzle está correto
        VerificarOrdem();
    }

    void VerificarOrdem()
    {
        List<Transform> livrosOrdenados = new List<Transform>(livros);
        livrosOrdenados.Sort((a, b) => a.position.x.CompareTo(b.position.x));

        Debug.Log("ORDEM DETECTADA");
        for (int i = 0; i < livrosOrdenados.Count; i++)
            Debug.Log($"Pos {i}: {livrosOrdenados[i].name}");

        Debug.Log("ORDEM CORRETA ESPERADA");
        for (int i = 0; i < ordemCorreta.Count; i++)
            Debug.Log($"Pos {i}: {ordemCorreta[i].name}");

        bool tudoCerto = true;

        for (int i = 0; i < livrosOrdenados.Count; i++)
        {
            if (livrosOrdenados[i] != ordemCorreta[i])
            {
                tudoCerto = false;
                Debug.Log($"Erro na posição {i}: {livrosOrdenados[i].name} != {ordemCorreta[i].name}");
                break;
            }
        }

        if (tudoCerto)
        {
            Debug.Log("PUZZLE COMPLETO!");
            if (folhaRespostas != null)
                folhaRespostas.SetActive(true);
        }
    }
}
