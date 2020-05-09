using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject[] platformPrefabs;
    public Transform player;
    public float jumpDistance = 10f;

    [Tooltip("Scale of a normal jump that's considered easy.")]
    [Range(0.1f, 1f)]
    public float easyDistanceScale = 0.5f;
    [Tooltip("Scale of a normal jump that's considered hard.")]
    [Range(1f, 2f)]
    public float hardDistanceScale = 1.5f;

    [Tooltip("Distance before which an easy jump is automatic.")]
    public float easyDistance = 100f;
    [Tooltip("Distance after which a hard jump is automatic.")]
    public float hardDistance = 1000f;

    private PlatformSection[] platformSections;
    private int[] currentSections;
    private float currentEndDistance = GameplayConstants.START_DISTANCE;

	void Start ()
    {
        InstantiatePlatformSections();
        BuildInitialLevel();
	}

    private void InstantiatePlatformSections()
    {
        platformSections = new PlatformSection[platformPrefabs.Length];

        GameObject poolParent = new GameObject("Platform_Pool");
        poolParent.transform.position = Vector3.zero;

        for (int i = 0; i < platformPrefabs.Length; i++)
        {
            GameObject clone = Instantiate(platformPrefabs[i], 0f * Vector3.left, Quaternion.identity);
            clone.transform.parent = poolParent.transform;
            platformSections[i] = clone.GetComponent<PlatformSection>();
            platformSections[i].Initialize(this);
            platformSections[i].Deactivate();
        }
    }

    private void BuildInitialLevel()
    {
        //Debug.Log("BuildInitialLevel");

        currentSections = new int[GameplayConstants.MAXIMUM_SECTIONS];
        for (int i = 0; i < GameplayConstants.MAXIMUM_SECTIONS - 1; i++)
        {
            AddPlatformSection();
        }

        for (int i = 0; i < GameplayConstants.MAXIMUM_SECTIONS - 2; i++)
        {
            platformSections[currentSections[i]].SetAlreadyActivated();
        }
    }

    public void AddPlatformSection()
    {
        //Debug.Log("AddPlatformSection");

        AdvanceCurrentPlatformCache();
        RandomUniquePlatform();
        ActivateNewPlatform();
    }

    private void AdvanceCurrentPlatformCache()
    {
        //Debug.Log("AdvanceCurrentPlatformCache");

        for (int i = 1; i < GameplayConstants.MAXIMUM_SECTIONS - 1; i++)
        {
            currentSections[i] = currentSections[i + 1];
        }
    }

    private void RandomUniquePlatform()
    {
        //Debug.Log("RandomUniquePlatform");

        bool duplicatePlatformSelected = true;
        int newPlatformIndex = -1;
        while (duplicatePlatformSelected)
        {
            newPlatformIndex = Random.Range(0, platformSections.Length);
            duplicatePlatformSelected = false;
            for (int i = 0; i < currentSections.Length; i++)
            {
                if (newPlatformIndex == currentSections[i])
                {
                    duplicatePlatformSelected = true;
                    break;
                }
            }
        }

        currentSections[GameplayConstants.MAXIMUM_SECTIONS - 1] = newPlatformIndex;
    }

    private void ActivateNewPlatform()
    {
        //Debug.Log("ActivateNewPlatform");

        PlatformSection newPlatform = platformSections[currentSections[GameplayConstants.MAXIMUM_SECTIONS - 1]];

        float jumpScalar = GetJumpScalar();

        currentEndDistance = newPlatform.MoveAndActivate(currentEndDistance + jumpScalar * jumpDistance);
    }

    private float GetJumpScalar()
    {
        float percentAlongScale = Mathf.InverseLerp(easyDistance, hardDistance, player.position.x);
        float difficultyScale = Mathf.Lerp(easyDistanceScale, hardDistanceScale, percentAlongScale);
        return difficultyScale;
    }
}
