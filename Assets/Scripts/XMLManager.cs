using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System;

public class XMLManager : MonoBehaviour
{
    public static XMLManager instance;

    //xml ����
    public TextAsset enemyFileXml;

    //�������� �������� �־ ����ü �ϳ��� �Ѱ��� ����ó�� �����ϰ� ����� �� ����
    struct MonParams
    {
        //xml ���Ϸκ��� ������ ���Ϳ� ���ؼ� �̵� �Ķ���� ���� �о� ���̰� ����ü ���� ������ �����ϰ� ����ü�� �̿��Ͽ� �� ���Ϳ��� �Ķ���� ���� ������
        public string name;
        public int level;
        public int maxHp;
        public int attackMin;
        public int attackMax;
        public int defense;
        public int exp;
        public int rewardMoney;
    }

    //��ųʸ��� Ű������ ���� �̸��� ����� �����̹Ƿ� stringŸ������ �ϰ� ������ �����δ� ����ü�� �̿��� MonParams�� ����
    Dictionary<string, MonParams> dicMonsters = new Dictionary<string, MonParams>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        MakeMonsterXML();
    }

    //XML�κ��� �Ķ���� �� �о� ���̱�
    void MakeMonsterXML()
    {
        XmlDocument monsterXMLDoc = new XmlDocument();
        monsterXMLDoc.LoadXml(enemyFileXml.text);

        XmlNodeList monsterNodeList = monsterXMLDoc.GetElementsByTagName("row");

        //��� ����Ʈ�κ��� ������ ��带 �̾Ƴ�
        foreach(XmlNode monsterNode in monsterNodeList)
        {
            MonParams monParams = new MonParams();

            foreach(XmlNode childNode in monsterNode.ChildNodes)
            {
                if (childNode.Name == "name")
                {
                    //<name>smallspider</name>
                    monParams.name = childNode.InnerText;
                }

                if (childNode.Name == "level")
                {
                    //<level>1</level> Int16.Parse()�� ���ڿ��� ������ �ٲ���
                    monParams.level = Int16.Parse(childNode.InnerText);
                }

                if (childNode.Name == "maxHp")
                {
                    monParams.maxHp=Int16.Parse(childNode.InnerText);
                }

                if (childNode.Name == "attackMin")
                {
                    monParams.attackMin = Int16.Parse(childNode.InnerText);
                }

                if (childNode.Name == "attackMax")
                {
                    monParams.attackMax = Int16.Parse(childNode.InnerText);
                }

                if (childNode.Name == "defense")
                {
                    monParams.defense = Int16.Parse(childNode.InnerText);
                }

                if (childNode.Name == "exp")
                {
                    monParams.exp = Int16.Parse(childNode.InnerText);
                }

                if (childNode.Name == "rewardMoney")
                {
                    monParams.rewardMoney = Int16.Parse(childNode.InnerText);
                }

                print(childNode.Name + ": " + childNode.InnerText);
            }
            dicMonsters[monParams.name] = monParams;
        }
     }

    //�ܺηκ��� ������ �̸���, EnemyParams ��ü�� ���� �޾Ƽ� �ش� �̸��� ���� ������
    //������(XML���� �о� �� ������)�� ���޹��� EnemyParams ��ü�� �����ϴ� ������ �ϴ� �Լ�
    public void LoadMonsterParamsFromXML(string monName, EnemyParams mParams)
    {
        mParams.level = dicMonsters[monName].level;
        mParams.curHp = mParams.maxHp = dicMonsters[monName].maxHp;
        mParams.attackMin = dicMonsters[monName].attackMin;
        mParams.attackMax = dicMonsters[monName].attackMax;
        mParams.defense = dicMonsters[monName].defense;
        mParams.exp = dicMonsters[monName].exp;
        mParams.rewardMoney = dicMonsters[monName].rewardMoney;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
