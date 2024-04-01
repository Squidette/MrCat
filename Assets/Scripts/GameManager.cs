using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static float platformSpeed = 6.5F;   // �������� �޶�����
    public static int level = 1;
    public Text UIScore;
    public Text UIGameOver;
    public Text UILevel;

    // �����ʿ��� �������� �ε巴�� �� ����� �������� ����UI�� ����� ���� ������
    bool UILevelShowing = false;
    float UILevelBackToFullSpeedPosition = 60.0F;
    float UILevelMinSpeedXPosition = 148.0F;
    float UILevelSlowingStartPosition = 230.0F;
    float UIMinSpeed = 50.0F;
    float UIMaxSpeed = 1500.0F;

    int score;

    void Awake() // ����: �ٽ� ������ ����
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
        UIGameOver.text = "���� ����.";
        UIGameOver.gameObject.SetActive(false);

        Invoke("SendLevelUI", 1.0F);
    }

    void Update()
    {
        // ���� ������Ʈ
        UIScore.text = $"����: {score}";

        // ������ �ð�
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
        Debug.Log("���� ��!");
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
        Debug.Log("���� ��!");

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

            // ���ǵ带 UILevelPositionX�� ���� ���� �����Լ� ���·� ������ �ֱ�
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

            // ���� ���ǵ常ŭ �������� �̵�
            UILevel.gameObject.GetComponent<RectTransform>().anchoredPosition += Vector2.left * UILevelSpeed * Time.deltaTime;

            // ȭ���� ����� ���� �ش�
            if (UILevel.gameObject.GetComponent<RectTransform>().anchoredPosition.x < -668)
            {
                UILevelShowing = false;
            }
        }
    }
}