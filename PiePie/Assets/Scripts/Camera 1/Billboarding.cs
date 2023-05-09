using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{
    Vector3 _cameraDir;
    [SerializeField] private Animator _spriteAnim;
    [SerializeField] private Camera _cam;

    public bool _startShowingTrigger;

    [SerializeField] InputHandler _IH;

    [SerializeField] private GameObject _signCamera;
    [SerializeField] private GameObject _playerCamera;
    [SerializeField] private GameManager _GM;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _cameraDir = _cam.transform.position;
        _cameraDir.y = 0;

        transform.rotation = Quaternion.LookRotation(_cameraDir);

        if (_startShowingTrigger)
        {
            
            _spriteAnim.SetBool("Play", true);
        }
        if (!_startShowingTrigger)
        {
            
            _spriteAnim.SetBool("Play", false);
        }
        if(_startShowingTrigger && _IH.Interact && !_GM._isInDia)
        {
            _GM._isInDia = true;
            _playerCamera.SetActive(false);
            _signCamera.SetActive(true);
            _spriteAnim.SetBool("Play", false);
        }
        else if( _IH.CanleClimbing)
        {
            _GM._isInDia = false;
            _spriteAnim.SetBool("Play", true);
            _playerCamera.SetActive(true);
            _signCamera.SetActive(false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _startShowingTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _startShowingTrigger = false;
        }
    }
}
