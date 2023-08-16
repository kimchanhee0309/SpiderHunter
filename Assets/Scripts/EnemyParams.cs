using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //����Ƽ UI�� ������ �� �߰��ϴ� ���ӽ����̽�

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

        //XMLManager���� �Ѱ� ���� �̸��� �ش��ϴ� ���� �Ķ���͸� ã�Ƽ� �ֿ� �Ķ���Ͱ��� �ݿ��� �ְ� ��
        XMLManager.instance.LoadMonsterParamsFromXML(name, this); 

        isDead = false;

        InitHpBarSize();
    }

    void InitHpBarSize()
    {
        //hpBar�� ����� ���� �ڽ��� ������, 1���� ������� �ʱ�ȭ���� �ְ� ��
        hpBar.rectTransform.localScale = new Vector3(1f, 1f, 1f);
    }

    protected override void UpdateAfterReceiveAttack()
    {
        base.UpdateAfterReceiveAttack();

        hpBar.rectTransform.localScale = new Vector3((float)curHp / (float)maxHp, 1f, 1f);
    }
}
