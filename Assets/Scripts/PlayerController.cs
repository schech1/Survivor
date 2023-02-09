using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{


    [SerializeField] GameObject bomb;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip pickUpSFX;
    [SerializeField] AudioClip hurtSFX;
    [SerializeField] SOPlayerData playerData;
    Vector2 moveInput;
    Vector2 lookInput;
    bool invincible;
    [SerializeField] Gun gun;
    public bool isDashing;
    Rigidbody2D myRb;
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>();

        ResetPlayerData();

    }

    void OnMove(InputValue value)
    {
        moveInput.x = value.Get<Vector2>().x;
        moveInput.y = value.Get<Vector2>().y;

    }


    void OnLook(InputValue value)
    {
        lookInput.x = value.Get<Vector2>().x;
        lookInput.y = value.Get<Vector2>().y;

    }

    void OnPrimaryFire(InputValue value)
    {

        {
            GameObject bul = Instantiate(gun.bullet, transform.position, transform.rotation);
            bul.GetComponent<Rigidbody2D>().AddForce(transform.up * gun.projectileSpeed, ForceMode2D.Impulse);
        }
    }


    void OnDash(InputValue value)
    {

        StartCoroutine(Dash());
    }

    IEnumerator Dash()
    {
        isDashing = true;
        playerData.speed = playerData.dashSpeed;
        // myRb.AddForce(transform.up * dashSpeed, ForceMode2D.Impulse);
        //myRb.velocity = new Vector2(myRb.velocity.x * 3f, myRb.velocity.y);
        yield return new WaitForSeconds(playerData.dashTime);
        playerData.speed = playerData.defaultSpeed;
        isDashing = false;
    }

    void OnSecondaryFire(InputValue value)
    {
        if (playerData.resource[2] >= 50)
        {
            GameObject spawnedBomb = Instantiate(bomb, transform.position, Quaternion.identity);
            playerData.resource[2] -= 50;
        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        myRb.MovePosition(myRb.position + moveInput * playerData.speed);
        if (lookInput != Vector2.zero)
        {
            transform.up = lookInput;
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (!invincible)
            {
                // Collect if stunned
                Enemy enemyRef = other.gameObject.GetComponent<Enemy>();
                if (enemyRef.state == Enemy.State.Stunned)
                {
                    CollectResource(enemyRef.level, enemyRef.type);
                    Destroy(other.gameObject);
                    PlaySound(pickUpSFX);
                }

                //Restart Game if lives 0 or less
                if (playerData.currentPlayerHealth <= 0)
                {
                    // SceneManager.LoadScene(0);
                    GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
                    foreach (GameObject enemy in allEnemies)
                    {
                        Destroy(enemy);
                    }
                    ResetPlayerData();

                }
            }
        }

    }



    void PlaySound(AudioClip sound)
    {
        audioSource.PlayOneShot(sound);
    }



    #region AggroRange
    private void OnTriggerEnter2D(Collider2D other)

    {
        if (other.gameObject.tag == "Enemy" && other != null)
        {

            other.gameObject.GetComponent<AIPath>().maxSpeed = other.gameObject.GetComponent<Enemy>().aggroSpeed;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" && other != null)
        {
            other.gameObject.GetComponent<AIPath>().maxSpeed = other.gameObject.GetComponent<Enemy>().defaultSpeed;
        }
    }
    #endregion

    IEnumerator SetInvincible()
    {
        invincible = true;
        GetComponent<SpriteRenderer>().color = Color.green;
        yield return new WaitForSeconds(3);
        invincible = false;
        GetComponent<SpriteRenderer>().color = Color.blue;
    }


    // Take Damage
    public void PlayerTakeDamage(int value)
    {

        playerData.currentPlayerHealth -= value;
        PlaySound(hurtSFX);
    }




    #region Manage PlayerData 
    // ######################## Manage PlayerData ################



    public void ResetPlayerData()
    {
        playerData.resource[0] = 0;
        playerData.resource[1] = 0;
        playerData.resource[2] = 0;
        playerData.currentPlayerHealth = playerData.initialPlayerHealth;

    }

    public void CollectResource(int level, int type)
    {
        // [0] = triangle,
        // [1] = square,
        // [2] = circle/hex

        playerData.resource[type] += level * Fibonacci(level);
    }


    int Fibonacci(int n)
    {
        if (n == 0 || n == 1)
        {
            return n;
        }
        else
        {
            return Fibonacci(n - 1) + Fibonacci(n - 2);
        }
    }
    #endregion

}
