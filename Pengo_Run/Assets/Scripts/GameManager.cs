﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    public static GameManager gm;
    private MissionBase[] missions;

    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
        else if (gm != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        missions = new MissionBase[2];

        for (int i = 0; i < missions.Length; i++)
        {
            GameObject newMission = new GameObject("Mission" + i);
            newMission.transform.SetParent(transform);
            MissionType[] missionType = { MissionType.SingleRun, MissionType.TotalMeter};
            int randomType = Random.Range(0, missionType.Length);
            if (randomType == (int)MissionType.SingleRun)
            {
                missions[i] = newMission.AddComponent<SingleRun>();
                missions[i].Created();
            }
            else if (randomType == (int)MissionType.TotalMeter)
            {
                missions[i] = newMission.AddComponent<TotalMeters>();
                missions[i].Created();
            }
        }
    }

    
    

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartRun()
    {
        SceneManager.LoadScene("Game");
    }

    public void EndRun()
    {
        SceneManager.LoadScene("Menu");
    }

    public void History()
    {
        SceneManager.LoadScene("History");
    }

    public MissionBase GetMission(int index)
    {
        return missions[index];
    }

    public void StartMissions()
    {
        for (int i = 0; i < 2; i++)
        {
            missions[i].RunStart();
        }
    }

    public void GenerateMission(int i)
    {
        Destroy(missions[i].gameObject);

        GameObject newMission = new GameObject("Mission" + i);
        newMission.transform.SetParent(transform);
        MissionType[] missionType = { MissionType.SingleRun, MissionType.TotalMeter };
        int randomType = Random.Range(0, missionType.Length);
        if (randomType == (int)MissionType.SingleRun)
        {
            missions[i] = newMission.AddComponent<SingleRun>();

        }
        else if (randomType == (int)MissionType.TotalMeter)
        {
            missions[i] = newMission.AddComponent<TotalMeters>();
        }
        
        missions[i].Created();

        FindObjectOfType<Menu>().SetMission();
    }
}
