using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void CheckClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //ī�޶�κ��� ȭ����� ��ǥ�� �����ϴ� ������ ��(����)�� �����ؼ� ������ �ִ� �Լ�
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            //Physics.Raycast(���� Ÿ�� ����, out ���� ĳ��Ʈ ��Ʈ Ÿ�Ժ���):
            //������ ��������(����)�� �浹ü�� �浹�ϸ�, true(��) ���� �����ϸ鼭 ���ÿ� ����ĳ��Ʈ ��Ʈ ������ �浹 ����� ������ ��� �ִ� �Լ�

            if(Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == "Terrain")
                {
                    //player.transform.position = hit.point;

                    //���콺 Ŭ�� ������ ��ǥ�� �÷��̾ ���޹��� ��, ���¸� �̵����·� �ٲ�
                    player.GetComponent<PlayerFSM>().MoveTo(hit.point);
                }
                else if(hit.collider.gameObject.tag=="Enemy") //���콺 Ŭ���� ����� �� ĳ������ ���
                {
                    player.GetComponent<PlayerFSM>().AttackEnemy(hit.collider.gameObject);
                }
            }
        }
    }

    void Update()
    {
        CheckClick();
    }
}
