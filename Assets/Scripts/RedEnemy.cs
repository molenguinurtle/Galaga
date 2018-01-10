using UnityEngine;

public class RedEnemy : Enemy
{
    public int myPoints;
    private bool canFire;
    private bool haveFired;
    public GameObject theRocket;
    private int _rocketNum;

    public bool HaveFired
    {
        get { return haveFired;}
        set { haveFired = value;}
    }

    public int RocketNum
    {
        get { return _rocketNum;}
        set { _rocketNum = value;}
    }

    // Use this for initialization
    public override void Start()
    {
        PointsWorth = myPoints;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanAttack)
        {
            Attack();
        }
        if (canFire)
        {
            FireRockets(_rocketNum);
        }
    }

    private void FireRockets(int amountToFire)
    {
        //Since the number of rockets is configurable, we'll use instantiation for them
        for (int i =0; i<amountToFire;i++)
        {
            var newRocket = Instantiate(theRocket, transform, true);
            newRocket.transform.position = new Vector3(Random.Range(transform.position.x - .5f, transform.position.x + .5f), transform.position.y, Random.Range(transform.position.z - .5f, transform.position.z + .5f));
        }
        canFire = false;
        HaveFired = true;
    }

    public override void Attack()
    {
        //Can shoot N rockets, N is configurable
        //Attacks by flying near Player and shooting the rockets
        Vector3 targetPos = ThePlayer.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x, targetPos.y, targetPos.z - 5), MovementSpeed* Time.deltaTime);
        if (Vector3.Distance(transform.position,ThePlayer.transform.position)<4 && !HaveFired)
        {
            //We're close enough so Fire the rockets.
            canFire = true;
        }
    }
}
