using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyManager : MonoBehaviour
{
    public float enemySpeed; //Speed of the enemies
    public int rocketNum; //Number of rockets the red enemies fire
    private int _enemyCount; //Number of enemies in the scene. The GameManager uses this number to determine when to end the game
    private Dictionary<GameObject, Vector3> enemyDict;//This will be used to put the enemy back in its original spot in the formation
    private List<GameObject> availableAttackers;
    private float attackTimer, hoverTimer;
    private Vector3 hoverTarget;
    private bool _startGame;

    public int EnemyCount
    {
        get { return _enemyCount;}
        set { _enemyCount = value;}
    }

    public bool StartGame
    { get { return _startGame; }
      set { _startGame = value; }
    }

    void Awake ()
    {
        var tempGrp = gameObject.GetComponentsInChildren<Enemy>();
        availableAttackers = new List<GameObject>();
        foreach (var enemy in tempGrp)
        {
            if (enemy.GetComponent<EnemyProjectile>() == null)
            {
                enemy.MovementSpeed = enemySpeed;
                if (enemy.GetComponent<RedEnemy>() !=null)
                {
                    enemy.GetComponent<RedEnemy>().RocketNum = rocketNum;
                }
                availableAttackers.Add(enemy.gameObject);
            }
        }
        _enemyCount = availableAttackers.Count;
        enemyDict = new Dictionary<GameObject, Vector3>();
        foreach(GameObject enemy in availableAttackers)
        {
            enemyDict.Add(enemy, enemy.transform.localPosition);
        }
        hoverTarget = DetermineHoverTarget();
	}
	
	void Update ()
    {
        if (_startGame)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= 3.0f && availableAttackers.Count > 0)
            {
                DetermineAttackers();
                attackTimer = 0;
            }
            hoverTimer += Time.deltaTime;
            HoverEnemyGroup(hoverTarget);
            if (hoverTimer >= 2)
            {
                hoverTarget = DetermineHoverTarget();
                hoverTimer = 0;
            }
        }
	}

    private Vector3 DetermineHoverTarget()
    {
        var randomX = 0;
        randomX = (transform.position.x>0)?UnityEngine.Random.Range(-1, -2): UnityEngine.Random.Range(1, 2);
        return new Vector3(randomX,transform.position.y,UnityEngine.Random.Range(2.7f,4));
    }

    private void HoverEnemyGroup(Vector3 targetPos)
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, 1.5f * Time.deltaTime);
    }

    private void OnEnable()
    {
        ReturnEnemyTrigger.OnEnemyEnter += ReturnEnemy;
    }
    private void OnDisable()
    {
        ReturnEnemyTrigger.OnEnemyEnter -= ReturnEnemy;
    }

    private void ReturnEnemy(object sender, EventArgs e)
    {
        var returnedEnemy = (GameObject)sender;
        returnedEnemy.GetComponent<Enemy>().CanAttack = false;
        if (returnedEnemy.GetComponent<RedEnemy>() !=null)
        {
            returnedEnemy.GetComponent<RedEnemy>().HaveFired = false;
            //Need to destroy any rockets that were fired late so they won't be firing once we're reset
            if (returnedEnemy.transform.GetComponentsInChildren<EnemyProjectile>() !=null)
            {
                var myRockets = returnedEnemy.transform.GetComponentsInChildren<EnemyProjectile>().ToList();
                foreach (var rocket in myRockets)
                {
                    Destroy(rocket.gameObject);
                }
            }
        }
        if (returnedEnemy.GetComponent<GreenEnemy>() != null)
        {
            var myGreenEnemy = returnedEnemy.GetComponent<GreenEnemy>();
            myGreenEnemy.BeamDone = false;
            myGreenEnemy.BeamFired = false;
            myGreenEnemy.energyBeam.transform.localScale = new Vector3(myGreenEnemy.energyBeam.transform.localScale.x, myGreenEnemy.energyBeam.transform.localScale.y, 0);
        }
        returnedEnemy.transform.localPosition = enemyDict[returnedEnemy];
        availableAttackers.Add(returnedEnemy);
    }

    private void DetermineAttackers()
    {
        //Need to go through the enemyGroup and randomly decide which enemies are attacking. Then set 'CanAttack' to true on those enemies
        var numberOfAttackers = UnityEngine.Random.Range(1, 4);
        for (int i = 0; i < numberOfAttackers; i++)
        {
            var chosenEnemy = availableAttackers[UnityEngine.Random.Range(0, availableAttackers.Count - 1)];
            if (!chosenEnemy.GetComponent<Enemy>().CanAttack)
            {
                chosenEnemy.GetComponent<Enemy>().CanAttack = true;
                availableAttackers.Remove(chosenEnemy);
                if (availableAttackers.Count <=0)
                {
                    break;
                }
            }
            if (chosenEnemy.GetComponent<GreenEnemy>() !=null)
            {
                //Only one green enemy should be attacking at a time
                break;
            }
        }        
    }

    public void Reset()
    {
        _startGame = false;
        attackTimer = 0;
        hoverTimer = 0;
        foreach(GameObject enemy in enemyDict.Keys)
        {
            enemy.SetActive(true);
            enemy.GetComponent<Enemy>().CanAttack = false;
            enemy.GetComponent<Enemy>().ResetHealth();
            ReturnEnemy(enemy, null);
        }
        var enemyRocketList =FindObjectsOfType<EnemyProjectile>().ToList();
        //This gets rid of any lingering rockets that were fired when we had to reset
        foreach(var rocket in enemyRocketList)
        {
            if (rocket.isRocket)
            {
                Destroy(rocket.gameObject);
            }
        }
    }
}
