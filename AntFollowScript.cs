using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntFollowScript : MonoBehaviour
{
    public GameObject[] ants;
    public GameObject food;
    public float randomWalkDuration = 10f;
    public float antSpeed = 2f;
    public float followDistance = 1.5f;

    private float timer;
    private bool isFollowing = false;
    private Vector3[] antPath;

    void Start()
    {
        timer = randomWalkDuration;

        // 初始化所有螞蟻位置
        InitializeAntsPosition();

        // 計算 ant1 到食物的路徑
        antPath = CalculatePath(ants[0].transform.position, food.transform.position);
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer > 0f)
        {
            // 十秒內隨機行走
            RandomWalk();
        }
        else if (!isFollowing)
        {
            // 十秒後開始跟隨
            isFollowing = true;
            FollowPath();
        }
    }

    void InitializeAntsPosition()
    {
        foreach (GameObject ant in ants)
        {
            ant.transform.position = new Vector3(Random.Range(-5f, 5f), 0.5f, Random.Range(-5f, 5f));
        }
    }

    void RandomWalk()
    {
        foreach (GameObject ant in ants)
        {
            ant.transform.Translate(Vector3.forward * antSpeed * Time.deltaTime);
        }
    }

    void FollowPath()
    {
        for (int i = 1; i < ants.Length; i++)
        {
            Vector3 targetPosition = antPath[Mathf.Clamp(i, 0, antPath.Length - 1)];
            Vector3 currentPosition = ants[i].transform.position;

            Vector3 direction = (targetPosition - currentPosition).normalized;
            ants[i].transform.Translate(direction * antSpeed * Time.deltaTime);

            // 調整螞蟻方向
            ants[i].transform.LookAt(targetPosition);
        }
    }

    Vector3[] CalculatePath(Vector3 start, Vector3 end)
    {
        // 這裡可以使用任何路徑規劃演算法，這裡使用簡單的線性插值
        int segments = ants.Length;
        Vector3[] path = new Vector3[segments];

        for (int i = 0; i < segments; i++)
        {
            float t = i / (float)(segments - 1);
            path[i] = Vector3.Lerp(start, end, t);
        }

        return path;
    }
}
