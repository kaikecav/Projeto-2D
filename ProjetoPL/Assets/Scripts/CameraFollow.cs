using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;    //indica o objeto a ser seguido
    private void FixedUpdate()  //atualiza a condição com um pequeno atraso
    {
        //muda a posição da câmera de forma suave
        transform.position = Vector2.Lerp(transform.position, player.position, 0.1f);
    }
}
