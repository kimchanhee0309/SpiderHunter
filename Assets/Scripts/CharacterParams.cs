using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; //����Ƽ �̺�Ʈ�� ����ϱ� ���ؼ��� ���ӽ����̽��� �߰��ؾ� ��

//CharacterParams�� �÷��̾��� �Ķ���� Ŭ������ ���� �Ķ���� Ŭ������ �θ� Ŭ���� ������ �ϰԵ�
public class CharacterParams : MonoBehaviour
{
    //�ۺ� ������ �ƴ϶� ���������Ƽ, �Ӽ����� ����
    //�ۺ� ������ �Ȱ��� ����� �� ������, ����Ƽ �ν����Ϳ� ����Ǵ� ���� ���� ������ ���� ���� ������Ƽ�� ��ȯ�� ������
    public int level { get; set; }
    public int maxHp { get; set; }
    public int curHp { get; set; }
    public int attackMin { get; set; }
    public int attackMax { get; set; }
    public int defense { get; set; }
    public bool isDead { get; set; }

    [System.NonSerialized]
    public UnityEvent deadEvent = new UnityEvent();

    void Start()
    {
        InitParams();   
    }

    //���߿� CharacterParams Ŭ������ ����� �ڽ�Ŭ��������
    //InitParams �Լ��� �ڽŸ��� ��ɾ �߰��ϱ⸸ �ϸ� �ڵ����� �ʿ��� ��ɾ���� ����
    public virtual void InitParams()
    {

    }

    public int GetRandomAttack()
    {
        int randAttack = Random.Range(attackMin, attackMax+1);
        return randAttack;
    }

    public void SetEnemyAttack(int enemyAttackPower)
    {
        curHp -= enemyAttackPower;
        UpdateAfterReceiveAttack();
    }

    //ĳ���Ͱ� �����κ��� ������ ���� �ڿ� �ڵ����� ����� �Լ��� �����Լ��� ����
    protected virtual void UpdateAfterReceiveAttack()
    {
        print(name + "'s HP: " + curHp);

        if (curHp <= 0)
        {
            curHp = 0;
            isDead = true;
            deadEvent.Invoke();
        }
    }
}
