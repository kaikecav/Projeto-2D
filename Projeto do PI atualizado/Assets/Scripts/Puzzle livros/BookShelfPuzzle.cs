using UnityEngine;
using System.Collections.Generic;

public class BookShelfPuzzle : MonoBehaviour
{
    public List<Transform> livros;         // Lista com os livros
    public List<string> ordemCorreta;      // Nomes dos livros na ordem certa
    public GameObject folhaRespostas;      // Folha que aparece no final

    private BookClick primeiroSelecionado;
    private BookClick segundoSelecionado;

    void Start()
    {
        if (folhaRespostas != null)
            folhaRespostas.SetActive(false);
    }

    public void SelecionarLivro(BookClick livroClicado)
    {
        // 1º clique — seleciona o primeiro livro
        if (primeiroSelecionado == null)
        {
            primeiroSelecionado = livroClicado;
            livroClicado.transform.localScale *= 1.1f; // destaca o livro
            Debug.Log($"Selecionou {livroClicado.name}");
        }
        // 2º clique — troca com o primeiro
        else if (segundoSelecionado == null && livroClicado != primeiroSelecionado)
        {
            segundoSelecionado = livroClicado;
            Debug.Log($"Selecionou {livroClicado.name} pra trocar");
            TrocarLivros();
        }
    }

    void TrocarLivros()
    {
        // Troca de posição
        Vector3 posTemp = primeiroSelecionado.transform.position;
        primeiroSelecionado.transform.position = segundoSelecionado.transform.position;
        segundoSelecionado.transform.position = posTemp;

        Debug.Log($"🔁 Trocou {primeiroSelecionado.name} com {segundoSelecionado.name}");

        // Tira destaque
        primeiroSelecionado.transform.localScale /= 1.1f;
        segundoSelecionado.transform.localScale /= 1.1f;

        // Limpa seleção
        primeiroSelecionado = null;
        segundoSelecionado = null;

        // Verifica se está na ordem correta
        VerificarOrdem();
    }

    void VerificarOrdem()
    {
        // Ordena os livros da esquerda pra direita (menor X primeiro)
        List<Transform> livrosOrdenados = new List<Transform>(livros);
        livrosOrdenados.Sort((a, b) => a.position.x.CompareTo(b.position.x));

        bool tudoCerto = true;

        for (int i = 0; i < livrosOrdenados.Count; i++)
        {
            if (livrosOrdenados[i].name != ordemCorreta[i])
            {
                tudoCerto = false;
                break;
            }
        }

        if (tudoCerto)
        {
            Debug.Log("🎯 Puzzle completo!");
            if (folhaRespostas != null)
                folhaRespostas.SetActive(true);
        }
    }

}
