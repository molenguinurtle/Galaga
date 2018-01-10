using System;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    private int pointsWorth;

    public int PointsWorth
    {
        get { return pointsWorth; }
        set { pointsWorth = value; }
    }

    public GameObject ThePlayer
    {
        get { return _thePlayer;}
        set { _thePlayer = value;}
    }

    public bool CanAttack
    {
        get { return canAttack;}

        set { canAttack = value;}
    }

    public float MovementSpeed
    {
        get { return _movementSpeed; }
        set { _movementSpeed = value; }
    }
    private float _movementSpeed;
    private GameObject _thePlayer;

    private int hitPoints;
    private bool canAttack;
    public delegate void HitAction(object sender, EventArgs e);
    public static event HitAction OnPlayerHit;
    public static event HitAction OnEnemyHit;
    //Alright so this is the base class for the 3 enemy types.
    // Need to note how many points are scored, how many shots it takes to kill, and need an attack method to be defined in the specific enemy classes
    public virtual void Start ()
    {
        if (pointsWorth != 0)
        {
            ResetHealth();
        }
        _thePlayer = GameObject.FindGameObjectWithTag("Player");
    }
	
    public abstract void Attack();//Every enemy type will have to define its own attack method

    void OnTriggerEnter(Collider collided)
    {
        if (collided.CompareTag("Bullet") && GetComponent<EnemyProjectile>()==null)
        {
            //We've been hit
            hitPoints -= 1;
            //Stop the bullet and move it offscreen
            var theBullet = collided.gameObject;
            theBullet.transform.position = new Vector3(100,1,0);
            Rigidbody bulletBody = theBullet.GetComponent<Rigidbody>();
            bulletBody.velocity = new Vector3(0, 0, 0);
            if (hitPoints <=0)
            {
                gameObject.SetActive(false);
                //Need to let the manager know to add our PointsWorth to playerScore
                if (OnEnemyHit !=null)
                {
                    OnEnemyHit(this,null);
                }
            }
        }
        if (collided.CompareTag("Player") && !collided.GetComponent<Player>().IsInvincible)
        {
            //Need to let the manager know that the player has been hit
            if (OnPlayerHit !=null)
            {
                OnPlayerHit(this,null);
            }
        }
    }
    public void ResetHealth()
    {
        hitPoints = pointsWorth / 100;
    }
}
