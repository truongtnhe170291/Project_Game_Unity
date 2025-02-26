using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;

public class NewEnemtAI : MonoBehaviour
{
    public bool roaming = true;
    public float moveSpeed;
    public float nextWPDistance;

    // Shoot
    public bool isShootabte = false;
    public GameObject bullet;
    public float buttetSpeed;
    public float timeBtwFire;
    private float fireCooldown;

    public Seeker seeker;
    public SpriteRenderer characterSR;

    Transform target;
    Path path;
    Coroutine moveCorountine;
    private void Start()
    {
        InvokeRepeating("CalculatePath", 0f, 0.3f);
    }

    private void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (fireCooldown <= 0)
        {
            fireCooldown = timeBtwFire;
            if (bullet != null && isShootabte)
            {
                EnemyFireBullet();
            }
        }
    }

    private void EnemyFireBullet()
    {
        var bulletTmp = Instantiate(bullet, transform.position, Quaternion.identity);
        Rigidbody2D rb = bulletTmp.GetComponent<Rigidbody2D>();
        Vector3 playerPos = FindAnyObjectByType<Player>().transform.position;
        Vector3 direction = playerPos - transform.position;
        rb.AddForce(direction.normalized * buttetSpeed, ForceMode2D.Impulse);
    }

    void CalculatePath()
    {
        Vector2 target = FindTarget();
        if (seeker.IsDone())
        {
            seeker.StartPath(transform.position, target, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (p.error) return;
        path = p;
        MoveToTarget();
    }

    void MoveToTarget()
    {
        if (moveCorountine != null) StopCoroutine(moveCorountine);
        moveCorountine = StartCoroutine(MoveToTargetCorountine());
    }

    IEnumerator MoveToTargetCorountine()
    {
        int currentWP = 0;
        while (currentWP < path.vectorPath.Count)
        {
            Vector2 direction = ((Vector2)path.vectorPath[currentWP] - (Vector2)transform.position).normalized;
            Vector3 force = direction * moveSpeed * Time.deltaTime;
            transform.position += force;

            float distance = Vector2.Distance(transform.position, path.vectorPath[currentWP]);
            if (distance < nextWPDistance)
            {
                currentWP++;
            }

            if (force.x != 0)
            {
                if (force.x < 0) characterSR.transform.localScale = new Vector3(-1, 1, 0);
                else characterSR.transform.localScale = new Vector3(1, 1, 0);
            }
            yield return null;
        }

    }

    Vector2 FindTarget()
    {
        Vector3 playerPos = FindAnyObjectByType<Player>().transform.position;
        if (roaming == true)
        {
            Vector2 randomDirection;
            do
            {
                randomDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            } while (randomDirection == Vector2.zero); // Đảm bảo không phải (0,0)

            return (Vector2)playerPos + (Random.Range(5f, 10f) * randomDirection.normalized);
        }
        else
        {
            return playerPos;
        }
    }
}