using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    private Rigidbody2D rb;

    private int lives = GameplayConstants.STARTING_LIVES;
    private int distanceScore = 0;
    private int enemyScore = 0;
	
	void Start ()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        distanceScore = Mathf.Max(distanceScore, (int)this.transform.position.x);
        int totalScore = distanceScore * GameplayConstants.SCORE_DISTANCE_MULTIPLIER + enemyScore * GameplayConstants.SCORE_ENEMY_MULTIPLIER;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == GameplayConstants.TAG_KillZone)
        {
            KillCharacter();
        }
    }

    private void KillCharacter()
    {
        lives -= 1;

        if (lives > 0)
        {
            rb.MovePosition(rb.position + GameplayConstants.RESPAWN_HEIGHT * Vector2.up);
            rb.velocity = Vector2.zero;
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        Debug.Log("Game Over!");
    }
}
