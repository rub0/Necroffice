﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Life))]
public class PlayerLifeBehaviour : LifeBehaviour
{
    GameObject RespawnPoint;
    Life Life;

    public void Awake()
    {
        Life = GetComponent<Life>();
        GameObject[] Respawn = GameObject.FindGameObjectsWithTag("Respawn");

        if(Respawn.Length != 1) Debug.Log("Must be just and only 1 Player Respawn point in scene, bitch");

        RespawnPoint = Respawn.FirstOrDefault();

    }

    void Start()
    {
        if (RespawnPoint != null) OnRespawn();
    }

    public override void OnDead() 
    {
        //Corrutina de la muerte
        StartCoroutine(GameManager.Instance.CorrutinaDeLaMuerte());

        //Respawn Player
        OnRespawn();

        //TODO: Create a creature
        //Instantiate(
    }

    public override void OnRespawn()
    {
        if (RespawnPoint != null)
            transform.root.position = RespawnPoint.transform.position;

        Life.life.Regenerate();
        transform.root.GetComponent<Stats>().RamdomStats();

        GetComponent<Creature>().EquipWeapon(GameManager.Instance.DefaultWeapon);

        //TODO: Quitar la Weapon y poner periodico
    }
}
