using System.Collections.Generic;
using UnityEngine;

public class PlatformSection : MonoBehaviour
{
    internal struct SpawnPoint
    {
        Vector3 center;
        float range;

        internal SpawnPoint(Vector3 spawnCenter, float spawnRange)
        {
            center = spawnCenter;
            range = spawnRange;
        }

        internal Vector3 RandomSpawnPoint()
        {
            return center + Random.Range(-range, range) * Vector3.right;
        }
    }
    
    private float leftOffset;
    private float rightOffset;
    private SpawnPoint[] spawnPoints;

    private PlatformManager pm;
    private bool nextPlatformActivated = false;

    private void CalculateWidthFromColliders(BoxCollider2D[] colliders)
    {
        
        leftOffset = float.MinValue;
        rightOffset = float.MinValue;

        foreach (BoxCollider2D col in colliders)
        {
            float localMin = this.transform.position.x - (col.bounds.min).x;
            leftOffset = Mathf.Max(localMin, leftOffset);

            float localMax = (col.bounds.max).x - this.transform.position.x;
            rightOffset = Mathf.Max(localMax, rightOffset);
        }
    }

    private void CalculateSpawnPoints(BoxCollider2D[] colliders)
    {
        List<SpawnPoint> spawnColliders = new List<SpawnPoint>();
        
        foreach (BoxCollider2D col in colliders)
        {
            if (col.gameObject.layer == GameplayConstants.LAYER_Radar)
            {
                continue;
            }
            
            float colExtent = col.bounds.extents.x;

            if (colExtent < 0.5 * GameplayConstants.SPAWN_ZONE_MINIMUM_WIDTH)
            {
                // Failed base width minimum.
                continue;
            }

            Vector2 colTop = (Vector2)col.bounds.center + new Vector2(0f, col.size.y + GameplayConstants.SLIP_ZONE_WIDTH);
            RaycastHit2D rayHit = Physics2D.Raycast(colTop, Vector2.up, GameplayConstants.SPAWN_ZONE_MINIMUM_HEIGHT);

            if (rayHit.collider != null)
            {
                // Failed height minimum.
                continue;
            }

            rayHit = Physics2D.Raycast(colTop, Vector2.right);
            float rightRange = colExtent;
            if (rayHit.collider != null)
            {
                rightRange = Mathf.Min(colExtent, rayHit.distance);
            }

            if (rightRange <= GameplayConstants.SLIP_ZONE_WIDTH)
            {
                // Failed minimum right width.
                continue;
            }

            rayHit = Physics2D.Raycast(colTop, Vector2.left);
            float leftRange = colExtent;
            if (rayHit.collider != null)
            {
                leftRange = Mathf.Min(colExtent, rayHit.distance);
            }

            if (leftRange <= GameplayConstants.SLIP_ZONE_WIDTH)
            {
                // Failed minimum left width.
                continue;
            }

            Vector3 spawnCenter = colTop + new Vector2(0.5f * (rightRange - leftRange), GameplayConstants.SPAWN_ZONE_DROP_HEIGHT - GameplayConstants.SLIP_ZONE_WIDTH);
            
            SpawnPoint point = new SpawnPoint(spawnCenter, 0.5f * (rightRange + leftRange));
            spawnColliders.Add(point);
        }

        spawnPoints = spawnColliders.ToArray();
    }

    private void SetTrigger()
    {
        BoxCollider2D coll = this.GetComponent<BoxCollider2D>();
        if (coll == null)
        {
            coll = this.gameObject.AddComponent<BoxCollider2D>();
        }

        coll.size = new Vector2(1f, 64f);
        coll.offset = leftOffset * Vector2.left;
        coll.isTrigger = true;
    }

    internal void Initialize(PlatformManager platformManager)
    {
        pm = platformManager;

        BoxCollider2D[] colliders = this.GetComponentsInChildren<BoxCollider2D>();
        CalculateWidthFromColliders(colliders);
        CalculateSpawnPoints(colliders);
        SetTrigger();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (!nextPlatformActivated && coll.tag == GameplayConstants.TAG_Player)
        {
            nextPlatformActivated = true;
            pm.AddPlatformSection();
        }
    }

    public float MoveAndActivate(float horizontalDistance)
    {
        float center = horizontalDistance + leftOffset;
        this.transform.position = center * Vector3.right;
        this.gameObject.SetActive(true);
        nextPlatformActivated = false;

        return center + rightOffset;
    }

    public Vector3[] GetEnemySpawnPoints(int number)
    {
        if (spawnPoints.Length < 1)
        {
            return null;
        }
        
        Vector3[] returnPoints = new Vector3[number];

        for (int i = 0; i < number; i++)
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);

            returnPoints[i] = this.gameObject.transform.position + spawnPoints[spawnPointIndex].RandomSpawnPoint();
        }

        return returnPoints;
    }

    public void SetAlreadyActivated()
    {
        nextPlatformActivated = true;
    }

    public void Deactivate()
    {
        this.gameObject.SetActive(false);
    }
}
