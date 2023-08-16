using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform player;

    Vector3 offset;

    void Start()
    {
        offset = player.position - transform.position;
    }

    //ī�޶� �÷��̾� �����ӿ� �� ���� �ʰ� �������� �ش�
    void LateUpdate()
    {
        //�÷��̾��� ��ġ�� ī�޶��� ��ġ�� ���� ������ ��ġ ���̸�ŭ �ڵ����� ���������ְ� ��
        transform.position = player.position - offset;
    }
}
