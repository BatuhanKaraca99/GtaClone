using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceSpawner : MonoBehaviour
{
    public GameObject[] AIPrefab;
    public int AIToSpawn;

    private void Start()
    {
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        int count = 0;
        while (count < AIToSpawn)
        {
            int randomIndex = Random.Range(0, AIPrefab.Length);

            GameObject obj = Instantiate(AIPrefab[randomIndex]);

            Transform child = transform.GetChild(Random.Range(0, transform.childCount - 1));
            obj.GetComponent<PoliceWaypointNavigator>().currentWaypoint = child.GetComponent<Waypoint>();

            obj.transform.position = child.position;

            yield return new WaitForSeconds(5f);

            count++;
        }
    }
}
