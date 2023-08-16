using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float rotateSpeed = 180f;

    [System.NonSerialized]
    public int money = 100;
 
    void Start()
    {
        
    }

    public void SetCoinValue(int money)
    {
        this.money = money;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            col.gameObject.GetComponent<PlayerParams>().AddMoney(money);

            SoundManager.instance.PlayPickUpSound();

            //Destroy(gameObject);
            RemoveFromWorld();
        }
    }

    public void RemoveFromWorld()
    {
        gameObject.SetActive(false);
    }

    void Update()
    {
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);   
    }
}
