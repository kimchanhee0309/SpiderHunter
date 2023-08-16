using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParams : CharacterParams
{
    public string name { get; set; }
    public int curExp { get; set; }
    public int expToNextLevel { get; set; }
    public int money { get; set; }

    public override void InitParams()
    {
        name = "name";
        level = 1;
        maxHp = 100;
        curHp = maxHp;
        attackMin = 5;
        attackMax = 8;
        defense = 1;

        curExp = 0;
        expToNextLevel = 100 * level;
        money = 0;

        isDead = false;

        //초기화할 때 헤드업 디스플레이에 플레이어의 이름과 기타 정보들이 제대로 표시되도록 함
        UIManager.instance.UpdatePlayerUI(this);
    }

    protected override void UpdateAfterReceiveAttack()
    {
        base.UpdateAfterReceiveAttack();

        UIManager.instance.UpdatePlayerUI(this);
    }

    public void AddMoney(int money)
    {
        this.money += money;
        UIManager.instance.UpdatePlayerUI(this);
    }
}
