using UnityEngine;

public class ClickManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera; // arraste a Main Camera aqui no inspetor
    [SerializeField] private float clickRange = 100f; // distância máxima do clique

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // clique esquerdo do mouse
        {
            //determina o "raio" que o mouse estava clicando e se o objeto era um com interação
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);     
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, clickRange))      //verifica se o sinal foi recebido como true
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();// verifica se o objeto clicado tem um Interactable

                if (interactable != null)       //determina que se o objeto interativo existe, ou seja é diferente de null(nada)
                {
                    interactable.Interact(); // executa o comportamento (no caso, coletar e destruir)
                    
                }
            }
        }
    }
}