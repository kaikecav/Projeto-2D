using UnityEngine;

public class ItemRotator : MonoBehaviour
{
    //VARIÁVEIS
    [Header("Objeto que será rotacionado")]
    public Transform rotationRoot;

    [Header("Configurações de velocidade do giro")]
    public float rotationSpeed = 50f;

    [HideInInspector] public bool canRotate = false;    //Diz se pode ou não rotacionar

    private bool dragging = false;  //Arrastar do mouse
    private Vector3 lastMousePosition;  //Grava a última posição do mouse

    void Update()
    {
        //Se não pode rotacionar no momento, sai
        if (!canRotate || rotationRoot == null)
            return;

        //Início do arrastar do mouse
        if (Input.GetMouseButtonDown(0))
        {
            dragging = true;
            lastMousePosition = Input.mousePosition;
        }

        //Final do arrastar do mouse
        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }

        //Ativa a rotação durante o arrasto
        if (dragging)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;

            //Verificação dos eixos a serem rotacionados
            float rotX = delta.y * rotationSpeed * Time.unscaledDeltaTime;
            float rotY = -delta.x * rotationSpeed * Time.unscaledDeltaTime;

            //Rotação estável em espaço global
            rotationRoot.Rotate(Vector3.up, rotY, Space.World);
            rotationRoot.Rotate(Vector3.right, rotX, Space.World);

            lastMousePosition = Input.mousePosition;
        }
    }

    //VINCULO COM O PICKUP
    
    //Função para ativar a rotação
    public void EnableRotation(bool state)
    {
        canRotate = state;

        if (state)
            dragging = false;
    }

    //Função para garantir a ativação da rotação
    public void EnableRotation()
    {
        EnableRotation(true);
    }
}
