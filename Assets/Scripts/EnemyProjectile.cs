using UnityEngine;

public class EnemyProjectile : Enemy
{
    public bool isRocket;

    void Update ()
    {
        if (isRocket)
        {
            Attack();
        }
	}
    public override void Attack()
    {
        Vector3 targetPos = ThePlayer.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x, targetPos.y, targetPos.z - 5), 2.0f * Time.deltaTime);
    }
}
