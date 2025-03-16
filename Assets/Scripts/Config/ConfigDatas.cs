using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacleData : IConfigData<string>
{
    public string ID { get; set; }
    public int Order { get; set; }
    public string SceneName { get; set; }
    public int ObstacleInSceneID { get; set; }
    public Vector3 StartPoint { get; set; }
    public Vector3 EndPoint { get; set; }
    public float Period { get; set; }
}
