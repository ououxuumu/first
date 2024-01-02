using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntController : MonoBehaviour
{
    public GameObject antPrefab; // 螞蟻的預置物
    public GameObject food;
    public int numberOfAnts = 10; // 螞蟻的數量
    public float antSpeed = 3f;
    public float changeDirectionInterval = 5f; // 改變方向的間隔時間
    public float detectionRadius = 1f;
    public float movementBoundary = 10f;

    private List<GameObject> ants = new List<GameObject>();
    private Vector3 globalTargetPosition;
    private float nextChangeDirectionTime;

    void Start()
    {
        SpawnAnts();
        nextChangeDirectionTime = Time.time + changeDirectionInterval;
    }

    void Update()
    {
        MoveAnts();
        CheckChangeDirection();
    }

    void SpawnAnts()
    {
        for (int i = 0; i < numberOfAnts; i++)
        {
            Vector3 spawnPosition = new Vector3(i - numberOfAnts / 2f, 0f, 0f); // 在 x 軸上均勻排列
            GameObject ant = Instantiate(antPrefab, spawnPosition, Quaternion.identity);
            ants.Add(ant);
        }

        globalTargetPosition = food.transform.position; // 初始目標位置為食物位置
    }

    void MoveAnts()
    {
        foreach (GameObject ant in ants)
        {
            Vector3 targetDirection = (globalTargetPosition - ant.transform.position).normalized;
            Vector3 newPosition = ant.transform.position + targetDirection * antSpeed * Time.deltaTime;
            MoveAnt(ant, newPosition);

            CheckAntReachedFood(ant);
        }
    }

    void MoveAnt(GameObject ant, Vector3 targetPosition)
    {
        targetPosition.x = Mathf.Clamp(targetPosition.x, -movementBoundary, movementBoundary);
        targetPosition.z = Mathf.Clamp(targetPosition.z, -movementBoundary, movementBoundary);

        ant.transform.position = targetPosition;
    }

    void CheckAntReachedFood(GameObject ant)
    {
        float distanceToFood = Vector3.Distance(ant.transform.position, food.transform.position);

        if (distanceToFood < detectionRadius)
        {
            Debug.Log("Ant reached the food!");
            // 在這裡你可以添加相應的處理，例如刪除該螞蟻物件等。
        }
    }

    void CheckChangeDirection()
    {
        if (Time.time >= nextChangeDirectionTime)
        {
            globalTargetPosition = food.transform.position; // 改變目標位置為食物位置
            nextChangeDirectionTime = Time.time + changeDirectionInterval;
        }
    }
}
