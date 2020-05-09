using UnityEngine;

public class Platform : MonoBehaviour
{
	void Awake()
	{
        this.tag = GameplayConstants.TAG_Ground;    // If you get an error here, create a Tag in Unity called "Ground".
        //See the GameplayConstants.cs file for other required Tags and Layers.
        
        SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer> ();
		if (spriteRenderer != null)
		{
            MatchColliderToSpriteSize(spriteRenderer);
		}
	}

    private void MatchColliderToSpriteSize(SpriteRenderer spriteRenderer)
    {
        BoxCollider2D coll = this.GetComponent<BoxCollider2D>();
        if (coll == null)
        {
            coll = this.gameObject.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
        }

        coll.size = spriteRenderer.size;
        coll.offset = 0.5f * spriteRenderer.size.y * Vector2.up;
    }
}
