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
        // 갈고리 던지는 조건
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
            // 던졌으나 아직 지붕에 걸리지 않았을 때
            if (!isHoldingRoof)
            {
                // 오른쪽 위쪽으로 갈고리를 던진다!
                transform.Translate(new Vector3(1.0F, 1.0F, 0.0F) * Time.deltaTime * hookSpeed);

                if (2 < transform.position.x/* || Input.GetKeyUp(KeyCode.Space)*/)
                    Cancel();
            }
            // 던졌고, 지붕에 걸린 상태일 때
            else
            {
                // 훅이 지붕에 걸린것처럼 보이게 하기 위해 지붕과 같은 속도로 옆으로 보내준다
                transform.Translate(Vector3.left * Time.deltaTime * GameManager.platformSpeed);

                // 스윙을 시킨다 (Kinematic으로 바꾼후 위치조정을 통해 대충 스윙하는것처럼 보이게 한다)
                float swingDipDistance = Mathf.Sqrt(userHookDistanceSquared - (transform.position.x - user.transform.position.x) * (transform.position.x - user.transform.position.x));

                user.transform.position = new Vector3(
                   user.transform.position.x,
                   transform.position.y - swingDipDistance,
                   user.transform.position.z
                   );

                Debug.DrawRay(transform.position, Vector3.down * Mathf.Sqrt(userHookDistanceSquared), new Color(0, 1, 0));

                // 스윙이 끝나는 조건
                if (user.transform.position.x - transform.position.x > Mathf.Abs(hookPositionWhenHold.x - userPositionWhenHold.x))
                {
                    // 후크가 떨어지며 스윙이 끝남
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
            // 후크가 지붕에 걸리면 스윙이 시작
            isHoldingRoof = true;
            user.GetComponent<Rigidbody2D>().isKinematic = true;
            user.GetComponent<BoxCollider2D>().isTrigger = true;
            user.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            SoundManager.Instance.playSound("chainHitSFX");

            // 훅이 걸렸을 때의 훅 좌표와 플레이어 좌표를 저장
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