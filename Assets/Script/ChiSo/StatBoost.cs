using UnityEngine;

[System.Serializable]
public class StatBoostValues
{
    public string statType; // maxHealth, heal, attack, defense, speed, exp
    public int boostValue;
}

public class StatBoost : MonoBehaviour
{
    public StatBoostValues[] boosts;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerStats player = other.GetComponent<PlayerStats>();
        if (player != null)
        {
            foreach (var boost in boosts)
            {
                player.IncreaseStat(boost.statType, boost.boostValue);
            }
            Destroy(gameObject);
        }
    }
}