using UnityEngine;

public class InteragirPC : MonoBehaviour
{
    public PcPasswordPuzzle puzzle;

    void OnMouseDown()
    {
        if (puzzle != null)
        {
            Debug.Log("🖱 Clique detectado no PC!");
            puzzle.AbrirComputador();
        }
    }
}
