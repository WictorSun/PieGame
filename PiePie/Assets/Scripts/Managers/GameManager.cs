using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Yarn.Unity;

public class GameManager : MonoBehaviour
{


    PlayerMovementBigActionMap PMBA;

    [SerializeField] private GameObject _bagMainBox;
    [SerializeField] private GameObject _bagLidBox;
    [SerializeField] private GameObject _bagLeftStrapBox;
    [SerializeField] private GameObject _bagRightStrapBox;
    [SerializeField] private InputHandler _IH;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _phone;

    [SerializeField] private Transform _goToPos;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private GameObject Title;
    [SerializeField] private GameObject GameMade;
    [SerializeField] private GameObject PlayerCam;
    [SerializeField] private GameObject IntroCam;
    [SerializeField] private GameObject _oldEventPark;
    [SerializeField] private GameObject _newEventPark;

    private float _timeCounter = 1;
    private float _Counter;

    [Header("Has Items and Stuff")]
    public bool hasBag;
    public bool hasBike;
    public bool _hasJetPack;
    public bool isReadyToClimb;
    public bool _isInDia;
    public bool _start;
    public bool _CamIsActive;
    public static bool _introIsDone;
    public static bool _timeToGetTicket;
    public  bool _climbTicketTICK;
    public static bool _climbTicket;

    [SerializeField] private Animator _playeranim;

    public GameObject _ViritualCamera;

    public int _moveWithBagID;
    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        PMBA = new PlayerMovementBigActionMap();
        _Counter = _timeCounter;
        _moveWithBagID = Animator.StringToHash("moveWithBag");
    }
    private void OnEnable()
    {
        PMBA.Interaction.Enable();
        PMBA.Interaction.Start.performed += PauseMenu;
        PMBA.Interaction.Start.canceled += PauseMenu;
    }

    private void OnDisable()
    {
        PMBA.Interaction.Disable();
        PMBA.Interaction.Start.performed -= PauseMenu;
        PMBA.Interaction.Start.canceled -= PauseMenu;
    }

    private void PauseMenu(InputAction.CallbackContext ctx)
    {

        _start = ctx.ReadValueAsButton();
    }

    public void Update()
    {
        _Counter -= Time.unscaledDeltaTime;

        if (_introIsDone)
        {
            _bagMainBox.SetActive(true);
            _bagLidBox.SetActive(true);
            _bagLeftStrapBox.SetActive(true);
            _bagRightStrapBox.SetActive(true);
            StartCoroutine(IntroOfGame(1f));
        }

        if (hasBag)
        {
            _playeranim.SetBool("HasBag", true);
            _bagMainBox.SetActive(true);
            _bagLidBox.SetActive(true);
            _bagLeftStrapBox.SetActive(true);
            _bagRightStrapBox.SetActive(true);
        }
        if (!_CamIsActive && _start && _Counter <= 0f)
        {


            _CamIsActive = true;
            _ViritualCamera.SetActive(true);
            _start = false;
            _Counter = 1f;
            _phone.SetActive(true);


        }
        else if (_CamIsActive && _start && _Counter <= 0f)
        {
            _CamIsActive = false;
            _ViritualCamera.SetActive(false);
            _start = false;
            _Counter = 1f;
            _phone.SetActive(false);
            //StartCoroutine(GoBackFromPauseMenu(.2f));

        }
        if (_timeToGetTicket)
        {
            _oldEventPark.SetActive(false);
            _newEventPark.SetActive(true);
        }
        if (_climbTicketTICK)
        {
            _climbTicket = true;
        }
    }
    [YarnCommand("IntroDone")]
    public static void GetBag()
    {
        _introIsDone = true;
    }
    [YarnCommand("GetTicket")]
    public static void GetTheTicket()
    {
        _timeToGetTicket = true;
    }
    [YarnFunction("TicketBool")]
    public static bool GetTic()
    {
        return _timeToGetTicket;
    }
    
 
    [YarnFunction("HasTicket")]
    public static bool _hasTheTicket()
    {
        return _climbTicket;
    }

   IEnumerator IntroOfGame(float sec)
    {
        _playeranim.SetBool("HasBag", true);
        _playeranim.SetFloat(_moveWithBagID, .6f, .1f, Time.deltaTime);

        
        _player.transform.position = Vector3.Lerp(_player.transform.position, _goToPos.position,  Time.deltaTime);
        yield return new WaitForSeconds(sec);
        _playeranim.SetFloat(_moveWithBagID, 0f, .1f, Time.deltaTime);
        yield return new WaitForSeconds(sec);
        
        IntroCam.SetActive(false);
        PlayerCam.SetActive(true);
        _introIsDone = false;
        hasBag = true;
    }
}

