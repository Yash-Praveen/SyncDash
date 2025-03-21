using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    private void Awake()
    {
        if (instance) Destroy(instance.gameObject);
        instance = this;
    }

    #region sender

    public void SendPos(Vector3 pos)
    {
        GetPos(pos);
    }

    public void SendData(string label,string data)
    {
        GetData(label, data);
    }

    #endregion
    #region receiver

    [SerializeField]
    Transform mirrorPlayer;
    public void GetPos(Vector3 pos)
    {
        mirrorPlayer.position = pos + Vector3.right * 1000;
    }
    public void GetData(string label, string data)
    {
        if (label == "ObstacleMov")
            RemoteLevelManager.instance.ObstacleMovement(data);
        else if (label == "PointMov")
            RemoteLevelManager.instance.PointMovement(data);
        else if (label == "ScoreSync")
            RemoteLevelManager.instance.SetScore(data);
        else if (label == "PointHit")
            RemoteLevelManager.instance.PointHit(data);
    }

    #endregion
}
