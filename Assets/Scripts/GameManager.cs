using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    List<GameObject> monsters = new List<GameObject>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    //�ܺο��� ���޵� ���Ͱ� ������ ����Ʈ�� �����ϰ� �ִ� ���Ϳ� ��ġ�ϴ��� ���� üũ
    public void AddNewMonsters(GameObject mon)
    {
        //���ڷ� �Ѿ�� ���Ͱ� ������ ����Ʈ�� �����ϸ� sameExist = true �ƴϸ� false
        bool sameExist = false;
        for(int i = 0; i < monsters.Count; i++)
        {
            if (monsters[i] == mon)
            {
                sameExist = true;

                break;
            }
        }

        if (sameExist == false)
        {
            monsters.Add(mon);
        }
    }

    public void RemoveMonster(GameObject mon)
    {
        foreach(GameObject monster in monsters)
        {
            if (monster == mon)
            {
                monsters.Remove(monster);
                break;
            }
        }
    }

    //���� �÷��̾ Ŭ���� ���͸� ���ø�ũ�� ǥ��
    public void ChangeCurrentTarget(GameObject mon)
    {
        DeselectAllMonsters();
        mon.GetComponent<EnemyFSM>().ShowSelection();
    }

    public void DeselectAllMonsters()
    {
        for(int i = 0; i < monsters.Count; i++)
        {
            monsters[i].GetComponent<EnemyFSM>().HideSelection();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
