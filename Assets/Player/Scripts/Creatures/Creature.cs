﻿using UnityEngine;
using System.Collections;

public class Creature : MonoBehaviour
{
    public enum Alignments { PLAYER = 0, NEUTRAL, ENEMY };

    public Alignments Alignment = Alignments.ENEMY;

    public Transform WeaponHoldPoint;

    public Weapon _Weapon;

    public string Name = "Player";

    [HideInInspector]
    public Controller _Control;

    [HideInInspector]
    public Stats _Stats;

    [HideInInspector]
    public Life _Life;


    void Awake()
    {
        _Control = GetComponent<Controller>();

        _Stats = GetComponent<Stats>();

        _Life = GetComponent<Life>();

    }

    void Start()
    {
        if (IsPlayer())
        {
            GeneratePlayerDwarf();
        }

        EquipWeapon(GameManager.Instance.DefaultWeapon);
    }

    void EquipWeapon(Weapon newWeapon)
    {
        if (_Weapon != null && _Weapon != GameManager.Instance.DefaultWeapon)
        {
            DropWeapon();
        }

        //Poner nueva
        GameObject _obj = Instantiate(newWeapon.gameObject, WeaponHoldPoint.position, WeaponHoldPoint.rotation) as GameObject;
        _obj.transform.parent = WeaponHoldPoint.transform;
        _obj.transform.localPosition = Vector3.zero;
        _obj.transform.localScale = Vector3.one;      //Todas las armas tienen que tener escala (1,1,1)!

        _Weapon = _obj.GetComponent<Weapon>();

        _Weapon.owner = this;

        if (IsPlayer())
        {
            _Weapon.weaponMode = WeaponMode.CONTROLLED;
        }
        else
        {
            _Weapon.weaponMode = WeaponMode.AI;
        }
    }

    void DropWeapon()
    {
        //todo: soltar como objeto en vez de destruir

        Destroy(_Weapon.gameObject);

        _Weapon = null;
    }

    void OnInputMouseClick(Vector3 clickPoint)
    {
        if (_Weapon != null)
            _Weapon.attack();


        //    Vector3 _attackingDirection = transform.forward;
        //    _attackingDirection.y = 0;
        //    _attackingDirection *= 8;

        //    GameManager.Instance.CreateHitbox(GetComponent<Creature>(), 1, 1, _attackingDirection);
    }

    public bool IsPlayer()
    {
        return (tag == "Player");
    }

    public void OnDead()
    {
        Debug.Log("Se muere " + name);

        if (IsPlayer())
        {
            //Muere player 
            GameManager.Instance.DestroyWithParticle("BloodSplat", gameObject);

            //Corrutina de la muerte
            StartCoroutine(GameManager.Instance.CorrutinaDeLaMuerte());
        }
        else
        {
            //Muere enemigo
            GameManager.Instance.DestroyWithParticle("BloodSplat", gameObject);
        }
    }

    public string GenerateDwarvenName()
    {
        //Nombres
        string[] _names = new string[] { "Ulan", "Fikden", "Iolkhan", "Gegdo", "Glorirgoid ", "Groornuki ", "Snavromi ", "Brufirlum ", "Dumroir ", "Mogis" };

        //Apellidos
        string[] _surnames = new string[] { "Trollrock", "Flaskmaker", "Brickarmour", "Chainflayer", "Axerock", "Chaosbreaker", "Pebblehorn", "Boneforged", "Koboldbelly", "Honorcloak" };


        string _fName = _names[UnityEngine.Random.Range(0, _names.Length)];
        string _fSurname = _surnames[UnityEngine.Random.Range(0, _surnames.Length)];
        return _fName + " " + _fSurname;
    }

    public void GeneratePlayerDwarf()
    {
        Name = GenerateDwarvenName();
        Debug.Log("Generado nombre");

        int _maxLife = Random.Range(40, 45);

        _maxLife -= (_Stats.Agility.value + _Stats.Power.value);

        _Life.life = new Stats.Attribute(0, _maxLife);

    }
}
