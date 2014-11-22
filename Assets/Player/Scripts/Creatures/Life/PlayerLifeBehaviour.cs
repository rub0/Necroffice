﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerLifeBehaviour : PlayerLifeManager
{
    GameObject RespawnPoint;
    public Life Life;

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
        GameObject[] Respawn = GameObject.FindGameObjectsWithTag("Respawn");

        if(Respawn.Length != 1) Debug.Log("Must be just and only 1 Respawn point in scene, bitch");

        RespawnPoint = Respawn.FirstOrDefault();

        if (RespawnPoint != null) OnRespawn();

    }

    public override void OnDead() 
    {
        //Respawn Player
        OnRespawn();

        //TODO: Create a enemy
    }

    public override void OnRespawn()
    {
        if (RespawnPoint != null)
            transform.root.position = RespawnPoint.transform.position;

        Life.life.Regenerate();
        transform.root.GetComponent<Stats>().RamdomStats();

        //TODO: Quitar la Weapon y poner periodico
    }
}