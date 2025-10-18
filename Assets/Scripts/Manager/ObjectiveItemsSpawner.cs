using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectiveItemsSpawner : MonoBehaviour
{
    [SerializeField] private List<Transform> spawnPoints;

    private void Start()
    {
        int randomIndex;

        foreach (ObjectiveItem objectiveItem in FindObjectsOfType<ObjectiveItem>(includeInactive: true))
        {
            randomIndex = UnityEngine.Random.Range(0, spawnPoints.Count);
            objectiveItem.transform.position = spawnPoints[randomIndex].position;
            spawnPoints.Remove(spawnPoints[randomIndex]);
            objectiveItem.gameObject.SetActive(true);
        }
    }
}
