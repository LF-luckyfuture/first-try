using UnityEngine;
using System.Collections.Generic;

public class SnakeControl : MonoBehaviour
{
    public Transform snakeHead; // 蛇头
    public GameObject snakeBodyPrefab; // 蛇身预制体
    private Vector2 moveDirection = Vector2.right; // 初始方向向右
    private float moveInterval = 0.2f; // 移动间隔（越小越快）
    private float timer; // 计时用
    private List<Transform> snakeBody = new List<Transform>(); // 存储蛇身
    public float maxX = 9.5f;
    public float minX = -9.5f;
    public float maxY = 4.5f;
    public float minY = -4.5f;
    void Update()
    {
        // 计时：达到间隔时间就移动一次
        timer += Time.deltaTime;
        if (timer >= moveInterval)
        {
            MoveSnake();
            timer = 0;
        }
        if (Input.GetKeyDown(KeyCode.W))
            moveDirection = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.S))
            moveDirection = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.A))
            moveDirection = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.D))
            moveDirection = Vector2.right;
    }

    // 蛇的移动逻辑
    void MoveSnake()
    {
        // 记录蛇头移动前的位置（给蛇身跟随用）
        Vector2 oldHeadPos = snakeHead.position;
        // 移动蛇头
        snakeHead.Translate(moveDirection);

        // 如果有蛇身，让最后一节蛇身移到蛇头原来的位置
        if (snakeBody.Count > 0)
        {
            Transform lastBody = snakeBody[snakeBody.Count - 1];
            lastBody.position = oldHeadPos;
            // 将最后一节蛇身移到列表最前面（实现“跟随”效果）
            snakeBody.RemoveAt(snakeBody.Count - 1);
            snakeBody.Insert(0, lastBody);
        }
    }
    // 检测蛇头与其他物体的碰撞（吃食物/撞墙/撞自己）
    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. 碰到食物：蛇变长，食物重新生成
        if (other.CompareTag("Food"))
        {
            GrowSnake(); // 蛇变长
            SpawnFood(other.gameObject); // 重新生成食物
        }
        // 2. 碰到边界或自己：游戏结束
        else if (other.CompareTag("Wall") || other.CompareTag("SnakeBody"))
        {
            Debug.Log("游戏结束！");
            Time.timeScale = 0; // 暂停游戏
        }
    }

    // 蛇变长：新增一节蛇身
    void GrowSnake()
    {
        Transform newBody = Instantiate(snakeBodyPrefab).transform;
        newBody.position = snakeHead.position; // 新蛇身生成在蛇头位置（后续会跟随移动）
        snakeBody.Add(newBody);
        newBody.tag = "SnakeBody"; // 给蛇身加标签，用于碰撞检测（撞自己）
    }

    // 随机生成食物（避免生成在蛇身上）
    void SpawnFood(GameObject food)
    {
        // 游戏边界：假设x轴-8到8，y轴-4到4（可根据视图调整）
        float x = Random.Range(-8, 9);
        float y = Random.Range(-4, 5);
        // 检查食物位置是否和蛇头/蛇身重叠，重叠则重新生成
        while (IsPosOverlap(new Vector2(x, y)))
        {
            x = Random.Range(-8, 9);
            y = Random.Range(-4, 5);
        }
        food.transform.position = new Vector2(x, y);
    }

    // 辅助函数：判断位置是否和蛇重叠
    bool IsPosOverlap(Vector2 pos)
    {
        // 检查是否和蛇头重叠
        if (Vector2.Distance(snakeHead.position, pos) < 0.5f)
            return true;
        // 检查是否和蛇身重叠
        foreach (var body in snakeBody)
        {
            if (Vector2.Distance(body.position, pos) < 0.5f)
                return true;
        }
        return false;
    }
}