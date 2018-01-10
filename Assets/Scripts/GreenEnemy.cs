using UnityEngine;

public class GreenEnemy : Enemy
{
    public int myPoints;
    public GameObject energyBeam;
    private float beamTimer;
    private bool beamFired;
    private bool beamDone;

    public bool BeamFired
    {
        get { return beamFired;}

        set { beamFired = value;}
    }

    public bool BeamDone
    {
        get { return beamDone;}

        set { beamDone = value;}
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
        if (beamFired && !beamDone)
        {
            beamTimer += Time.deltaTime;
            energyBeam.transform.localScale = new Vector3(energyBeam.transform.localScale.x, energyBeam.transform.localScale.y,Mathf.MoveTowards(energyBeam.transform.localScale.z, .7f, .75f * Time.deltaTime));
            if (beamTimer >=3)
            {
                energyBeam.transform.localScale = new Vector3(energyBeam.transform.localScale.x, energyBeam.transform.localScale.y, 0);
                beamDone = true;
                beamTimer = 0;
            }
        }
    }
    public override void Attack()
    {
        //Can shoot energy beam
        //Attacks by flying above Player position and shooting energy beam
        if (!beamFired)
        {
            Vector3 targetPos = ThePlayer.transform.position;
            targetPos = new Vector3(targetPos.x, targetPos.y, targetPos.z + 3);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, MovementSpeed * Time.deltaTime);
            //Once we get into position, need to fire beam. Then continue flying off screen
            if (Vector3.Distance(transform.position, targetPos) < .6f && !beamFired)
            {
                //We're in position, so fire the beam
                beamFired = true;
            }
        }
        if (beamDone)
        {
            //Alright, we fired the beam so now we just need to fly off screen
            Vector3 targetPos = ThePlayer.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetPos.x, targetPos.y, targetPos.z - 5), MovementSpeed * Time.deltaTime);
        }
    }
}
