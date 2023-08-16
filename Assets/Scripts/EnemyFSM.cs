using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public enum State
    {
        Idle, //����
        Chase, //����
        Attack, //����
        Dead, //���
        NoState //�ƹ� �ϵ� ���� ����
    }

    public State currentState = State.Idle;

    EnemyParams myParams;

    EnemyAni myAni;

    Transform player;

    PlayerParams playerParams;

    CharacterController controller;

    float chaseDistance = 5f; //�÷��̾ ���� ���Ͱ� ������ ������ �Ÿ�
    float attackDistance = 2.5f; //�÷��̾ �������� ������ �Ǹ� ������ ����
    float reChaseDistance = 3f; //�÷��̾ ������ ��� �󸶳� �������� �ٽ� ����

    float rotAnglePerSecond = 360f; //�ʴ� ȸ�� ����
    float moveSpeed = 1.3f; //������ �̵� �ӵ�

    float attackDelay = 2f;
    float attackTimer = 0f;

    //public ParticleSystem hitEffect;

    public GameObject selectMark;

    //������ ��ų ���͸� ���� ����
    GameObject myRespawnObj;

    //������ ������Ʈ���� ������ ���° ���Ϳ� ���� ����
    public int spawnID { get; set; }

    //���Ͱ� ó�� ������ ���� ��ġ�� ����
    Vector3 originPos;

    void Start()
    {
        myAni = GetComponent<EnemyAni>();
        myParams = GetComponent<EnemyParams>();
        myParams.deadEvent.AddListener(CallDeadEvent);

        ChangeState(State.Idle, EnemyAni.IDLE);

        controller = GetComponent<CharacterController>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerParams = player.gameObject.GetComponent<PlayerParams>();

        //hitEffect.Stop();
        HideSelection();
    }

    //���Ͱ� ������ �� �� �ʱ�ȭ ���·� ��
    public void AddToWorldAgain()
    {
        //������ ������Ʈ���� ó�� ������ ���� ��ġ�� ���� ��
        transform.position = originPos;

        GetComponent<EnemyParams>().InitParams();
        GetComponent<BoxCollider>().enabled = true;
    }

    public void HideSelection()
    {
        selectMark.SetActive(false);
    }

    public void ShowSelection()
    {
        selectMark.SetActive(true);
    }

    //���Ͱ� ��� ������ ������Ʈ�κ��� ������������� ���� ������ ���� ���� �Լ�
    public void SetRespawnObj(GameObject respawnObj, int spawnID, Vector3 originPos)
    {
        myRespawnObj = respawnObj;
        this.spawnID = spawnID;
        this.originPos = originPos;
    }

    //���Ͱ� �״� ���� ó�� ��ɾ�
    void CallDeadEvent()
    {
        ChangeState(State.Dead, EnemyAni.DIE);

        //���Ͱ� ���� �� ������ �� ������ �����Ѵ�.
        ObjectManager.instance.DropCoinToPosition(transform.position, myParams.rewardMoney);

        player.gameObject.SendMessage("CurrentEnemyDead");

        //���Ͱ� ������� �� ���� �Ҹ�
        SoundManager.instance.PlayEnemyDie();

        StartCoroutine(RemoveMeFromWorld());
    }

    IEnumerator RemoveMeFromWorld()
    {
        yield return new WaitForSeconds(1f);
        ChangeState(State.Idle, EnemyAni.IDLE);

        //������ ������Ʈ�� �ڱ� �ڽ��� ������ �޶�� ��û
        myRespawnObj.GetComponent<RespawnObj>().RemoveMonster(spawnID);
    }

    /*public void ShowHitEffect()
    {
            hitEffect.Play();
    }*/

    public void AttackCalculate()
    {
        playerParams.SetEnemyAttack(myParams.GetRandomAttack());
        //���Ͱ� ������ �� ���� �Ҹ�
        SoundManager.instance.PlayEnemyAttack();
    }


    void UpdateState()
    {
        switch (currentState)
        {
            case State.Idle:
                IdleState();
                break;

            case State.Chase:
                ChaseState();
                break;

            case State.Attack:
                AttackState();
                break;

            case State.Dead:
                DeadState();
                break;

            case State.NoState:
                NoState();
                break;
        }
    }

    public void ChangeState(State newState, string aniName)
    {
        if (currentState == newState)
        {
            return;
        }

        currentState = newState;
        myAni.ChangeAni(aniName);
    }

    void IdleState()
    {
        if (GetDistanceFromPlayer() < chaseDistance)
        {
            ChangeState(State.Chase, EnemyAni.WALK);
        }
    }

    void ChaseState()
    {
        //���Ͱ� ���� ���� �Ÿ� ������ ���� ���� ����
        if (GetDistanceFromPlayer() < attackDistance)
        {
            ChangeState(State.Attack, EnemyAni.ATTACK);
        }
        else
        {
            TurnToDestination();
            MoveToDestination();
        }
    }

    void AttackState()
    {
        if (player.GetComponent<PlayerFSM>().currentState == PlayerFSM.State.Dead)
        {
            ChangeState(State.NoState, EnemyAni.IDLE);
        }
        if (GetDistanceFromPlayer() > reChaseDistance)
        {
            attackTimer = 0f;
            ChangeState(State.Chase, EnemyAni.WALK);
        }
        else
        {
            if (attackTimer > attackDelay)
            {
                transform.LookAt(player.position);
                myAni.ChangeAni(EnemyAni.ATTACK);

                attackTimer = 0f;
                //���Ͱ� ������ �� ���� �Ҹ�
                //SoundManager.instance.PlayEnemyAttack();
            }

            attackTimer += Time.deltaTime;
        }
    }

    void DeadState()
    {
        GetComponent<BoxCollider>().enabled = false;
    }

    void NoState()
    {

    }

    void TurnToDestination()
    {
        Quaternion lookRotation = Quaternion.LookRotation(player.position - transform.position);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotAnglePerSecond);
    }

    void MoveToDestination()
    {
        //transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

        //������ �̵��� ĳ���� ��Ʈ�ѷ��� �ٲ�, ���Ͱ� �������� moveSpeed ��ŭ�� ������� �̵��ϰ� ��
        controller.Move(transform.forward * moveSpeed * Time.deltaTime);
    }

    //�÷��̾�� �Ÿ��� ��� �Լ�
    float GetDistanceFromPlayer()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        return distance;
    }

    void Update()
    {
        UpdateState();
    }
}
