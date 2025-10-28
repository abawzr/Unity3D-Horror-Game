using System.Collections.Generic;
using UnityEngine;

public class ObjectiveItemsSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<ObjectiveItem> excludedObjectiveItemsFromRandomRespawn;

    private void Start()
    {
        int randomIndex;

        if (spawnPoints.Count == 0) return;

        foreach (ObjectiveItem objectiveItem in FindObjectsOfType<ObjectiveItem>(includeInactive: true))
        {
            if (excludedObjectiveItemsFromRandomRespawn.Contains(objectiveItem)) continue;

            randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
            objectiveItem.transform.position = spawnPoints[randomIndex].position;
            spawnPoints.Remove(spawnPoints[randomIndex]);
            objectiveItem.gameObject.SetActive(true);
        }
    }
}
