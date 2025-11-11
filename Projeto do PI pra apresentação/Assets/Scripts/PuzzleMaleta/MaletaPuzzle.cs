using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MaletaPuzzle : MonoBehaviour
{
    [Header("Informações do puzile")]
    [SerializeField] private string _codigoPuzzle;
    [SerializeField] private UnityEvent _evento;

    [Header("Objetos do puzile")]
    [SerializeField] private GameObject _maletaInterativo;
    [SerializeField] private GameObject _maletaPuzzle;
    [SerializeField] private GameObject _vc;
    [SerializeField] private GameObject _maleta;

    [Header("Configurações do Puzzle")]
    [SerializeField] private bool usarCilindros = true;
    [SerializeField] private RotacaoDaSala rotacaoDaSala;

    //ENTIDADE
    [Header("Sistema da Entidade")]
    public EntitySpawn entitySpawn;

    [Header("Ponto de spawn que causa jumpscare neste puzzle")]
    public Transform puzzleSpawnPoint;

    [Header("Chance de jumpscare (0 = 0%, 1 = 100%)")]
    [Range(0f, 1f)]
    public float jumpscareChance = 0.35f;


    private bool _puzzlesStarts;

    [Header("Cilindros")]
    //private float _rotationStep = 20f;
    [SerializeField] private int _cilindrodeAgr = 0;

    private int _cilindro01Step = 0;
    [SerializeField] private GameObject _cilindro01;

    private int _cilindro02Step = 0;
    [SerializeField] private GameObject _cilindro02;

    private int _cilindro03Step = 0;
    [SerializeField] private GameObject _cilindro03;

    

    private string _cilindro01Numer = "";
    private string _cilindro02Numer = "";
    private string _cilindro03Numer = "";
    

    private Animator anim;

    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

        if (_puzzlesStarts == true)
        {
            if (Input.GetMouseButtonDown(1))
            {
                EndPuzzle();
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            _cilindrodeAgr = Mathf.Min(_cilindrodeAgr + 1, 2);
            //audio sound effect
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            _cilindrodeAgr = Mathf.Max(_cilindrodeAgr - 1, 0);
            //audio sound effect
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (_cilindrodeAgr == 2)
            {
                _cilindro01Step = (_cilindro01Step + 1) % 10;
                _cilindro01.transform.localEulerAngles = new Vector3(_cilindro01Step * 35f, 0f, 0f);
                ValordeCilindro01();
            }
            if (_cilindrodeAgr == 1)
            {
                _cilindro02Step = (_cilindro02Step + 1) % 10;
                _cilindro02.transform.localEulerAngles = new Vector3(_cilindro02Step * 35f, 0f, 0f);
                ValordeCilindro02();
            }
            if (_cilindrodeAgr == 0)
            {
                _cilindro03Step = (_cilindro03Step + 1) % 10;
                _cilindro03.transform.localEulerAngles = new Vector3(_cilindro03Step * 35f, 0f, 0f);
                ValordeCilindro03();
            }


        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (_cilindrodeAgr == 2)
            {
                _cilindro01Step = (_cilindro01Step - 1 + 10) % 10;
                _cilindro01.transform.localEulerAngles = new Vector3(_cilindro01Step * 35f, 0f, 0f);
                ValordeCilindro01();
            }
            if (_cilindrodeAgr == 1)
            {
                _cilindro02Step = (_cilindro02Step - 1 + 10) % 10;
                _cilindro02.transform.localEulerAngles = new Vector3(_cilindro02Step * 35f, 0f, 0f);
                ValordeCilindro02();
            }
            if (_cilindrodeAgr == 0)
            {
                _cilindro03Step = (_cilindro03Step - 1 + 10) % 10;
                _cilindro03.transform.localEulerAngles = new Vector3(_cilindro03Step * 35f, 0f, 0f);
                ValordeCilindro03();
            }

        }

    }

    public void CodigoCheck()
    {
        string enteredCode = _cilindro03Numer + _cilindro02Numer + _cilindro01Numer;

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
                _cilindro01Numer = "0";
                break;
            case 1:
                _cilindro01Numer = "9";
                break;
            case 2:
                _cilindro01Numer = "8";
                break;
            case 3:
                _cilindro01Numer = "7";
                break;  
            case 4:
                _cilindro01Numer = "6";
                break;
            case 5:
                _cilindro01Numer = "5";
                break;
            case 6:
                _cilindro01Numer = "4";
                break;
            case 7:
                _cilindro01Numer = "3";
                break;  
            case 8:
                _cilindro01Numer = "2";
                break;
            case 9:
                _cilindro01Numer = "1";
                break;
            default:
                _cilindro01Numer = "0";
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
                _cilindro02Numer = "0";
                break;
            case 1:
                _cilindro02Numer = "9";
                break;
            case 2:
                _cilindro02Numer = "8";
                break;
            case 3:
                _cilindro02Numer = "7";
                break;
            case 4:
                _cilindro02Numer = "6";
                break;
            case 5:
                _cilindro02Numer = "5";
                break;
            case 6:
                _cilindro02Numer = "4";
                break;  
            case 7:
                _cilindro02Numer = "3";
                break;
            case 8: 
                _cilindro02Numer = "2";
                break;
            case 9:
                _cilindro02Numer = "1";
                break;
            default:
                _cilindro02Numer = "0";
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
                _cilindro03Numer = "0";
                break;
            case 1:
                _cilindro03Numer = "9";
                break;
            case 2:
                _cilindro03Numer = "8";
                break;
            case 3:
                _cilindro03Numer = "7";
                break;
            case 4:
                _cilindro03Numer = "6";
                break;
            case 5:
                _cilindro03Numer = "5";
                break;
            case 6:
                _cilindro03Numer = "4";
                break;
            case 7:
                _cilindro03Numer = "3";
                break;
            case 8:
                _cilindro03Numer = "2";
                break;
            case 9:
                _cilindro03Numer = "1";
                break;
            default:
                _cilindro03Numer = "0";
                break;
        }
        //  Debug.Log("O cilindro é " + _cilindrodeAgr + " O número é " + _cilindro03Numero + " e o case é " + _cilindro03Step + " Angulação é " + _cilindro03Step * 35);
        CodigoCheck();
    }

    public void StartPuzzle()
    {
        StartCoroutine(PuzzleStart());
        if (rotacaoDaSala.maletaPuzzle != null)
        {
            rotacaoDaSala.maletaPuzzle.enabled = true;
        }
    }

    public void EndPuzzle()
    {
        GameManager.Instance.UnPauseGame();

        if (rotacaoDaSala != null)
        {
            rotacaoDaSala.enabled = true;
            rotacaoDaSala.RestaurarParedesPadrao();
        }

        _maletaInterativo.SetActive(true);
        _maletaPuzzle.SetActive(false);
        _vc.SetActive(false);
        _puzzlesStarts = false;
    }

    IEnumerator PuzzleCompleto()
    {
        GameManager.Instance.UnPauseGame();

        if (rotacaoDaSala != null)
        {
            rotacaoDaSala.enabled = true;
            rotacaoDaSala.RestaurarParedesPadrao();
        }

        anim.SetTrigger("AbertoMaleta");
        yield return new WaitForSeconds(1.0f);
        _maletaInterativo.SetActive(false);
        _maletaPuzzle.SetActive(false);
        _vc.SetActive(false);
        _puzzlesStarts = false;

    }

    IEnumerator PuzzleStart()
    {
        if (rotacaoDaSala != null)
        {
            rotacaoDaSala.enabled = false;
            rotacaoDaSala.ForcarTodasAsParedes(true);
        }

        _vc.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        yield return null;      // Aguarda 1 frame para validar posição
        _maletaInterativo.SetActive(false);
        _maletaPuzzle.SetActive(true);
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
