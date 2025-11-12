using UnityEngine;

public class BookClick : MonoBehaviour
{
    private BookShelfPuzzle controlador;

    void Start()
    {
        controlador = FindObjectOfType<BookShelfPuzzle>();
    }

    void OnMouseDown()
    {
        Debug.Log($"Livro {gameObject.name} clicado!");
        if (controlador != null)
        {
            controlador.SelecionarLivro(this);
        }
    }
}
