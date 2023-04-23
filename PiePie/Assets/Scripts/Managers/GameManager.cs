using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    

   
   
    [SerializeField] private GameObject _bagMainBox;
    [SerializeField] private GameObject _bagLidBox;
    [SerializeField] private GameObject _bagLeftStrapBox;
    [SerializeField] private GameObject _bagRightStrapBox;

    [Header("Has Items and Stuff")]
    public bool hasBag;
    public bool hasBike;
    public bool _hasJetPack;
    public bool isReadyToClimb;

    [SerializeField] private Animator _playeranim;

  

    private void Awake()
    {
       
    }
    private void OnEnable()
    {
        

    }

    private void OnDisable()
    {
      
       
    }

   

   

    private void Update()
    {
        if (hasBag)
        {
            _playeranim.SetBool("HasBag", true);
            _bagMainBox.SetActive(true);
            _bagLidBox.SetActive(true);
            _bagLeftStrapBox.SetActive(true);
            _bagRightStrapBox.SetActive(true);
        }
    }
}
