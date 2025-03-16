using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigManager
{
    public static ConfigManager Instance { get; private set; }

    public ConfigDatabase<string, MovingObstacleData> MovingObstacleConfig { get; private set; } = new ConfigDatabase<string, MovingObstacleData>();

    private ConfigManager() { }

    public static void Init()
    {
        if (Instance == null)
        {
            Instance = new ConfigManager();
            Instance.LoadAllConfigs();
        }
    }

    private void LoadAllConfigs()
    {
        MovingObstacleConfig.LoadConfig("Configs/MovingObstacle");
    }

    public TData GetConfigData<TKey, TData>(TKey dataID) where TData : class, IConfigData<TKey>
    {
        if (typeof(TData) == typeof(MovingObstacleData))
        {
            return MovingObstacleConfig.GetDataByKey((string)(object)dataID) as TData;
        }
        return null;
    }
}
