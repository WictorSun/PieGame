using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerDialog : MonoBehaviour
{
    [SerializeField] private Transform _detectNPC;
    [SerializeField] private LayerMask _NPC;
    [SerializeField] private InputHandler _IH;
    private NPCTriggerDialog _NpcTrigger;
    public bool _nojump;

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
        RaycastHit hit;
        if(Physics.Raycast(_detectNPC.transform.position, _detectNPC.transform.forward, out hit, 10f, _NPC))
        {
            if(hit.collider.gameObject.GetComponent<YarnInteractable>() != null && _IH.Jump)
            {
                hit.collider.gameObject.GetComponent<YarnInteractable>().OnMouseDown();
                _nojump = true;
            }
            else
            {
                _nojump = false;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(_detectNPC.transform.position, Vector3.forward * 10f);
    }
}
