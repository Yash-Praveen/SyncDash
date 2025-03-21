using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemoteLevelManager : MonoBehaviour
{
    public static RemoteLevelManager instance;

    [SerializeField]
    Transform player, obstacles, points;
    [SerializeField]
    Transform[] platforms;
    [SerializeField]
    Text scoreTxt;

    int movablePlatformIndex = 0, nextPlatformPos = 100;

    private void Awake()
    {
        if (instance) Destroy(instance.gameObject);
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(PlatformMovement());
    }
    IEnumerator PlatformMovement()
    {
        yield return new WaitUntil(() => player.position.z > nextPlatformPos);
        platforms[movablePlatformIndex].position += Vector3.forward * 200;
        movablePlatformIndex = movablePlatformIndex == 0 ? 1 : 0;
        nextPlatformPos += 100;
        StartCoroutine(PlatformMovement());
    }

    public void ObstacleMovement(string data)
    {
        string[] str = data.Split('_');
        FindObject(obstacles, str[0]).position = new Vector3(1000,0,(float)Convert.ToDouble(str[1]));
    }
    public void PointMovement(string data)
    {
        string[] str = data.Split('_');
        Transform obj = FindObject(points, str[0]);
        obj.position = new Vector3(1000, 0.5f, (float)Convert.ToDouble(str[1]));
        obj.gameObject.SetActive(true);
    }

    public void PointHit(string data)
    {
        FindObject(points, data).gameObject.SetActive(false);
    }

    Transform FindObject(Transform parent,string name)
    {
        for (int i = 0; i < parent.childCount; i++)
            if (parent.GetChild(i).name == name)
                return parent.GetChild(i);
        return null;
    }

    public void SetScore(string score)
    {
        scoreTxt.text= "Score:" + score;
    }
}