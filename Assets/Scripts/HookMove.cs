using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookMove : MonoBehaviour
{
    public GameObject user;
    public float hookSpeed;

    SpriteRenderer spriteRenderer;

    bool isThrown = false;
    bool isHoldingRoof = false;

    Vector3 hookPositionWhenHold = Vector3.zero;
    Vector3 userPositionWhenHold = Vector3.zero;
    float userHookDistanceSquared = 0.0F;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        Cancel();
    }

    void Update()
    {
        // ���� ������ ����
        if (user.GetComponent<Animator>().GetBool("isJumping") && !isThrown && !isHoldingRoof)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                transform.position = user.transform.position;
                spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1.0F);
                isThrown = true;
                SoundManager.Instance.playSound("throwSFX");
            }
        }

        if (isThrown)
        {
            // �������� ���� ���ؿ� �ɸ��� �ʾ��� ��
            if (!isHoldingRoof)
            {
                // ������ �������� ������ ������!
                transform.Translate(new Vector3(1.0F, 1.0F, 0.0F) * Time.deltaTime * hookSpeed);

                if (2 < transform.position.x/* || Input.GetKeyUp(KeyCode.Space)*/)
                    Cancel();
            }
            // ������, ���ؿ� �ɸ� ������ ��
            else
            {
                // ���� ���ؿ� �ɸ���ó�� ���̰� �ϱ� ���� ���ذ� ���� �ӵ��� ������ �����ش�
                transform.Translate(Vector3.left * Time.deltaTime * GameManager.platformSpeed);

                // ������ ��Ų�� (Kinematic���� �ٲ��� ��ġ������ ���� ���� �����ϴ°�ó�� ���̰� �Ѵ�)
                float swingDipDistance = Mathf.Sqrt(userHookDistanceSquared - (transform.position.x - user.transform.position.x) * (transform.position.x - user.transform.position.x));

                user.transform.position = new Vector3(
                   user.transform.position.x,
                   transform.position.y - swingDipDistance,
                   user.transform.position.z
                   );

                Debug.DrawRay(transform.position, Vector3.down * Mathf.Sqrt(userHookDistanceSquared), new Color(0, 1, 0));

                // ������ ������ ����
                if (user.transform.position.x - transform.position.x > Mathf.Abs(hookPositionWhenHold.x - userPositionWhenHold.x))
                {
                    // ��ũ�� �������� ������ ����
                    isHoldingRoof = false;
                    Cancel();
                    user.GetComponent<Rigidbody2D>().isKinematic = false;
                    user.GetComponent<BoxCollider2D>().isTrigger = false;
                    user.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 17.0F, ForceMode2D.Impulse);
                    user.GetComponent<Animator>().SetTrigger("swingJump");
                }
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "HookDetector" && isThrown)
        {
            // ��ũ�� ���ؿ� �ɸ��� ������ ����
            isHoldingRoof = true;
            user.GetComponent<Rigidbody2D>().isKinematic = true;
            user.GetComponent<BoxCollider2D>().isTrigger = true;
            user.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            SoundManager.Instance.playSound("chainHitSFX");

            // ���� �ɷ��� ���� �� ��ǥ�� �÷��̾� ��ǥ�� ����
            hookPositionWhenHold = transform.position;
            userPositionWhenHold = user.transform.position;
            userHookDistanceSquared = (hookPositionWhenHold.x - userPositionWhenHold.x) * (hookPositionWhenHold.x - userPositionWhenHold.x)
                + (hookPositionWhenHold.y - userPositionWhenHold.y) * (hookPositionWhenHold.y - userPositionWhenHold.y);
        }
    }

    void Cancel()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.0F);
        isThrown = false;
    }

    public bool GetIsThrown()
    {
        return isThrown;
    }
}