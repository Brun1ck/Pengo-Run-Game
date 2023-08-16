using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Track : MonoBehaviour
{
    public GameObject[] obstacles;
    public Vector2 numberofObstacles;


    public List<GameObject> newObstacles;

    // Start is called before the first frame update
    void Start()
    {
        int newNumberofObstacles = (int)Random.Range(numberofObstacles.x, numberofObstacles.y);
       

        for (int i = 0; i < newNumberofObstacles; i++)
        {
            newObstacles.Add(Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform));
            newObstacles[i].SetActive(false);
        }

        PosicionateObstacles();
    }

    void PosicionateObstacles()
    {
        for (int i = 0; i < newObstacles.Count; i++)
        {
            float posZMin = (67.1f / newObstacles.Count) + (67.1f / newObstacles.Count) * i;
            float posZMax = (67.1f / newObstacles.Count) + (67.1f / newObstacles.Count) * i + 1;
            newObstacles[i].transform.localPosition = new Vector3(0, 0, Random.Range(posZMin, posZMax));
            newObstacles[i].SetActive(true);
            if (newObstacles[i].GetComponent<ChangeLine>() != null)
                newObstacles[i].GetComponent<ChangeLine>().PositionLane();
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().IncreaseSpeed();
            transform.position = new Vector3(0, 0, transform.position.z + 66f * 2);
            PosicionateObstacles();
           
        }
    }
}
