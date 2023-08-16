using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : MonoBehaviour
{
    public enum State
    {
        Idle,
        Move,
        Attack,
        AttackWait,
        Dead
    }
    //idle ���¸� �⺻ ���·� ����
    public State currentState = State.Idle;

    //���콺 Ŭ�� ����, �÷��̾ �̵��� �������� ��ǥ�� ������ ����
    Vector3 curTargetPos;

    GameObject curEnemy;

    public float rotAnglePerSecond = 360f; //1�ʿ� �÷��̾��� ������ 360�� ȸ���Ѵ�

    public float moveSpeed = 3.5f; //�ʴ� 3.5������ �ӵ��� �̵�

    float attackDelay = 0.8f; //������ �ѹ��ϰ� �ٽ� �����Ҷ� ������ �����ð�
    float attackTimer = 0f; //������ �ϰ� �� �ڿ� ����Ǵ� �ð��� ����ϱ� ���� ����
    float attackDistance = 1.5f; //���� �Ÿ�(������ �Ÿ�)
    float chaseDistance = 2.5f; //���� �� ���� �������� �ٽ� ������ ���� �ϱ� ���� �Ÿ�

    PlayerAni myAni;

    PlayerParams myParams;

    EnemyParams curEnemyParams;

    void Start()
    {
        myAni = GetComponent<PlayerAni>();
        //myAni.ChangeAni(PlayerAni.ANI_WALK);

        myParams = GetComponent<PlayerParams>();

        myParams.InitParams();

        myParams.deadEvent.AddListener(ChangeToPlayerDead);

        ChangeState(State.Idle, PlayerAni.ANI_IDLE);

        curEnemy = GameObject.FindGameObjectWithTag("Enemy");
    }

    public void ChangeToPlayerDead()
    {
        print("player was dead");
        ChangeState(State.Dead, PlayerAni.ANI_DIE);

        UIManager.instance.ShowGameOver();
    }

    public void CurrentEnemyDead()
    {
        ChangeState(State.Idle, PlayerAni.ANI_IDLE);
        print("enemy was killed");

        curEnemy = null;
    }

    public void AttackCalculate()
    {
        if(curEnemy == null)
        {
            return;
        }

        print("Attack" + curEnemy.name + "...");
        //curEnemy.GetComponent<EnemyFSM>().ShowHitEffect();

        int attackPower = myParams.GetRandomAttack();
        curEnemyParams.SetEnemyAttack(attackPower);

        //�÷��̾ ������ �� ���� �Ҹ�
        SoundManager.instance.PlayHitSound();
    }

    //���� �����ϱ� ���� �Լ�
    public void AttackEnemy(GameObject enemy)
    {
        if (curEnemy != null && curEnemy == enemy)
        {
            return;
        }

        //��(����)�� �Ķ���͸� ������ ����
        curEnemyParams = enemy.GetComponent<EnemyParams>();

        if (curEnemyParams.isDead == false)
        {
            curEnemy = enemy;
            curTargetPos = curEnemy.transform.position;

            GameManager.instance.ChangeCurrentTarget(curEnemy);

            ChangeState(State.Move, PlayerAni.ANI_WALK);
        }
        else
        {
            curEnemyParams = null;
        }
    }

    void ChangeState(State newState, int aniNumber)
    {
        if(currentState == newState)
        {
            return;
        }

        myAni.ChangeAni(aniNumber);
        currentState = newState;
    }

    //ĳ������ ���°� �ٲ�� � ���� �Ͼ���� �̸� ����
    void UpdateState()
    {
        switch (currentState)
        {
            case State.Idle:
                IdleState();
                break;

            case State.Move:
                MoveState();
                break;

            case State.Attack:
                AttackState();
                break;

            case State.AttackWait:
                AttackWaitState();
                break;

            case State.Dead:
                DeadState();
                break;

            default:
                break;
        }
    }


    void IdleState()
    {

    }

    void MoveState()
    {
        TurnToDestination();
        MoveToDestination();
    }

    void AttackState()
    {
        attackTimer = 0f;

        //transform.LookAt(��ǥ���� ��ġ) ��ǥ������ ���� ������Ʈ�� ȸ�� ��Ű�� �Լ�
        transform.LookAt(curTargetPos);
        ChangeState(State.AttackWait, PlayerAni.ANI_ATKIDLE);
    }

    void AttackWaitState()
    {
        if (attackTimer > attackDelay)
        {
            ChangeState(State.Attack, PlayerAni.ANI_ATTACK);
        }

        attackTimer += Time.deltaTime;
    }

    void DeadState()
    {

    }

    //MoveTo(ĳ���Ͱ� �̵��� ��ǥ ������ ��ǥ)
    public void MoveTo(Vector3 tPos)
    {
        //����Ͽ��� �� ������ ���� ó��(���⼭ ����)
        if (currentState == State.Dead)
        {
            return;
        }

        curEnemy = null;
        curTargetPos = tPos;
        ChangeState(State.Move, PlayerAni.ANI_WALK);
    }

    void TurnToDestination()
    {
        //Quaternion lookRotation(ȸ���� ��ǥ ����) : ��ǥ ������ ������ ��ġ���� �ڽ��� ��ġ�� ���� ����
        Quaternion lookRotation = Quaternion.LookRotation(curTargetPos - transform.position);

        //Quaternion.RotateTowards(������ rotation��, ������ǥ rotation ��, �ִ� ȸ����)
        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, Time.deltaTime * rotAnglePerSecond);
    }

    void MoveToDestination()
    {
        //Vector3.MoveTowards(��������, ��ǥ����, �ִ��̵��Ÿ�)
        transform.position = Vector3.MoveTowards(transform.position, curTargetPos, moveSpeed * Time.deltaTime);

        if (curEnemy == null)
        {
            //�÷��̾��� ��ġ�� ��ǥ������ ��ġ�� ������, ���¸� Idle ���·� �ٲٶ�� ���
            if (transform.position == curTargetPos)
            {
                ChangeState(State.Idle, PlayerAni.ANI_IDLE);
            }
        }
        else if (Vector3.Distance(transform.position, curTargetPos) < attackDistance) //Vectoe3.Distance(A,B) : A��B������ �Ÿ�
        {
            ChangeState(State.Attack, PlayerAni.ANI_ATTACK);
        }

    }

    void Update()
    {
        UpdateState();   
    }
}
