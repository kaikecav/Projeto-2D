using UnityEngine;

public class RotacaoDaSala : MonoBehaviour
{
    [Header("Configurações de Rotação")]        //cria um "título no menu da Unity
    [SerializeField] float dragRotationSpeed = 100f;     // Velocidade da movimentação com o mouse
    [SerializeField] float smoothRotationSpeed = 300f;   // Velocidade da rotação com as setas

    [Header("Referências das Paredes")]     //cria um "título no menu da Unity
    public GameObject Parede1;      //determina qual parede vai ser ativado a consição de aparecer ou sumir
    public GameObject Parede2;
    public GameObject Parede3;
    public GameObject Parede4;

    [Header("Puzzle")]
    [SerializeField] public LockPuzzleum lockPuzzleum;
    [SerializeField] public MaletaPuzzle maletaPuzzle;
    [SerializeField] public MaletaPuzzle PuzzleMaberta;
    [SerializeField] public PickupPuzzle pickupPuzzle;
    

    private bool dragging = false;      //indica se o mouse está sendo movimentado/arrastado
    private bool isRotating = false;        //indica se o objeto está rodando
    private Vector3 lastMousePosition;      //grava a última posição do mouse

    private Quaternion targetRotation;      //determia a rotação que o objeto deve atingir, com o Quaternion garantindo que a ângulação seja mais livre
    private float currentSnapAngle;     //demonstra o ângulo atual (arredondando para os múltiplos do 90º)
    private float nextSnapAngle;        //determina o próxima ângulo (mantendo os múltiplos de 90º)


    void Start()        //Determina tudo o que vai ocorrer no ínicio do jogo
    {
        currentSnapAngle = 0f;      //determina o ângulo inicial da sala
        targetRotation = Quaternion.Euler(0, currentSnapAngle, 0); //determina o tipo de rotação da sala, sendo feita através de ângulos
        transform.rotation = targetRotation;    //atualiza a rotação conforme o próximo ângulo
        nextSnapAngle = currentSnapAngle;       //determina que o ângulo atual vai ser o próximo ângulo

        AtualizarParedes();     //Atualiza a função com estas condições
    }

    void Update()       //determina o que vai acontecer a cada frame
    {
        // Bloqueia toda a lógica se o script estiver desabilitado externamente
        if (!enabled)
            return;

        //MOVIMENTAÇÃO DO MOUSE
        if (Input.GetMouseButtonDown(0) && !isRotating)     //quando o botão esquerdo do mouse é pressionado sem estar rodando o objeto
        {
            dragging = true;        //ativa a função de arrastar o mouse
            lastMousePosition = Input.mousePosition;

            float rounded = Mathf.Round(transform.eulerAngles.y / 90f) * 90f;   //garante que o ângulo vai ser arredondado para o mais próximo mútiplo de 90º
            currentSnapAngle = rounded;
            nextSnapAngle = currentSnapAngle;
        }

        if (Input.GetMouseButtonUp(0) && dragging && !isRotating)       //quando o botão esquerdo do mouse é para de ser pressionado cancela todas as funções
        {
            dragging = false;
            targetRotation = Quaternion.Euler(0, nextSnapAngle, 0);
            isRotating = true;
        }

        // MOVIMENTAÇÃO SUAVE DE ÂNGULO EM ÂNGULO
        if (isRotating)
        {
            transform.rotation = Quaternion.RotateTowards      //limita quantos graus vai ser rodado por frame
                (
                transform.rotation,     //determina a rotação em si do objeto
                targetRotation,     //determina o objetivo final da rotação
                smoothRotationSpeed * Time.deltaTime        //define a velocidade (em graus por segundo) da rotação, utilizando o Time.deltaTime para ocorrer a cada frame do computador
                );

            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)        //calcula o ângulo de diferença (em graus) entre duas rotações.
            {
                transform.rotation = targetRotation;
                isRotating = false;     //finaliza a rotação
            }

            AtualizarParedes();     //Atualiza a função com estas condições
            return; //garantia de que somente este script seja usado quando for ativado
        }

        //FUNCIONAMENTO DO ARRASTAR DO MOUSE
        if (dragging)
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;        //calcula quanto o mouse se moveu no eixo X desde o último frame.
            float rotationAmount = -delta.x * dragRotationSpeed * Time.deltaTime;       //converte isso em um valor de rotação.

