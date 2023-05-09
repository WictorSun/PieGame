using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutUI : MonoBehaviour
{
    [SerializeField] private GameObject _tutUI;
    [SerializeField] private Animator _tutAnim;
    private bool _endAnim;

    private void Update()
    {
        if (_endAnim)
        {
            StartCoroutine(EndTut(2f));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _tutUI.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _endAnim = true;
    }
    IEnumerator EndTut(float sec)
    {
        _tutAnim.SetBool("Play", false);
        yield return new WaitForSeconds(sec);
        _tutUI.SetActive(false);
    }
}
