using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformControl : MonoBehaviour
{
    public float endPosition;

    void Start()
    {

    }

    void FixedUpdate()
    {
        if (gameObject.activeSelf)
        {
            transform.Translate(Vector3.left * GameManager.platformSpeed * 0.01F);
        }
    }

    void Update()
    {
        //if (gameObject.activeSelf)
        //    transform.Translate(Vector3.left * Time.deltaTime * GameManager.platformSpeed);

        if (transform.position.x < endPosition)
        {
            SetToWaitMode();
        }
    }

    public void SetToWaitMode()
    {
        // 먹은 동전들은 다시 살려두자
        foreach (Transform child in transform)
        {
            if (child.gameObject.name.Contains("Coin"))
            {
                child.gameObject.SetActive(true);
            }
        }

        gameObject.SetActive(false);
        transform.position = Vector3.zero;
    }
}
