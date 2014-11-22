﻿using UnityEngine;
using System.Collections;

public class AttackController : MonoBehaviour 
{
    public void OnObjective(object o)
    {
        Vector3 objetive = (Vector3)o;

        if(objetive != null)
            Debug.DrawLine(transform.position, objetive);
    }
}