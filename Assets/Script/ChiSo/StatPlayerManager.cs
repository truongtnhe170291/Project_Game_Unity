using System.Collections;
using UnityEngine;

[System.Serializable]
public class StatValues
{
    public string statType; // maxHealth, heal, attack, defense, speed, exp // maxHealth, heal, attack, defense, speed
    public int statValue;
}

public class StatPlayerManager : MonoBehaviour
{
    public StatValues[] reductions;
    public StatValues[] boosts;
    public bool nonDestructive = true;
    public float interval = 2f;

    private Coroutine statCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerStats player = other.GetComponent<PlayerStats>();

        //EnemyStats enemyStats = GetComponentInParent<EnemyStats>();

        //if (enemyStats != null)
        //{
        //    int attack = enemyStats.attack;
        //    player.ReduceStat("health", attack);
        //    Debug.Log("Attack từ Parent: " + enemyStats.attack);
        //    return;
        //}

        if (player != null)
        {
            ApplyStatChanges(player);
            if (nonDestructive && statCoroutine == null)
            {
                statCoroutine = StartCoroutine(RepeatedStatChange(player));
            }

            if (!nonDestructive)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (statCoroutine != null)
        {
            StopCoroutine(statCoroutine);
            statCoroutine = null;
        }
    }

    private void ApplyStatChanges(PlayerStats player)
    {
        foreach (var reduction in reductions)
        {
            player.ReduceStat(reduction.statType, reduction.statValue);
        }
        foreach (var boost in boosts)
        {
            player.IncreaseStat(boost.statType, boost.statValue);
        }
    }

    private IEnumerator RepeatedStatChange(PlayerStats player)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            ApplyStatChanges(player);
        }
    }
}
