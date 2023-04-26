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

    [SerializeField] private GameObject _phone;

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
    [SerializeField] private Animator _playeranim;

    public GameObject _ViritualCamera;
  

    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
        PMBA = new PlayerMovementBigActionMap();
        _Counter = _timeCounter;
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

    public  void Update()
    {
        _Counter -= Time.unscaledDeltaTime;
        
        if (hasBag)
        {
            _playeranim.SetBool("HasBag", true);
            _bagMainBox.SetActive(true);
            _bagLidBox.SetActive(true);
            _bagLeftStrapBox.SetActive(true);
            _bagRightStrapBox.SetActive(true);
        }
        if (!_CamIsActive && _start && _Counter <=0f)
        {


            _CamIsActive = true;
            _ViritualCamera.SetActive(true);
            _start = false;
            _Counter = 1f;
            _phone.SetActive(true);


        }
        else if(_CamIsActive && _start && _Counter <= 0f)
        {
            _CamIsActive = false;
            _ViritualCamera.SetActive(false);
            _start = false;
            _Counter = 1f;
            _phone.SetActive(false);
            //StartCoroutine(GoBackFromPauseMenu(.2f));

        }
       
    }
    
   
}

