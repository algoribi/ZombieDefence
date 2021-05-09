using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int key;
    public int HP;
    int maxHP;
    void Start()
    {
        maxHP = HP;
    }
    void Update()
    {
        if (HP != maxHP && StateManager.isDay)
        {
            HP = maxHP;
        }
    }
    public void Hit(int size)
    {
        HP -= size;
        Die();
    }
    void Die()
    {
        if(HP <= 0)
        {
            Destroy(gameObject);
        }
    }
}
