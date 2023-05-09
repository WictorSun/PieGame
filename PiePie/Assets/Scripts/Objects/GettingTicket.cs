using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class GettingTicket : MonoBehaviour
{
    [SerializeField] private GameObject _oldTrigger;
    [SerializeField] private Animator _TicketAnim;
    [SerializeField] private GameObject _switcher2;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _newTransform;
    [SerializeField] private GameObject _oldCamera;
    [SerializeField] private GameObject _playerCamera;
    [SerializeField] private YarnInteractable _YI;
    public  GameManager _GM;

    public static bool _hasTicket;

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _GM._climbTicketTICK = true;
            _hasTicket = true;
            StartCoroutine(PlayTriggerAnim(3f));
            
        }
    }
    [YarnFunction("GotTicket")]
    public static bool hasTicket()
    {
        return _hasTicket;
    }
    IEnumerator PlayTriggerAnim(float sec)
    {
        _oldTrigger.SetActive(false);
        
        _TicketAnim.SetBool("Go", true);
        yield return new WaitForSeconds(sec);

        _switcher2.SetActive(true);
        StartCoroutine(Moveplayer(1f));
        _TicketAnim.SetBool("Go", false);
        yield return new WaitForSeconds(sec);
        _switcher2.SetActive(false);
        _YI.OnMouseDown();
    }
    IEnumerator Moveplayer(float sec)
    {

        yield return new WaitForSeconds(sec);
        _player.transform.position = _newTransform.transform.position;
        _oldCamera.SetActive(false);
        _playerCamera.SetActive(true);
    }
}
