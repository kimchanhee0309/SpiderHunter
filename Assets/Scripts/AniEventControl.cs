using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AniEventControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SendAttackEnemy()
    {
        transform.parent.gameObject.SendMessage("AttackCalculate");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
