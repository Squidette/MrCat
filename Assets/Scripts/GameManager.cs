using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static float platformSpeed = 6.5F;   // 레벨별로 달라진다
    public static int level = 1;
    public Text UIScore;
    public Text UIGameOver;
    public Text UILevel;

    // 오른쪽에서 왼쪽으로 부드럽게 슥 멈췄다 지나가는 레벨UI를 만들기 위한 변수들
    bool UILevelShowing = false;
    float UILevelBackToFullSpeedPosition = 60.0F;
    float UILevelMinSpeedXPosition = 148.0F;
    float UILevelSlowingStartPosition = 230.0F;
    float UIMinSpeed = 50.0F;
    float UIMaxSpeed = 1500.0F;

    int score;

    void Awake() // 리나: 다시 생각해 봐라
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UIGameOver.text = "게임 오버.";
        UIGameOver.gameObject.SetActive(false);

        Invoke("SendLevelUI", 1.0F);
    }

    void Update()
    {
        // 점수 업데이트
        UIScore.text = $"점수: {score}";

        // 레벨업 시간
        if (Time.time > 20.0F && level == 1) LevelUp();
        if (Time.time > 50.0F && level == 2) LevelUp();
        if (Time.time > 80.0F && level == 3) LevelUp();

        UILevelUpdate();
    }

    public void AddScore(int pt)
    {
        score += pt;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.gameObject.tag == "Player")
        {
            ShowGameOver();
        }
    }
    
    void ShowGameOver()
    {
        Debug.Log("게임 끝!");
        SoundManager.Instance.playSound("gameOverSFX");
        UILevel.gameObject.SetActive(false);
        UIGameOver.gameObject.SetActive(true);
        UIScore.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(734.0F, -379.0F);

        Time.timeScale = 0.0F;
    }

    void LevelUp()
    {
        if (level > 3) return;
        level++;
        Debug.Log("레벨 업!");

        switch (level)
        {
            case 1:
                platformSpeed = 6.5F;
                break;
            case 2:
                platformSpeed = 8.0F;
                break;
            case 3:
                platformSpeed = 10.0F;
                break;
            case 4:
                platformSpeed = 12.0F;
                break;
        }

        SendLevelUI();
    }

    void SendLevelUI()
    {
        UILevel.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(500.0F, 0.0F);
        UILevel.text = "Level " + level;
        UILevelShowing = true;
        SoundManager.Instance.playSound("levelUpSFX");
    }

    void UILevelUpdate()
    {
        if (UILevelShowing)
        {
            float UILevelPositionX = UILevel.gameObject.GetComponent<RectTransform>().anchoredPosition.x;

            // 스피드를 UILevelPositionX의 값에 따라 이차함수 형태로 설정해 주기
            float UILevelSpeed;

            if (UILevelPositionX < UILevelBackToFullSpeedPosition || UILevelPositionX >= UILevelSlowingStartPosition)
            {
                UILevelSpeed = UIMaxSpeed;
            }
            else if (UILevelBackToFullSpeedPosition <= UILevelPositionX && UILevelPositionX < UILevelMinSpeedXPosition)
            {
                float slope = (UIMaxSpeed - UIMinSpeed) / ((UILevelMinSpeedXPosition - UILevelBackToFullSpeedPosition) * (UILevelMinSpeedXPosition - UILevelBackToFullSpeedPosition));
                UILevelSpeed = slope * (UILevelPositionX - UILevelMinSpeedXPosition) * (UILevelPositionX - UILevelMinSpeedXPosition) + UIMinSpeed;
            }
            else /*if (UILevelMinSpeedXPosition <= UILevelPositionX && UILevelPositionX < UILevelSlowingStartPosition)*/
            {
                float slope = (UIMaxSpeed - UIMinSpeed) / ((UILevelSlowingStartPosition - UILevelMinSpeedXPosition) * (UILevelSlowingStartPosition - UILevelMinSpeedXPosition));
                UILevelSpeed = slope * (UILevelPositionX - UILevelMinSpeedXPosition) * (UILevelPositionX - UILevelMinSpeedXPosition) + UIMinSpeed;
            }

            // 계산된 스피드만큼 왼쪽으로 이동
            UILevel.gameObject.GetComponent<RectTransform>().anchoredPosition += Vector2.left * UILevelSpeed * Time.deltaTime;

            // 화면을 벗어나면 멈춰 준다
            if (UILevel.gameObject.GetComponent<RectTransform>().anchoredPosition.x < -668)
            {
                UILevelShowing = false;
            }
        }
    }
}