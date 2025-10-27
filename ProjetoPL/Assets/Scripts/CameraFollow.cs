using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;    //indica o objeto a ser seguido
    private void FixedUpdate()  //atualiza a condi��o com um pequeno atraso
    {
        //muda a posi��o da c�mera de forma suave
        transform.position = Vector2.Lerp(transform.position, player.position, 0.1f);
    }
}
