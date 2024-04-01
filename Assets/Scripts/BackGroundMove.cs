using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMove : MonoBehaviour
{
    void Start()
    {

    }

    void FixedUpdate()
    {
        switch (GameManager.level)
        {
            case 1:
                transform.Translate(Vector3.left * 0.0005F);
                break;
            case 2:
                transform.Translate(Vector3.left * 0.001F);
                break;
            case 3:
                transform.Translate(Vector3.left * 0.0015F);
                break;
            case 4:
                transform.Translate(Vector3.left * 0.002F);
                break;
        }

        // 화면에서 나가면 다시 오른쪽으로 옮겨주기
        if (transform.position.x < -18.5)
        {
            transform.Translate(37.0F, 0.0F, 0.0F); // 이런건 scriptable object로 처리해주는게 더 깔끔하다고 합니다..
        }
    }
}