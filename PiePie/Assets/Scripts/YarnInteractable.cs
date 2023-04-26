using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;


public class YarnInteractable : MonoBehaviour {
    // internal properties exposed to editor
    [SerializeField] private string conversationStartNode;

    // internal properties not exposed to editor
    public DialogueRunner dialogueRunner;
    public  GameManager _GM; 
    private Light lightIndicatorObject = null;
    private bool interactable = true;
    private bool isCurrentConversation = false;
    private float defaultIndicatorIntensity;
    private float _timeleft = 1f;
    private float _counter;

    public void Start() {
        //dialogueRunner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        dialogueRunner.onDialogueComplete.AddListener(EndConversation);
        lightIndicatorObject = GetComponentInChildren<Light>();
        // get starter intensity of light then
        // if we're using it as an indicator => hide it 
        if (lightIndicatorObject != null) {
            defaultIndicatorIntensity = lightIndicatorObject.intensity;
            lightIndicatorObject.intensity = 0;
        }
        _counter = _timeleft;
    }

    public void OnMouseDown() {
        if (interactable && !dialogueRunner.IsDialogueRunning && _counter <= 0f) {
            StartConversation();
        }
    }

    private void StartConversation() {
        Debug.Log($"Started conversation with {name}.");
        isCurrentConversation = true;
        _GM._isInDia = true;
        // if (lightIndicatorObject != null) {
        //     lightIndicatorObject.intensity = defaultIndicatorIntensity;
        // }
        dialogueRunner.StartDialogue(conversationStartNode);
        //_GM._isInDia = true;
    }

    private void EndConversation() {
        if (isCurrentConversation) {
            // if (lightIndicatorObject != null) {
            //     lightIndicatorObject.intensity = 0;
            // }
            isCurrentConversation = false;
            _GM._isInDia = false;
            Debug.Log("Ended coversation");
            _counter = _timeleft;
        }
    }

    private void Update()
    {
       
        _counter -= Time.deltaTime;
    }

    [YarnCommand("disable")]
    public void DisableConversation() {
        interactable = false;
        Debug.Log("Conversation off");
    }
}