using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageToPlayer : MonoBehaviour
{
    [SerializeField] int damage;

    private void OnCollisionEnter2D(Collision2D other)
    {
        var myScript = GetComponent<Enemy>();

        if (other.gameObject.tag == "Player" && myScript.state != Enemy.State.Stunned)
        {
            var playerScript = other.gameObject.GetComponent<PlayerController>();
            if (!playerScript.isDashing)
            {
                playerScript.PlayerTakeDamage(damage);

            }
            Destroy(myScript.gameObject);

        }
    }
}
