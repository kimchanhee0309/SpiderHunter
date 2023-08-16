using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //유니티 UI를 생성할 때 추가하는 네임스페이스

public class EnemyParams : CharacterParams
{
    public string name;
    public int exp { get; set; }
    public int rewardMoney { get; set; }
    public Image hpBar;

    public override void InitParams()
    {
        /*
           name = "Monster";
           level = 1;
           maxHp = 50;
           curHp = maxHp;
           attackMin = 2;
           attackMax = 5;
           defense = 1;

           exp = 10;
           rewardMoney = Random.Range(10, 31);
        */

        //XMLManager에서 넘겨 받은 이름을 해당하는 몬스터 파라미터를 찾아서 주요 파라미터값을 반영해 주게 됨
        XMLManager.instance.LoadMonsterParamsFromXML(name, this); 

        isDead = false;

        InitHpBarSize();
    }

    void InitHpBarSize()
    {
        //hpBar의 사이즈를 원래 자신의 사이즈, 1배의 사이즈로 초기화시켜 주게 됨
        hpBar.rectTransform.localScale = new Vector3(1f, 1f, 1f);
    }

    protected override void UpdateAfterReceiveAttack()
    {
        base.UpdateAfterReceiveAttack();

        hpBar.rectTransform.localScale = new Vector3((float)curHp / (float)maxHp, 1f, 1f);
    }
}
