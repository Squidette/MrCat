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
        // 처음에는 가장 긴 플랫폼을 아래 깔아주자
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
            //Debug.Log("플랫폼의 끝이 보입니다.. " + nextPlatformTime + "초 후 남은 플랫폼 중 하나를 생성합니다"); ;
            Invoke("SetPlatformActive", nextPlatformTime);
            //Invoke("SetPlatformActive", 0.005F);
        }
    }

    // 활성화되지 않은 플랫폼중 랜덤으로 하나를 뽑음
    int GetWaitingPlatform()
    {
        List<int> myList = new List<int>();

        // 레벨이 높아질수록 더 다양한 플랫폼이 나타남
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
                Debug.LogError("레벨이 뭔가 잘못된것 같음");
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
            Debug.LogError("남은 플랫폼이 없습니다");
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