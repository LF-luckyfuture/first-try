using UnityEngine;
using System.Collections.Generic;

public class SnakeControl : MonoBehaviour
{
    public Transform snakeHead; // ��ͷ
    public GameObject snakeBodyPrefab; // ����Ԥ����
    private Vector2 moveDirection = Vector2.right; // ��ʼ��������
    private float moveInterval = 0.2f; // �ƶ������ԽСԽ�죩
    private float timer; // ��ʱ��
    private List<Transform> snakeBody = new List<Transform>(); // �洢����
    public float maxX = 9.5f;
    public float minX = -9.5f;
    public float maxY = 4.5f;
    public float minY = -4.5f;
    void Update()
    {
        // ��ʱ���ﵽ���ʱ����ƶ�һ��
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

    // �ߵ��ƶ��߼�
    void MoveSnake()
    {
        // ��¼��ͷ�ƶ�ǰ��λ�ã�����������ã�
        Vector2 oldHeadPos = snakeHead.position;
        // �ƶ���ͷ
        snakeHead.Translate(moveDirection);

        // ��������������һ�������Ƶ���ͷԭ����λ��
        if (snakeBody.Count > 0)
        {
            Transform lastBody = snakeBody[snakeBody.Count - 1];
            lastBody.position = oldHeadPos;
            // �����һ�������Ƶ��б���ǰ�棨ʵ�֡����桱Ч����
            snakeBody.RemoveAt(snakeBody.Count - 1);
            snakeBody.Insert(0, lastBody);
        }
    }
    // �����ͷ�������������ײ����ʳ��/ײǽ/ײ�Լ���
    void OnTriggerEnter2D(Collider2D other)
    {
        // 1. ����ʳ��߱䳤��ʳ����������
        if (other.CompareTag("Food"))
        {
            GrowSnake(); // �߱䳤
            SpawnFood(other.gameObject); // ��������ʳ��
        }
        // 2. �����߽���Լ�����Ϸ����
        else if (other.CompareTag("Wall") || other.CompareTag("SnakeBody"))
        {
            Debug.Log("��Ϸ������");
            Time.timeScale = 0; // ��ͣ��Ϸ
        }
    }

    // �߱䳤������һ������
    void GrowSnake()
    {
        Transform newBody = Instantiate(snakeBodyPrefab).transform;
        newBody.position = snakeHead.position; // ��������������ͷλ�ã�����������ƶ���
        snakeBody.Add(newBody);
        newBody.tag = "SnakeBody"; // ������ӱ�ǩ��������ײ��⣨ײ�Լ���
    }

    // �������ʳ����������������ϣ�
    void SpawnFood(GameObject food)
    {
        // ��Ϸ�߽磺����x��-8��8��y��-4��4���ɸ�����ͼ������
        float x = Random.Range(-8, 9);
        float y = Random.Range(-4, 5);
        // ���ʳ��λ���Ƿ����ͷ/�����ص����ص�����������
        while (IsPosOverlap(new Vector2(x, y)))
        {
            x = Random.Range(-8, 9);
            y = Random.Range(-4, 5);
        }
        food.transform.position = new Vector2(x, y);
    }

    // �����������ж�λ���Ƿ�����ص�
    bool IsPosOverlap(Vector2 pos)
    {
        // ����Ƿ����ͷ�ص�
        if (Vector2.Distance(snakeHead.position, pos) < 0.5f)
            return true;
        // ����Ƿ�������ص�
        foreach (var body in snakeBody)
        {
            if (Vector2.Distance(body.position, pos) < 0.5f)
                return true;
        }
        return false;
    }
}