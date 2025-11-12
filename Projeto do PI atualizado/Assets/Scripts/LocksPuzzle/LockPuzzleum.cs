using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LockPuzzleum : MonoBehaviour
{
    //VARIÁVEIS
    [Header("Informações do puzile")]
    [SerializeField] private string _codigoPuzzle;
    [SerializeField] private UnityEvent _evento;

    [Header("Configurações do Puzzle")]
    [SerializeField] private bool usarCilindros = true;

    [Header("Objetos do puzile")]
    [SerializeField] private GameObject _lockInterativo;
    [SerializeField] private GameObject _lockPuzzle;
    [SerializeField] private GameObject _vc;
    [SerializeField] private GameObject _Lock;

    [Header("Rotação da Sala")]
    [SerializeField] private RotacaoDaSala rotacaoDaSala;
    private bool _puzzlesStarts;
    //private float _rotationStep = 20f;

    //ENTIDADE
    [Header("Sistema da Entidade")]
    public EntitySpawn entitySpawn;

    [Header("Ponto de spawn que causa jumpscare neste puzzle")]
    public Transform puzzleSpawnPoint;

    [Header("Chance de jumpscare (0 = 0%, 1 = 100%)")]
    [Range(0f, 1f)]
    public float jumpscareChance = 0.35f;


    [Header("Cilindros")]
    [SerializeField] private int _cilindrodeAgr = 0;

    //CILINDROS
    private int _cilindro01Step = 0;
    [SerializeField] private GameObject _cilindro01;

    private int _cilindro02Step = 0;
    [SerializeField] private GameObject _cilindro02;

    private int _cilindro03Step = 0;
    [SerializeField] private GameObject _cilindro03;

    private int _cilindro04Step = 0;
    [SerializeField] private GameObject _cilindro04;

    private string _cilindro01Numero = "";
    private string _cilindro02Numero = "";
    private string _cilindro03Numero = "";
    private string _cilindro04Numero = "";

    private Animator anim;


    // Update is called once per frame
    void Update()
    {

        // SE NÃO USA CILINDROS IGNORA TODA A LÓGICA DELES
        if (!usarCilindros)
            return;

        //LÓGICA DOS CILINDROS
        if (_puzzlesStarts == true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                EndPuzzle();
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            _cilindrodeAgr = Mathf.Min(_cilindrodeAgr + 1, 3);
            //audio sound effect
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            _cilindrodeAgr = Mathf.Max(_cilindrodeAgr - 1, 0);
            //audio sound effect
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (_cilindrodeAgr == 0)
            {
                _cilindro01Step = (_cilindro01Step + 1) % 10;
                _cilindro01.transform.localEulerAngles = new Vector3(0f, 0f, _cilindro01Step * 35f);
                ValordeCilindro01();
            }
            if (_cilindrodeAgr == 1)
            {
                _cilindro02Step = (_cilindro02Step + 1) % 10;
                _cilindro02.transform.localEulerAngles = new Vector3(0f, 0f, _cilindro02Step * 35f);
                ValordeCilindro02();
            }
            if (_cilindrodeAgr == 2)
            {
                _cilindro03Step = (_cilindro03Step + 1) % 10;
                _cilindro03.transform.localEulerAngles = new Vector3(0f, 0f, _cilindro03Step * 35f);
                ValordeCilindro03();
            }
            if (_cilindrodeAgr == 3)
            {
                _cilindro04Step = (_cilindro04Step + 1) % 10;
                _cilindro04.transform.localEulerAngles = new Vector3(0f, 0f, _cilindro04Step * 35f);
                ValordeCilindro04();
            }

        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (_cilindrodeAgr == 0)
            {
                _cilindro01Step = (_cilindro01Step - 1 + 10) % 10;
                _cilindro01.transform.localEulerAngles = new Vector3(0f, 0f, _cilindro01Step * 35f);
                ValordeCilindro01();
            }
            if (_cilindrodeAgr == 1)
            {
                _cilindro02Step = (_cilindro02Step - 1 + 10) % 10;
                _cilindro02.transform.localEulerAngles = new Vector3(0f, 0f, _cilindro02Step * 35f);
                ValordeCilindro02();
            }
            if (_cilindrodeAgr == 2)
            {
                _cilindro03Step = (_cilindro03Step - 1 + 10) % 10;
                _cilindro03.transform.localEulerAngles = new Vector3(0f, 0f, _cilindro03Step * 35f);
                ValordeCilindro03();
            }
            if (_cilindrodeAgr == 3)
            {
                _cilindro04Step = (_cilindro04Step - 1 + 10) % 10;
                _cilindro04.transform.localEulerAngles = new Vector3(0f, 0f, _cilindro04Step * 35f);
                ValordeCilindro04();
            }

        }

    }

    public void CodigoCheck()
    {
        string enteredCode = _cilindro01Numero + _cilindro02Numero + _cilindro03Numero + _cilindro04Numero;

        if (enteredCode == _codigoPuzzle)
        {
            StartCoroutine("PuzzleCompleto");
            Debug.Log("O puzzle está completo! código: " + enteredCode);
            //audio sound effect = cadeado abrindo
        }
        else
        {
            Debug.Log("Código incorreto! código: " + _codigoPuzzle + " // o que vc colocou: " + enteredCode);
            //audio sound effect = cadeado emperrado
        }
    }

    public void ValordeCilindro01()
    {
        switch (_cilindro01Step)
        {
            case 0:
                _cilindro01Numero = "7";
                break;
            case 1:
                _cilindro01Numero = "8";
                break;
            case 2:
                _cilindro01Numero = "9";
                break;
            case 3:
                _cilindro01Numero = "0";
                break;
            case 4:
                _cilindro01Numero = "1";
                break;
            case 5:
                _cilindro01Numero = "2";
                break;
            case 6:
                _cilindro01Numero = "3";
                break;
            case 7:
                _cilindro01Numero = "4";
                break;
            case 8:
                _cilindro01Numero = "5";
                break;
            case 9:
                _cilindro01Numero = "6";
                break;
            default:
                _cilindro01Numero = "7";
                break;
        }
        // Debug.Log("O cilindro é " + _cilindrodeAgr + " O número é " + _cilindro01Numero + " e o case é " + _cilindro01Step + " Angulação é " + _cilindro01Step * 35);
        CodigoCheck();
    }
    public void ValordeCilindro02()
    {
        switch (_cilindro02Step)
        {
            case 0:
                _cilindro02Numero = "8";
                break;
            case 1:
                _cilindro02Numero = "9";
                break;
            case 2:
                _cilindro02Numero = "0";
                break;
            case 3:
                _cilindro02Numero = "1";
                break;
            case 4:
                _cilindro02Numero = "2";
                break;
            case 5:
                _cilindro02Numero = "3";
                break;
            case 6:
                _cilindro02Numero = "4";
                break;
            case 7:
                _cilindro02Numero = "5";
                break;
            case 8:
                _cilindro02Numero = "6";
                break;
            case 9:
                _cilindro02Numero = "7";
                break;
            default:
                _cilindro02Numero = "8";
                break;
        }
        //  Debug.Log("O cilindro é " + _cilindrodeAgr + " O número é " + _cilindro02Numero + " e o case é " + _cilindro02Step + " Angulação é " + _cilindro02Step * 35);
        CodigoCheck();
    }
    public void ValordeCilindro03()
    {
        switch (_cilindro03Step)
        {
            case 0:
                _cilindro03Numero = "9";
                break;
            case 1:
                _cilindro03Numero = "0";
                break;
            case 2:
                _cilindro03Numero = "1";
                break;
            case 3:
                _cilindro03Numero = "2";
                break;
            case 4:
                _cilindro03Numero = "3";
                break;
            case 5:
                _cilindro03Numero = "4";
                break;
            case 6:
                _cilindro03Numero = "5";
                break;
            case 7:
                _cilindro03Numero = "6";
                break;
            case 8:
                _cilindro03Numero = "7";
                break;
            case 9:
                _cilindro03Numero = "8";
                break;
            default:
                _cilindro03Numero = "9";
                break;
        }
        //  Debug.Log("O cilindro é " + _cilindrodeAgr + " O número é " + _cilindro03Numero + " e o case é " + _cilindro03Step + " Angulação é " + _cilindro03Step * 35);
        CodigoCheck();
    }
    public void ValordeCilindro04()
    {
        switch (_cilindro04Step)
        {
            case 0:
                _cilindro04Numero = "0";
                break;
            case 1:
                _cilindro04Numero = "1";
                break;
            case 2:
                _cilindro04Numero = "2";
                break;
            case 3:
                _cilindro04Numero = "3";
                break;
            case 4:
                _cilindro04Numero = "4";
                break;
            case 5:
                _cilindro04Numero = "5";
                break;
            case 6:
                _cilindro04Numero = "6";
                break;
            case 7:
                _cilindro04Numero = "7";
                break;
            case 8:
                _cilindro04Numero = "8";
                break;
            case 9:
                _cilindro04Numero = "9";
                break;
            default:
                _cilindro04Numero = "0";
                break;
        }
        //   Debug.Log("O cilindro é " + _cilindrodeAgr + " O número é " + _cilindro04Numero + " e o case é " + _cilindro04Step + " Angulação é " + _cilindro04Step * 35);
        CodigoCheck();
    }

    public void StartPuzzle()
    {
        StartCoroutine(PuzzleStart());
        //Desativa o código LockPuzzleum
        if (rotacaoDaSala.lockPuzzleum != null)
        {
            rotacaoDaSala.lockPuzzleum.enabled = true;
        }
    }

    public void EndPuzzle()
    {
        GameManager.Instance.UnPauseGame();

        //Reativa rotação da sala e restaura paredes normais
        if (rotacaoDaSala != null)
        {
            rotacaoDaSala.enabled = true;
            rotacaoDaSala.RestaurarParedesPadrao();
        }

        _lockInterativo.SetActive(true);
        _lockPuzzle.SetActive(false);
        _vc.SetActive(false);
        _puzzlesStarts = false;
    }

    IEnumerator PuzzleCompleto()
    {
        GameManager.Instance.UnPauseGame();
        //Reativa rotação da sala e restaura paredes normais
        if (rotacaoDaSala != null)
        {
            rotacaoDaSala.enabled = true;
            rotacaoDaSala.RestaurarParedesPadrao();
        }
        anim.SetTrigger("Aberto");
        yield return new WaitForSeconds(1.0f);
        EndPuzzle();

    }

    IEnumerator PuzzleStart()
    {
        //Desabilita rotação da sala e exibe todas as paredes
        if (rotacaoDaSala != null)
        {
            rotacaoDaSala.enabled = false;
            rotacaoDaSala.ForcarTodasAsParedes(true);
        }

        _vc.SetActive(true);
        yield return null;      // Aguarda 1 frame para validar posição
        yield return new WaitForSeconds(0.5f);
        _lockInterativo.SetActive(false);
        _lockPuzzle.SetActive(true);
        _puzzlesStarts = true;

        //JUMPSCARE
        if (entitySpawn != null && puzzleSpawnPoint != null)
        {
            //verifica se a entidade está no mesmo ponto que o puzzle acessado
            if (entitySpawn.IsEntityAt(puzzleSpawnPoint))
            {
                //sorteia o valor para o jumpscare
                float roll = Random.value;
                Debug.Log("[JUMPSCARE CHECK] Sorteio: " + roll);

                //Se o valor for maior que a chance, então acontece o jumpscare
                if (roll <= jumpscareChance)
                {
                    Debug.Log("[JUMPSCARE] Sucesso");
                    yield return entitySpawn.TriggerJumpscare();
                }
                //Se não, sem jumpscare
                else
                {
                    Debug.Log("[JUMPSCARE] Falhou");
                }
            }
            //Se ela estiver em outro puzzle
            else
            {
                Debug.Log("[JUMPSCARE] Entidade não está no spawn deste puzzle.");
            }
        }
        
        // Pequena espera antes do pause
        yield return new WaitForSecondsRealtime(0.1f);

        // Agora pausa o jogo
        GameManager.Instance.PauseGame();
    }
}
