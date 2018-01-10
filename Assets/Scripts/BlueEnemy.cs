using UnityEngine;

public class BlueEnemy : Enemy
{
    public int myPoints;

	// Use this for initialization
	public override void Start ()
    {
        PointsWorth = myPoints;
        base.Start();
	}

    //OK, so we're gonna need an Enemy Manager. This will determine when and which enemies attack
    //We'll also need each enemy to be able to return to its spot in line. That might be a separate method that is defined in Enemy
    void Update ()
    {
        if (CanAttack)
        {
            Attack();
        }
	}
    public override void Attack()
    {
        //Attacks by flying into Player’s position
        //OK, I think you need to have the enemy fly forward a little bit THEN turn towards the player's position. Then just go in that direction, not
        // necessarily to where the player currently is. 
        Vector3 targetPos = ThePlayer.transform.position;
        transform.position = Vector3.MoveTowards(transform.position,new Vector3( targetPos.x,targetPos.y,targetPos.z-5), MovementSpeed * Time.deltaTime);
        //Once the enemy is offscreen, have it return to its place in the formation from the top of the screen. Will need Enemy manager + separate
        // 'Return' function to do that
    }
}
