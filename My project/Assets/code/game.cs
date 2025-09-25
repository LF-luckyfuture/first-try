using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class SnakeControl : MonoBehaviour
{
    public Transform snakeHead;
    public GameObject snakeBodyPrefab;
    private Vector2 moveDirection = Vector2.right;
    private float moveInterval = 0.2f;
    private float timer; // 计时用
    private List<Transform> snakeBody = new List<Transform>();
    public ParticleSystem Particle;
    void Update()
    {
        Particle = GetComponent<ParticleSystem>();
        // 计时：达到间隔时间就移动一次
        timer += Time.deltaTime;
        if (timer >= moveInterval)
        {
            MoveSnake();
            timer = 0;
        }
        if (Input.GetKeyDown(KeyCode.W)&&moveDirection!=Vector2.down)
        {
            moveDirection = Vector2.up;Particle.Play();
        }
        else if (Input.GetKeyDown(KeyCode.S) && moveDirection != Vector2.up)
        {
            moveDirection = Vector2.down; Particle.Play();
        }
        else if (Input.GetKeyDown(KeyCode.A) && moveDirection != Vector2.right)
        {
            moveDirection = Vector2.left;Particle.Play();
        }
        else if (Input.GetKeyDown(KeyCode.D) && moveDirection != Vector2.left)
        {

            moveDirection = Vector2.right;Particle.Play();
        }
    }
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
    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. 碰到食物：蛇变长，食物重新生成
        if (other.CompareTag("Food"))
        {
            GrowSnake(); // 蛇变长
            SpawnFood(other.gameObject); // 重新生成食物
        }
        else if (other.CompareTag("Wall") || other.CompareTag("SnakeBody"))
        {
            SceneManager.LoadScene(2);
            enabled = false;
        }
    }

    // 蛇变长：新增一节蛇身
    void GrowSnake()
    {
        Transform newBody = Instantiate(snakeBodyPrefab).transform;
        snakeBody.Add(newBody);
        newBody.tag = "SnakeBody";
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