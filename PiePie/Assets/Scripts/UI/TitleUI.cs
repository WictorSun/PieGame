using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUI : MonoBehaviour
{
    bool _outoftrigger;
    [SerializeField] Animator _anim;
    public Collider _trigger;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_outoftrigger)
        {
            StartCoroutine(Play(1f));
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _outoftrigger = true;
        }
    }
    IEnumerator Play(float sec)
    {
        _anim.SetBool("PlayIntro", true);

        yield return new WaitForSeconds(sec);
        _anim.SetBool("PlayIntro", false);

        _trigger.enabled = false;
        _outoftrigger = false;
    }
}
