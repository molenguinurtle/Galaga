using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int playerLives = 3;
    public int playerScore;
    public Text scoreText;
    public Text livesText;
    public Text gameOverText;
    public Player myPlayer;
    public EnemyManager enemyManager;
    private int killedCount; //The number of enemies the player has killed. Used to determine when to end the game.
    private int playerLivesHolder; //Set this at the start so that on reset, we have a reference to what the lives should be
    private bool endedGame;

    void Start ()
    {
        playerLivesHolder = playerLives;
        UpdateTheScore();
        UpdateTheLives();
        myPlayer.CanMove = true;
        enemyManager.StartGame = true;
	}

    private void Update()
    {
        if (endedGame)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Restart the game
                playerLives = playerLivesHolder;
                killedCount = 0;
                playerScore = 0;
                UpdateTheScore();
                UpdateTheLives();
                gameOverText.gameObject.SetActive(false);
                myPlayer.CanMove = true;
                enemyManager.StartGame = true;
                endedGame = false;
            }
        }
    }
    private void OnEnable()
    {
        Enemy.OnPlayerHit += Enemy_OnPlayerHit;
        Enemy.OnEnemyHit += AddToScore;
    }
    private void OnDisable()
    {
        Enemy.OnPlayerHit -= Enemy_OnPlayerHit;
        Enemy.OnEnemyHit -= AddToScore;
    }

    private void AddToScore(object sender, EventArgs e)
    {
        var myEnemy = (Enemy)sender;
        playerScore += myEnemy.PointsWorth;
        UpdateTheScore();
        //Need to check here if we've destroyed all the enemies. If so, end the game.
        killedCount++;
        if (killedCount >= enemyManager.EnemyCount)
        {
            EndGame(false);
        }
    }

    private void Enemy_OnPlayerHit(object sender, EventArgs args)
    {
        playerLives -= 1;
        //Will need to take a moment and put in the next player ship while giving a couple seconds of invulnerability
        myPlayer.IsInvincible = true;
        StartCoroutine(PlayerInvincibility());
        if (playerLives <=0)
        {
            EndGame(true);
        }
        else
        {
            UpdateTheLives();
        }
    }
    IEnumerator PlayerInvincibility()
    {
        for (int i =0;i<3;i++)
        {
            myPlayer.GetComponent<Renderer>().enabled = false;
            yield return new WaitForSeconds(.1f);
            myPlayer.GetComponent<Renderer>().enabled = true;
            yield return new WaitForSeconds(.1f);
        }
        myPlayer.GetComponent<Renderer>().enabled = true;
        myPlayer.IsInvincible = false;

    }
    private void EndGame(bool playerLost)
    {
        myPlayer.CanMove = false;
        foreach(var bullet in myPlayer.bullets)
        {
            bullet.transform.localPosition = new Vector3(0, 0, 0);
        }
        if (playerLost)
        {
            gameOverText.gameObject.SetActive(true);
        }
        enemyManager.Reset();
        endedGame = true;
    }

    //On the spacebar press, we need to set score and killCount back to 0. Also player lives back to whatever it was.
    private void UpdateTheScore()
    {
        scoreText.text = "Score: " + playerScore.ToString();
    }

    private void UpdateTheLives()
    {
        livesText.text = "Lives Left: " + (playerLives-1).ToString();
    }
}
