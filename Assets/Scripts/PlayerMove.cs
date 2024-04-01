using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float jumpPower;
    public GameObject particlePrefab;
    bool isJumping;
    bool canJump = false;

    Rigidbody2D rigid;
    Animator animator;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (canJump)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
            }
        }
    }

    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, Vector3.down * 5, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, Vector3.down, 5, LayerMask.GetMask("Platform"));

        if (rayHit.collider)
        {
            canJump = true;

            if (isJumping)
            {
                rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                SoundManager.Instance.playSound("jumpSFX");
                isJumping = false;
            }

            // ���� ������� ��
            if (rayHit.distance < 0.5F)
            {
                // �⺻�����δ� ����ִ� ��� ���
                animator.SetBool("isJumping", false);
            }
            else // ������ �������ٸ� ���� ���
            {
                animator.SetBool("isJumping", true);
                canJump = false;
            }
        }
        else
        {
            canJump = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� �Ա�
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
