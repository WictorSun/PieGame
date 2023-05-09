using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerDialog : MonoBehaviour
{
    [SerializeField] private Transform _detectNPC;
    [SerializeField] private LayerMask _NPC;
    [SerializeField] private InputHandler _IH;
    [SerializeField] private GameObject _uiA;
    private NPCTriggerDialog _NpcTrigger;
    [SerializeField] private Animator _interactAnim;
    public bool _nojump;
    public bool _noInt;
    [SerializeField] GameManager _GM;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public bool _isFacingNPC()
    {
        return Physics.CheckSphere(_detectNPC.position, 2f, _NPC);

    }

    // Update is called once per frame
    void Update()
    {
        //StartCoroutine(InteractGo(2f));
        RaycastHit hit;
        if(Physics.Raycast(_detectNPC.transform.position, _detectNPC.transform.forward, out hit, 10f, _NPC))
        {
            
            //if (hit.collider.gameObject.GetComponent<YarnInteractable>() != null)
            //{
            //    _nojump = true;
                
                
            //}
            if(hit.collider.gameObject.GetComponent<YarnInteractable>() != null && _IH.Interact)
            {
                hit.collider.gameObject.GetComponent<YarnInteractable>().OnMouseDown();
                _nojump = true;
                _interactAnim.SetBool("Play", true);
            }
            else
            {
                _nojump = false;
                _interactAnim.SetBool("Play", false);
            }
        }
        if (_isFacingNPC())
        {
            _interactAnim.SetBool("Play", true);
        }

        if (!_isFacingNPC())
        {
            _interactAnim.SetBool("Play", false);
        }
        if (_GM._isInDia)
        {
            _interactAnim.SetBool("Play", false);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(_detectNPC.transform.position, Vector3.forward * 10f);
    }
    
}
