using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon, IWeapon
{
    void Start()
    {
        fireRate = GameData.RIFLE_FIRE_RATE;
        reloadSpeed = GameData.RIFLE_RELOAD_SPEED;
    }
    public void Attack()
    {
        for (int idx = 0; idx < GameManager.instance.RifleBullets.Length; idx++)
        {
            if (!GameManager.instance.RifleBullets[idx].activeSelf)
            {
                //위치 셋 회전 셋
                GameManager.instance.RifleBullets[idx].transform.position = startPos.position;
                GameManager.instance.RifleBullets[idx].transform.localRotation = startPos.rotation;
                GameManager.instance.RifleBullets[idx].SetActive(true);
                GameObject.Find("PistolFireSound").GetComponent<AudioSource>().Play();
                return;
            }
        }
    }
}
