using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigDatabase<TKey, TData> where TData : class, IConfigData<TKey>, new()
{
    private Dictionary<TKey, TData> _dataDictionary = new Dictionary<TKey, TData>();
    private string _filePath = string.Empty;

    public void LoadConfig(string filePath)
    {
        _dataDictionary.Clear();
        _filePath = filePath;
        var dataList = CSVLoader.LoadConfig<TData>(filePath);
        for (int i = 0; i < dataList.Count; i++)
        {
            _dataDictionary[dataList[i].ID] = dataList[i];
        }
    }

    public void Reload()
    {
        LoadConfig(_filePath);
    }

    public TData GetDataByKey(TKey dataID)
    {
        if (_dataDictionary.TryGetValue(dataID, out TData value))
        {
            return value;
        }
        
        Debug.LogError($"ConfigDatabase: Can't find {typeof(TData).Name} with ID {dataID}");
        return null;
    }
}
