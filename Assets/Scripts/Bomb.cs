using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Bomb : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Enemy" && other.gameObject != null)
        {
            var enemyState = other.gameObject.GetComponent<Enemy>();

            //On hit stun enemy if: not null, tag = enemy, not merging, no NoMerge, not already stunned
            if (enemyState.state == Enemy.State.Following)
            {
                Debug.Log("stun");
                enemyState.state = Enemy.State.Stunned;
            }
        }

    }


    public void DestroyBomb()
    {
        Destroy(gameObject);
    }
}
