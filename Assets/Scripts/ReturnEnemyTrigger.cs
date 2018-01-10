using System;
using UnityEngine;

public class ReturnEnemyTrigger : MonoBehaviour
{
    public delegate void EnterAction(object sender, EventArgs e);
    public static event EnterAction OnEnemyEnter;

    void OnTriggerEnter(Collider collided)
    {
        if (collided.CompareTag("Enemy"))
        {
            if (collided.GetComponent<EnemyProjectile>() == null)
            {
                //This is an enemy
                if (OnEnemyEnter != null)
                {
                    OnEnemyEnter(collided.gameObject, null);
                }
            }
            else
            {
                //This is an enemy rocket
                Destroy(collided.gameObject);
            }
        }
    }
}
