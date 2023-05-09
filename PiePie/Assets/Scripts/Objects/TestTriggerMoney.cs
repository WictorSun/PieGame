using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTriggerMoney : MonoBehaviour
{
    public InventoryManager _IM;
    public bool add;

    private void Start()
    {
        
    }
    private void Update()
    {
        if (add)
        {
            
        }
    }
    public  void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            add = true;
        }
    }
}
