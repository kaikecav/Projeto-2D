using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{
    [Header("Câmera Principal")]
    [SerializeField] private Camera mainCamera;

    [Header("Câmeras Virtuais (adicione todas aqui)")]
    [SerializeField] private List<Camera> virtualCameras = new List<Camera>();

    [Header("Configuração de clique")]
    [Tooltip("Distância máxima do raycast (padrão: 100).")]
    [SerializeField] private float clickRange = 100f;

    [Tooltip("Distância 'precisa' para objetos próximos (padrão: 3).")]
    [SerializeField] private float precisionDistance = 3f;

    [Tooltip("Se marcado, mostrará gizmo do ray no Editor para debug.")]
    [SerializeField] private bool debugRay = false;

    private Camera activeCam;

    void Start()
    {
        // fallback
        if (mainCamera == null)
            mainCamera = Camera.main;

        activeCam = mainCamera;
    }

    void Update()
    {
        DetectActiveCamera();

        // só processa quando pressionou o botão
        if (!Input.GetMouseButtonDown(0))
            return;

        TryHandleClick();
    }

    private void DetectActiveCamera()
    {
        // prioriza cameras virtuais (aquelas que você arrastar na lista)
        foreach (var cam in virtualCameras)
        {
            if (cam == null) continue;

            // considera ativa se o GameObject e o componente estão habilitados
            if (cam.gameObject.activeInHierarchy && cam.enabled)
            {
                activeCam = cam;
                return;
            }
        }

        // fallback para a main
        activeCam = mainCamera;
    }

    private void TryHandleClick()
    {
        if (activeCam == null)
        {
            Debug.LogWarning("ClickManager: nenhuma câmera ativa definida.");
            return;
        }

        Ray ray = activeCam.ScreenPointToRay(Input.mousePosition);

        // Primeiro tenta uma checagem 'precisa' curta para evitar acertar objetos atrás
        if (Physics.Raycast(ray, out RaycastHit hit, precisionDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            HandleHit(hit);
            if (debugRay) Debug.DrawRay(ray.origin, ray.direction * precisionDistance, Color.green, 1f);
            return;
        }

        // Se nada foi atingido na distância precisa, tenta até a distância máxima
        if (Physics.Raycast(ray, out hit, clickRange, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            HandleHit(hit);
            if (debugRay) Debug.DrawRay(ray.origin, ray.direction * clickRange, Color.yellow, 1f);
            return;
        }

        if (debugRay) Debug.DrawRay(ray.origin, ray.direction * clickRange, Color.red, 1f);
    }

    private void HandleHit(RaycastHit hit)
    {
        // Busca o componente Interactable (sua classe base)
        Interactable interactable = hit.collider.GetComponent<Interactable>();

        if (interactable != null)
        {
            // chame o comportamento que já implementa coleta, puzzles, etc.
            interactable.Interact();
            return;
        }

        // caso queira dar comportamento separado para CollectableItem
        // (não é necessário pois CollectableItem já herda Interactable),
        // mas deixo o exemplo comentado:
        //
        // CollectableItem collectable = hit.collider.GetComponent<CollectableItem>();
        // if (collectable != null)
        // {
        //     collectable.Interact(); // ou collectable.Collect() se existisse
        //     return;
        // }
    }

#if UNITY_EDITOR
    // Desenha um gizmo do ray no editor quando debug ativado
    private void OnDrawGizmosSelected()
    {
        if (!debugRay || activeCam == null) return;

        Ray ray = activeCam.ScreenPointToRay(Input.mousePosition);
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * precisionDistance);
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(ray.origin + ray.direction * precisionDistance, ray.origin + ray.direction * clickRange);
    }
#endif
}
