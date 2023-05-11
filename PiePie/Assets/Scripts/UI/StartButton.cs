using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    public Button _startButton;
    public bool _activate;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartBut(2f));
    }

    // Update is called once per frame
    void Update()
    {
        if (_activate)
        {
            _startButton.Select();
        }
    }
    IEnumerator StartBut(float sec)
    {
        _activate = true;
        yield return new WaitForSeconds(sec);
        _activate = false;
    }
}
