﻿using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    int score;
    void Awake()
    {
        //ñ
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != null)
             Destroy(gameObject);
        
    
    }
    public void SetScore(int value)
    {
        score = value;
    }
    public int GetScore()
    {
        return score;
    }
}
