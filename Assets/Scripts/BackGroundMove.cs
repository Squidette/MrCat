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

        // ȭ�鿡�� ������ �ٽ� ���������� �Ű��ֱ�
        if (transform.position.x < -18.5)
        {
            transform.Translate(37.0F, 0.0F, 0.0F); // �̷��� scriptable object�� ó�����ִ°� �� ����ϴٰ� �մϴ�..
        }
    }
}