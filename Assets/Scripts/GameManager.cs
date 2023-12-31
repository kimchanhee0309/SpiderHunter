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

    //외부에서 전달된 몬스터가 기존에 리스트에 보관하고 있는 몬스터와 일치하는지 여부 체크
    public void AddNewMonsters(GameObject mon)
    {
        //인자로 넘어온 몬스터가 기존의 리스트에 존재하면 sameExist = true 아니면 false
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

    //현재 플레이어가 클릭한 몬스터만 선택마크가 표시
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
