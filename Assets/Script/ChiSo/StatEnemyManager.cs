using System.Collections;
using UnityEngine;

[System.Serializable]
public class StatEnemyValues
{
    public string statType; // maxHealth, heal, attack, defense, speed, exp // maxHealth, heal, attack, defense, speed
    public int statValue;
}

public class StatEnemyManager : MonoBehaviour
{
    public StatEnemyValues[] reductions;
    public StatEnemyValues[] boosts;
    public bool nonDestructive = true;
    public float interval = 2f;

    private Coroutine statCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyStats enemy = other.GetComponent<EnemyStats>();


        if (enemy != null) //quai vat chi bi tac dong boi nhung vat the co dinh khong bien mat (gai,...)
        {
            ApplyStatChanges(enemy);
            if (nonDestructive && statCoroutine == null)
            {
                statCoroutine = StartCoroutine(RepeatedStatChange(enemy));
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

    private void ApplyStatChanges(EnemyStats enemy)
    {
        foreach (var reduction in reductions)
        {
            enemy.ReduceStat(reduction.statType, reduction.statValue);
        }
        foreach (var boost in boosts)
        {
        }
    }

    private IEnumerator RepeatedStatChange(EnemyStats enemy)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            ApplyStatChanges(enemy);
        }
    }
}
