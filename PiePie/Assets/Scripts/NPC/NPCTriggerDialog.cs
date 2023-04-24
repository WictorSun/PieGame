using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class NPCTriggerDialog : MonoBehaviour
{
    public bool _startDialog;
    [SerializeField] private DialogueRunner _diaRunner;
    [SerializeField] private string _nameOfDialog;
    [SerializeField] private string _stopString;
    // Start is called before the first frame update
    void Start()
    {
        _startDialog = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_startDialog)
        {
            _diaRunner.StartDialogue(_nameOfDialog);
            _startDialog = false;

        }
    }
   
}
