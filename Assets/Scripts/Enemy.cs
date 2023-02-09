using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{

    public enum Type { Circle, Triangle, Square };
    public enum State { Following, Stunned, Merging, NoMerge, MergeFinished };
    public Type shape;
    public State state;
    public int type;
    public int level = 1;
    public float defaultSpeed = 5;
    public float aggroSpeed = 15;

    bool isStunned = true;
    float timer;
    bool timerIsRunning;
    [SerializeField] Animator myAnimator;

    Color oldColor;
    PlayerController player;
    [SerializeField] Collider2D myCollider;
    [SerializeField] SOLevelData levelData;


    void Awake()
    {
        // Start with INvincible Time (not able to merge)
        //   state = State.NoMerge;
    }

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        GetComponent<AIPath>().maxSpeed = defaultSpeed;
        GetComponent<AIDestinationSetter>().target = player.transform;



        switch (shape)
        {

            case Type.Triangle:
                type = 0;
                break;


            case Type.Square:
                type = 1;
                break;

            case Type.Circle:
                type = 2;
                break;
            default:
                Debug.Log("NOTHING");
                break;
        }



    }

    void Update()
    {
        switch (state)
        {
            case State.Following:
                GetComponent<AIPath>().canMove = true;
                GetComponent<AIDestinationSetter>().target = player.gameObject.transform;
                GetComponentInChildren<SpriteRenderer>().color = Color.green;

                break;
            case State.Stunned:


                //StartCoroutine(RunStunTime(stunTime));
                GetComponentInChildren<SpriteRenderer>().color = Color.black;
                GetComponent<AIPath>().canMove = false;
                GetComponent<AIDestinationSetter>().target = null;

                RunTimer();


                break;
            case State.Merging:

                gameObject.transform.localScale = new Vector3(level * 2, level * 2, 1);
                oldColor = GetComponentInChildren<SpriteRenderer>().color;
                GetComponentInChildren<SpriteRenderer>().color = Color.gray;
                GetComponent<AIPath>().canMove = false;


                break;

            case State.NoMerge:

                StartCoroutine(StopInvincibleTime());


                break;

            case State.MergeFinished:
                myCollider.enabled = true;
                aggroSpeed += (level * 2f);
                GetComponent<AIPath>().canMove = true;
                level++;
                state = Enemy.State.Following;

                break;

            default:

                break;
        }
    }



    void RunTimer()
    {
        timer += Time.deltaTime;
        if (timer > levelData.stunTime - 1)
        {
            myAnimator.SetBool("UnstunWarning", true);
        }
        if (timer > levelData.stunTime)
        {
            myAnimator.SetBool("UnstunWarning", false);
            timer = 0;
            state = Enemy.State.Following;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {

        if (state != State.Merging && state != State.NoMerge && other != null && other.gameObject.tag == "Enemy")
        {
            if (other.gameObject.GetComponent<Enemy>().shape == shape && other.gameObject.GetComponent<Enemy>().level == level || other.gameObject.GetComponent<Enemy>().level == level - 1)
            {

                StartCoroutine(ChangeSize(other));
            }

        }

    }



    IEnumerator RunStunTime(float t)
    {
        yield return new WaitForSeconds(t);
        state = Enemy.State.Following;
    }

    IEnumerator StopInvincibleTime()
    {
        yield return new WaitForSeconds(2);
        state = Enemy.State.Following;
    }

    IEnumerator ChangeSize(Collision2D other)

    {
        // Adding positions to determine a unique difference between two colliding enemies (one must die)
        if (other.gameObject.transform.position.x + other.gameObject.transform.position.y > gameObject.transform.position.x + gameObject.transform.position.y)
        {

            state = Enemy.State.Merging;

            yield return new WaitForSeconds(1);

            state = Enemy.State.MergeFinished;
        }
        else

        {
            Destroy(gameObject);
        }

    }

}
