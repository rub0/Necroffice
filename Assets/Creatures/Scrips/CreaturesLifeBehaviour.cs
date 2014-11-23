﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Life))]
public class CreaturesLifeBehaviour : PlayerLifeManager
{
    Life Life;

    //<HACK>
    //bool wait = false;
    //public void Update()
    //{
    //    if (!wait) StartCoroutine(FalseLife());
    //}
    //IEnumerator FalseLife()
    //{
    //    wait = true;
    //    yield return new WaitForSeconds(Random.Range(2, 5));
    //    Life.OnDamage(20);
    //    wait = false;
    //}
    //</HACK>

    public void Awake()
    {
        Life = GetComponent<Life>();

        OnRespawn();

    }

    public override void OnDead() 
    {
    }

    public override void OnRespawn()
    {
        transform.root.GetComponent<Stats>().RamdomStats();
        Life.life.Regenerate();

        GameObject[] weapons = Resources.LoadAll<GameObject>("Weapons");
        GameObject choosen = weapons[Random.Range(0, weapons.Count())];
        
        Creature creature = GetComponent<Creature>();
        
        if(creature != null)
        {
            choosen.transform.root.BroadcastMessage("SetOwner", creature);
            choosen.transform.root.BroadcastMessage("SetMode", WeaponMode.AI);
        }
    }
}