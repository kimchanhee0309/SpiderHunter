using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnObj : MonoBehaviour
{
    List<Transform> spawnPos = new List<Transform>();
    GameObject[] monsters;

    public GameObject monPrefab;
    public int spawnNumber = 1;
    public float respawnDelay = 3f;

    int deadMonsters = 0;

    void Start()
    {
        MakeSpawnPos();
    }

    void MakeSpawnPos()
    {
        //�ڽ��� Ʈ�������� Ȯ���Ͽ� �±� ����(Respawn)�� ã�� ����Ʈ(spawnPos)�� �ִ´�
        foreach(Transform pos in transform)
        {
            if (pos.tag == "Respawn")
            {
                spawnPos.Add(pos);
            }
        }
        if (spawnNumber > spawnPos.Count)
        {
            spawnNumber = spawnPos.Count;
        }

        monsters = new GameObject[spawnNumber];

        MakeMonsters();
    }

    //���������κ��� ���͸� ����� �����ϴ� �Լ�
    void MakeMonsters()
    {
        for(int i = 0; i < spawnNumber; i++)
        {
            GameObject mon = Instantiate(monPrefab, spawnPos[i].position, Quaternion.identity) as GameObject;
            mon.GetComponent<EnemyFSM>().SetRespawnObj(gameObject, i, spawnPos[i].position);
            mon.SetActive(false);

            monsters[i] = mon;

            GameManager.instance.AddNewMonsters(mon);
        }
    }

    public void RemoveMonster(int spawnID)
    {
        //Destroy(monsters[spawnID];
        deadMonsters++;

        monsters[spawnID].SetActive(false);
        print(spawnID + "monster was killed");

        //���� ������ ���ڰ� ���� �迭�� ũ��� ���ٸ� �����ð��Ŀ� ���͸� �ٽ� ����
        if (deadMonsters == monsters.Length)
        {
            StartCoroutine(InitMonsters());
            deadMonsters = 0;
        }
    }

    IEnumerator InitMonsters()
    {
        yield return new WaitForSeconds(respawnDelay);
        GetComponent<SphereCollider>().enabled = true;
    }

    void SpawnMonster()
    {
        for(int i = 0; i < monsters.Length; i++)
        {
            //���Ͱ� ������ �� �� �ʱ�ȭ �� �Լ��� ã�� ����
            monsters[i].GetComponent<EnemyFSM>().AddToWorldAgain();
            monsters[i].SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            SpawnMonster();
            GetComponent<SphereCollider>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
