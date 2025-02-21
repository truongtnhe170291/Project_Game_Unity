using System.Collections;
using UnityEngine;

[System.Serializable]
public class StatBoostValues
{
    public string statType; // maxHealth, heal, attack, defense, speed, exp
    public int boostValue;
}

[System.Serializable]
public class StatReduceValues
{
    public string statType; // maxHealth, heal, attack, defense, speed
    public int reduceValue;
}

public class StatManager : MonoBehaviour
{
    public StatReduceValues[] reductions;
    public StatBoostValues[] boosts;
    public bool nonDestructive = true; // Nếu true, tiếp tục cộng/trừ khi đứng trên collider
    public float interval = 2f; // Mỗi bao lâu thì thay đổi chỉ số

    private Coroutine statCoroutine; // Để kiểm soát vòng lặp khi người chơi thoát ra

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerStats player = other.GetComponent<PlayerStats>();
        if (player != null)
        {
            // Thực hiện thay đổi chỉ số lần đầu tiên
            ApplyStatChanges(player);

            // Nếu shouldDestroyAfterUse bật, bắt đầu vòng lặp thay đổi chỉ số khi đứng trên collider
            if (nonDestructive && statCoroutine == null)
            {
                statCoroutine = StartCoroutine(RepeatedStatChange(player));
            }

            if(!nonDestructive)
            {
                Destroy(gameObject);
            }
        }
    }

    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     // Khi người chơi thoát ra, dừng coroutine để không cộng/trừ chỉ số nữa
    //     if (statCoroutine != null)
    //     {
    //         StopCoroutine(statCoroutine);
    //         statCoroutine = null;
    //     }
    // }

    private void ApplyStatChanges(PlayerStats player)
    {
        foreach (var reduction in reductions)
        {
            player.ReduceStat(reduction.statType, reduction.reduceValue);
        }

        foreach (var boost in boosts)
        {
            player.IncreaseStat(boost.statType, boost.boostValue);
        }
    }

    private IEnumerator RepeatedStatChange(PlayerStats player)
    {
        while (true) // Lặp vô hạn, nhưng sẽ dừng nếu người chơi thoát khỏi vùng collider
        {
            yield return new WaitForSeconds(interval);
            ApplyStatChanges(player);
        }
    }
}
