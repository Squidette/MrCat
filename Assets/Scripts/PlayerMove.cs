using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float jumpPower;
    public GameObject particlePrefab;

    Rigidbody2D rigid;
    Animator animator;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        JumpUpdate();
    }

    void JumpUpdate()
    {
        Debug.DrawRay(transform.position, Vector3.down * 5, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, Vector3.down, 5, LayerMask.GetMask("Platform"));

        if (rayHit.collider)
        {
            // 땅에 닿아있을 때
            if (rayHit.distance < 0.5F)
            {
                // 기본적으로는 닿아있는 모션 출력
                animator.SetBool("isJumping", false);

                // 점프 가능
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                    SoundManager.Instance.playSound("jumpSFX");
                }
            }
            else // 땅에서 떨어졌다면 점프 모션
            {
                animator.SetBool("isJumping", true);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 코인 먹기
        if (collision.gameObject.tag == "Coin")
        {
            if (collision.gameObject.name.Contains("Silver"))
            {
                GameManager.instance.AddScore(10);
            }
            else // "Gold"
            {
                GameManager.instance.AddScore(50);
            }
            collision.gameObject.SetActive(false);
        }

        PlayerPopEffect();
        SoundManager.Instance.playSound("coinEatSFX");
    }

    public void PlayerPopEffect()
    {
        //Instantiate(particlePrefab, transform.position, Quaternion.identity);
        ObjectPoolManager.SpawnObject(particlePrefab, transform.position, Quaternion.identity);
    }
}
