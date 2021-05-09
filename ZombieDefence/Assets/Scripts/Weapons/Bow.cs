using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon, IWeapon
{
    void Start()
    {
        fireRate = GameData.BOW_FIRE_RATE;
        reloadSpeed = GameData.BOW_RELOAD_SPEED;
    }
    public void Attack()
    {
        for (int idx = 0; idx < GameManager.instance.BowArrows.Length; idx++)
        {
            if (!GameManager.instance.BowArrows[idx].activeSelf)
            {
                //위치 셋 회전 셋
                GameManager.instance.BowArrows[idx].transform.position = startPos.position;
                GameManager.instance.BowArrows[idx].transform.localRotation = startPos.rotation;
                GameManager.instance.BowArrows[idx].SetActive(true);
                GameObject.Find("GunTurretFireSound").GetComponent<AudioSource>().Play();
                return;
            }
        }
    }
}
