using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform positionPivot;
    public ItemRotator itemRotator;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    public bool inspecting = false;

    void Update()
    {
        // Botão direito -> sair da inspeção
        if (inspecting && Input.GetMouseButtonDown(1))
        {
            ExitInspectMode();
        }
    }

    public void TeleportAndEnableRotation()
    {
        if (inspecting) return;

        // Salva posição inicial
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        // Teleporta para o pivot
        transform.position = positionPivot.position;

        // DIREÇÃO da câmera -> item (para alinhar eixo X)
        Vector3 direction = (Camera.main.transform.position - transform.position).normalized;

        // Criar rotação com eixo X apontando para a câmera
        Quaternion lookRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Mas LookRotation usa o eixo Z como "frente".
        // Então giramos 90° para que o EIXO X fique voltado para a câmera.
        Quaternion xAlignedRotation = lookRotation * Quaternion.Euler(0, -90f, 0);

        // Aplicar a rotação corrigida
        transform.rotation = xAlignedRotation;

        // Resetar rotação local do modelo interno
        if (itemRotator != null && itemRotator.rotationRoot != null)
            itemRotator.rotationRoot.localRotation = Quaternion.identity;

        // Ativar rotação
        itemRotator.EnableRotation(true);

        inspecting = true;

        Debug.Log("Entrou na inspeção com eixo X alinhado à câmera.");
    }

    public void ExitInspectMode()
    {
        // Volta item ao local original
        transform.SetPositionAndRotation(
            originalPosition,
            originalRotation
        );

        // Desliga rotação
        itemRotator.EnableRotation(false);

        inspecting = false;

        Debug.Log("Saiu da inspeção do item e voltou ao puzzle.");
    }
}
