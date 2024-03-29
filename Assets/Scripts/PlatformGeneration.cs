using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class PlatformGeneration : MonoBehaviour
{
    public GameObject[] Platforms;
    int nextPlatformIndex = -1;

    void Start()
    {
        // ó������ ���� �� �÷����� �Ʒ� �������
        Platforms[1].SetActive(true);
        Platforms[1].transform.position = new Vector3(-16.0F, 0.0F, 0.0F);
    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EndDetector")
        {
            nextPlatformIndex = GetWaitingPlatform();
            float nextPlatformTime = Random.Range(0.3F, 0.6F); //0.65F
            //Debug.Log("�÷����� ���� ���Դϴ�.. " + nextPlatformTime + "�� �� ���� �÷��� �� �ϳ��� �����մϴ�"); ;
            Invoke("SetPlatformActive", nextPlatformTime);
            //Invoke("SetPlatformActive", 0.005F);
        }
    }

    // Ȱ��ȭ���� ���� �÷����� �������� �ϳ��� ����
    int GetWaitingPlatform()
    {
        List<int> myList = new List<int>();

        // ������ ���������� �� �پ��� �÷����� ��Ÿ��
        int platformLimitIndex = -1;
        switch (GameManager.level)
        {
            case 1:
                platformLimitIndex = Platforms.Length - 4;
                break;
            case 2:
                platformLimitIndex = Platforms.Length - 2;
                break;
            case 3:
                platformLimitIndex = Platforms.Length - 1;
                break;
            case 4:
                platformLimitIndex = Platforms.Length;
                break;
            default:
                Debug.LogError("������ ���� �߸��Ȱ� ����");
                break;
        }

        for (int i = 0; i < platformLimitIndex; i++)
        {
            if (!Platforms[i].activeSelf)
            {
                myList.Add(i);
            }    
        }

        if (myList.Count == 0)
        {
            Debug.LogError("���� �÷����� �����ϴ�");
            return -1;
        }
        else
        {
             return myList[Random.Range(0, myList.Count)];
        }
    }

    void SetPlatformActive()
    {
        Platforms[nextPlatformIndex].SetActive(true);
    }
}