using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip vfClip;
    
    private Rigidbody2D rd2d;

    public float speed;
    
    public TextMeshProUGUI score;
    public TextMeshProUGUI lives;
    
    public GameObject winTextObject;
    public GameObject loseTextObject;
    
    private int scoreValue = 0;
    private int livesValue = 3;

    private bool levelTwo = false;
    private bool gameWon = false;

    public float jumpForce;
    private bool isOnGround = true;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask allGround;

    private bool facingRight = true;

    Animator anim; 

    public TextMeshProUGUI jumpText;
    

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = scoreValue.ToString();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        lives.text = "Lives: " + livesValue.ToString(); 
        musicSource.Play();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal") < 0)
        {
            anim.SetBool("isRunning", true);
        }
        else 
            anim.SetBool("isRunning", false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
         void Flip()
            {
                facingRight = !facingRight;
                Vector2 Scaler = transform.localScale;
                Scaler.x = Scaler.x * -1;
                transform.localScale = Scaler;
            }

        if (Input.GetKey(KeyCode.Escape))
            {
            Application.Quit();
            }


        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundCheck.position, checkRadius, allGround);

        if (facingRight == false && hozMovement > 0)
        {
            Flip(); 

        }

        if (facingRight == true && hozMovement < 0)
        {
            Flip();

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            Destroy(collision.collider.gameObject);
            scoreValue = scoreValue + 1;
            score.text = scoreValue.ToString();
        }

        if (scoreValue >= 4 && levelTwo == false)
        {
            livesValue = 3;
            lives.text = "Lives: " + livesValue.ToString();
            levelTwo = !levelTwo;
            transform.position = new Vector2(60.0f, 0.0f);
        }

        if (collision.collider.tag == "Enemy")
        {
            livesValue = livesValue - 1;
            Destroy(collision.collider.gameObject);
            lives.text = "Lives: " + livesValue.ToString();
        }

        if (livesValue <= 0)
        {
            loseTextObject.SetActive(true);
            Destroy(gameObject);
        }

        if (scoreValue >= 8 && gameWon == false)
        {
            winTextObject.SetActive(true);
            musicSource.clip = vfClip;
            musicSource.Play();
            gameWon = !gameWon;
            rd2d.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.collider.tag == "Ground" && isOnGround)
        {
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce (new Vector2(0, jumpForce), ForceMode2D.Impulse);
                anim.SetBool("isJumping", true);
            }
            else
                anim.SetBool("isJumping", false);
        }
    }
}
