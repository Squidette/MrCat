using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance = null;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }
            else
            {
                return instance;
            }
        }
    }

    AudioSource audioSource;
    public AudioClip stageBGM;
    public AudioClip throwSFX;
    public AudioClip jumpSFX;
    public AudioClip chainHitSFX;
    public AudioClip gameOverSFX;
    public AudioClip coinEatSFX;
    public AudioClip levelUpSFX;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            // ���� ��ȯ�Ǵ��� �ı����� �ʰ� ������
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            // �����͸Ŵ����� �̹� �ִµ� �ϳ��� �� ����������� ���
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        playSound("stageBGM");
    }

    public void playSound(string soundName)
    {
        AudioClip c = null;
        float f = 0.0F;

        switch (soundName)
        {
            case "stageBGM":
                c = stageBGM;
                f = 0.8F;
                break;
            case "throwSFX":
                c = throwSFX;
                f = 1.0F;
                break;
            case "jumpSFX":
                c = jumpSFX;
                f = 1.0F;
                break;
            case "chainHitSFX":
                c = chainHitSFX;
                f = 1.0F;
                break;
            case "gameOverSFX":
                c = gameOverSFX;
                f = 1.0F;
                break;
            case "coinEatSFX":
                c = coinEatSFX;
                f = 1.0F;
                break;
            case "levelUpSFX":
                c = levelUpSFX;
                f = 1.0F;
                break;
        }

        if (c != null)
        {
            audioSource.PlayOneShot(c, f);
        }
    }
}
