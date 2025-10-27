using UnityEngine;

public class InimigoSimples : MonoBehaviour
{
    public float velocidade = 2f;
    private bool indoDireita = true;

    void Update()
    {
        // Movimento constante
        float direcao = indoDireita ? 1f : -1f;
        transform.Translate(Vector2.right * direcao * velocidade * Time.deltaTime);
    }

    // Quando colide com algo (bloco invisível, parede etc.)
    void OnCollisionEnter2D(Collision2D colisao)
    {
        if (colisao.gameObject.CompareTag("Limite"))
        {
            // Inverte a direção
            indoDireita = !indoDireita;

            // Espelha o sprite (vira)
            Vector3 escala = transform.localScale;
            escala.x *= -1;
            transform.localScale = escala;
        }
       
    }
}

