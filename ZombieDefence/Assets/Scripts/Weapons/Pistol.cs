using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon, IWeapon
{
    void Start()
    {
        fireRate = GameData.PISTOL_FIRE_RATE;
        reloadSpeed = GameData.PISTOL_RELOAD_SPEED;
    }
    public void Attack()
    {
        for (int idx = 0; idx < GameManager.instance.PistolBullets.Length; idx++)
        {
            if (!GameManager.instance.PistolBullets[idx].activeSelf)
            {
                //위치 셋 회전 셋
                GameManager.instance.PistolBullets[idx].transform.position = startPos.position;
                GameManager.instance.PistolBullets[idx].transform.localRotation = startPos.rotation;
                GameManager.instance.PistolBullets[idx].SetActive(true);
                GameObject.Find("PistolFireSound").GetComponent<AudioSource>().Play();
                return;
            }
        }
    }
}
