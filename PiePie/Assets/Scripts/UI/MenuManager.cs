using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    [SerializeField] private GameManager _GM;
    [SerializeField] private InputHandler _IH;

    //GameObjects not ui
    [SerializeField] private GameObject _DiaRunner;
    [SerializeField] private GameObject _camera1;
    [SerializeField] private GameObject _camera2;
    [SerializeField] private GameObject _camera3;
    [SerializeField] private GameObject _camera4;

    //GameObjects UI
    [SerializeField] private GameObject _mainMenuCanvas;
    [SerializeField] private GameObject _optionsButtons;
    [SerializeField] private GameObject _options;
    [SerializeField] private GameObject _controlImage;
    [SerializeField] private GameObject _musicPanel;
    [SerializeField] private GameObject _creditPanel;

    
    
    //Bools
    public bool _startgame;
    private bool _isInMainMenu;

    // Start is called before the first frame update
    void Awake()
    {
        _isInMainMenu = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!_isInMainMenu && _IH.CanleClimbing)
        {
            _isInMainMenu = true;
            _mainMenuCanvas.SetActive(true);
            
            _mainMenuCanvas.SetActive(true);
            _optionsButtons.SetActive(false);
            _options.SetActive(false);
            _controlImage.SetActive(false);

        }
        if (!_startgame)
        {
            _camera3.SetActive(false);
            _camera4.SetActive(false);
        }
    }
    public void StartGame()
    {
        StartCoroutine(StartinGame(1f));
        _mainMenuCanvas.SetActive(false);
        _startgame = true;
    }
    public void Options()
    {
        _mainMenuCanvas.SetActive(false);
       
        _optionsButtons.SetActive(true);
        _options.SetActive(true);
        _controlImage.SetActive(true);
        _isInMainMenu = false;
    }
    public void QuitGame()
    {

    }
    public void ControlButton()
    {
        _controlImage.SetActive(true);
        _musicPanel.SetActive(false);
        _creditPanel.SetActive(false);
    }
    public void AudioButton()
    {
        _controlImage.SetActive(false);
        _musicPanel.SetActive(true);
        _creditPanel.SetActive(false);
    }
    public void CreditButton()
    {
        _controlImage.SetActive(false);
        _musicPanel.SetActive(false);
        _creditPanel.SetActive(true);
    }
    IEnumerator StartinGame(float sec)
    {
        _camera1.SetActive(false);
        _camera2.SetActive(true);
        yield return new WaitForSeconds(sec);
        _DiaRunner.SetActive(true);
    }
}
