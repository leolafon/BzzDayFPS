using System.Collections.Generic;
using UnityEngine;
using Nakama.TinyJson;
public class Target : MonoBehaviour
{
    public int health = 50;

    public void TakeDamage(int damage)
    {
        NakamaController.Instance.ShootOpponent(damage);
        health -= damage;
        if (health <= 0f)
        {
            Die();
        }
    }

    public void TakeDamageOther(string data)
    {
        var obj = data.FromJson<Dictionary<string, int>>();
        health -= obj["damage"];
        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Destroy(gameObject);
    }
}
