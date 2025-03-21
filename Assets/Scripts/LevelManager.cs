using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField]
    Transform player, obstacles,points;
    [SerializeField]
    Transform[] platforms;
    [SerializeField]
    Text scoreTxt;
    [SerializeField]
    GameObject gameOverScene;

    Queue<Transform> obstaclesQ=new Queue<Transform>();
    Queue<Transform> pointsQ=new Queue<Transform>();

    int movablePlatformIndex=0, nextPlatformPos = 100;
    float lastObstaclePos, lastPointPos;
    int score;

    [HideInInspector]
    public bool play;

    private void Awake()
    {
        if (instance) Destroy(instance.gameObject);
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(PlatformMovement());

        for (int i = 0; i < obstacles.childCount; i++)
            obstaclesQ.Enqueue(obstacles.GetChild(i));
        lastObstaclePos = obstacles.GetChild(obstacles.childCount - 1).position.z;
        StartCoroutine(ObstacleMovement());

        for (int i = 0; i < points.childCount; i++)
            pointsQ.Enqueue(points.GetChild(i));
        lastPointPos = points.GetChild(points.childCount - 1).position.z;
        StartCoroutine(PointMovement());
    }

    IEnumerator PlatformMovement()
    {
        yield return new WaitUntil(() => player.position.z > nextPlatformPos);
        platforms[movablePlatformIndex].position += Vector3.forward * 200;
        movablePlatformIndex = movablePlatformIndex == 0 ? 1 : 0;
        nextPlatformPos += 100;
        Score();
        StartCoroutine(PlatformMovement());
    }

    IEnumerator ObstacleMovement()
    {
        Transform firstObj = obstaclesQ.Dequeue();
        yield return new WaitUntil(() => player.position.z > firstObj.position.z + 10);
        lastObstaclePos += Random.Range(30,100);
        firstObj.position = Vector3.forward * lastObstaclePos;
        obstaclesQ.Enqueue(firstObj);
        StartCoroutine(ObstacleMovement());

        NetworkManager.instance.SendData("ObstacleMov", firstObj.name + "_"+lastObstaclePos);
    }
    IEnumerator PointMovement()
    {
        Transform firstObj = pointsQ.Dequeue();
        yield return new WaitUntil(() => player.position.z > firstObj.position.z + 10);
        lastPointPos += Random.Range(3, 50);
        firstObj.position = new Vector3(0, 0.5f, lastPointPos);
        firstObj.gameObject.SetActive(true);
        pointsQ.Enqueue(firstObj);
        StartCoroutine(PointMovement());

        NetworkManager.instance.SendData("PointMov", firstObj.name + "_" + lastPointPos);
    }

    public void Point(Transform point)
    {
        Score();
        point.gameObject.SetActive(false);

        NetworkManager.instance.SendData("PointHit", point.name);
    }

    void Score()
    {
        score++;
        scoreTxt.text = "Score:" + score;

        NetworkManager.instance.SendData("ScoreSync", score.ToString());
    }

    public void GameOver()
    {
        play = false;
        gameOverScene.SetActive(true);
    }
}
