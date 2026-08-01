using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class alphaPlayer_main : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D rb;

    [SerializeField]
    float speed;

    [SerializeField]
    float jumpPower;

    bool isGrounded;

    [SerializeField]
    LayerMask groundLayer;

    [SerializeField]
    alphaCamera camera;

    [SerializeField]
    alphaMain_main main_Main;

    [SerializeField]
    Canvas resultCanvas;

    public bool goalFlag;

    [SerializeField]
    Renderer playerRenderer;
    // Start is called before the first frame update
    void Start()
    {
        camera.setPosition(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(
            transform.position,
            transform.position
            - transform.up * 1.25f,
            Color.black
        );

        isGrounded = Physics2D.Linecast(
            transform.position,
            transform.position -
            transform.up * 1.25f,
            groundLayer);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                isGrounded = false;
            }
        }

        if (transform.position.x >=  -0.16f&&transform.position.x<=31f)
        {       
            camera.setPosition(transform.position);
        }
        camera.basePos.y = transform.position.y;

        if (goalFlag)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(x * speed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "GoalArea")
        {
            goalFlag = true;
            resultCanvas.enabled = true;
        }

        if (collision.gameObject.tag == "Enemy")
        {
            StartCoroutine(playerDamage());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DeadZone")
        {
            main_Main.Invoke("reloadFunc", 0.5f);
        }
    }

    IEnumerator playerDamage()
    {
        gameObject.layer = LayerMask.NameToLayer("PlayerDamage");
        int count = 10;
        while (count > 0)
        {
            playerRenderer.material.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(0.05f);
            playerRenderer.material.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.05f);
            count--;
        }
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
}