            float currentY = transform.eulerAngles.y;       //obtêm o ângulo atual de y

            // Limita a rotação entre +-90°
            float minAngle = (currentSnapAngle - 90f + 360f) % 360f;
            float maxAngle = (currentSnapAngle + 90f) % 360f;

            //garante que o movimento não ultrapasse o limite de 90º por giro
            float proposedY = (currentY + rotationAmount + 360f) % 360f;
            bool allowRotation;

            if (minAngle < maxAngle)
                allowRotation = proposedY >= minAngle && proposedY <= maxAngle;
            else
                allowRotation = proposedY >= minAngle || proposedY <= maxAngle;

            if (allowRotation)
            {
                transform.Rotate(0f, rotationAmount, 0f);
                lastMousePosition = Input.mousePosition;        //determina o último posicionamento do mouse enquanto estava rodando o objeto

                //determina que o ângulo escolhido para rodar será o mais próximo do múltiplo de 90º, sendo um movimento maior que 45º vai pro próximo ângulo e menor volta para o que estava
                float angleDiff = Mathf.DeltaAngle(currentSnapAngle, transform.eulerAngles.y);
                if (Mathf.Abs(angleDiff) >= 45f)
                {
                    nextSnapAngle = (angleDiff > 0)
                        ? (currentSnapAngle + 90f) % 360f
                        : (currentSnapAngle - 90f + 360f) % 360f;
                }
            }

            AtualizarParedes();     //Atualiza a função com estas condições
        }

        //ROTAÇÃO DA SALA ATRAVÉS DAS TECLAS/SETAS
        if (Input.GetKeyDown(KeyCode.A) && !isRotating)
        {
            currentSnapAngle = Mathf.Round(transform.eulerAngles.y / 90f) * 90f;
            nextSnapAngle = (currentSnapAngle - 90f + 360f) % 360f;
            targetRotation = Quaternion.Euler(0, nextSnapAngle, 0);
            isRotating = true;
        }
        else if (Input.GetKeyDown(KeyCode.D) && !isRotating)
        {
            currentSnapAngle = Mathf.Round(transform.eulerAngles.y / 90f) * 90f;
            nextSnapAngle = (currentSnapAngle + 90f) % 360f;
            targetRotation = Quaternion.Euler(0, nextSnapAngle, 0);
            isRotating = true;
        }

        AtualizarParedes();     //Atualiza a função com estas condições

        //Desativa o código LockPuzzleum
        if (lockPuzzleum != null)
        {
            lockPuzzleum.enabled = false;
        }
    }

    //VISIBILIDADE DAS PAREDES QUANDO DÁ ZOOM

    //Função que obriga todas as paredes a ficarem ativas
    public void ForcarTodasAsParedes(bool estado)
    {
        Parede1.SetActive(estado);
        Parede2.SetActive(estado);
        Parede3.SetActive(estado);
        Parede4.SetActive(estado);
    }

    //Função que obriga todas as paredes voltarem a configuração padrão
    public void RestaurarParedesPadrao()
    {
        AtualizarParedes();
    }

    //VISIBILIDADE DAS PAREDES NORMAL
    void AtualizarParedes()     //função para ativar ou desativar as paredes de acordo com a angulação
    {
        float angulo = transform.eulerAngles.y;


        angulo = (angulo + 360f) % 360f;    // Normaliza o ângulo entre 0 – 360

        // INTERVALOS 

        // 0° = entre 315° e 45°
        if (angulo >= 315f || angulo < 45f)
        {
            Parede1.SetActive(true);
            Parede2.SetActive(false);
            Parede3.SetActive(false);
            Parede4.SetActive(true);
        }

        // 90° = entre 45° e 135°
        else if (angulo >= 45f && angulo < 135f)
        {
            Parede1.SetActive(true);
            Parede2.SetActive(false);
            Parede3.SetActive(true);
            Parede4.SetActive(false);
        }

        // 180° = entre 135° e 225°
        else if (angulo >= 135f && angulo < 225f)
        {
            Parede1.SetActive(false);
            Parede2.SetActive(true);
            Parede3.SetActive(true);
            Parede4.SetActive(false);
        }

        // 270° = entre 225° e 315°
        else if (angulo >= 225f && angulo < 315f)
        {
            Parede1.SetActive(false);
            Parede2.SetActive(true);
            Parede3.SetActive(false);
            Parede4.SetActive(true);
        }
    }
}

