﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class DwarfController : MonoBehaviour
{
    //Parametrization
    [SerializeField]
    float speed = 10.0f;
    [SerializeField]
    float gravity = 10.0f;
    [SerializeField]
    float maxVelocityChange = 10.0f;
    [SerializeField]
    public bool smooth = false;
    [SerializeField]
    float lookSpeed = 2.0f;

    //States... ifs... ~~
    private bool grounded = false;
    private bool attacking = false;

    Transform graphic;

    void Awake()
    {
        graphic = transform.root.FindChild("Graphic");
    }

    void FixedUpdate()
    {
        if (grounded)
        {
            // Calculate how fast we should be moving
            Vector3 targetOrientation = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;

            if (targetOrientation == Vector3.zero) return;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = rigidbody.velocity;
            Vector3 velocityChange = (targetOrientation - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
            velocityChange.y = 0;
            velocityChange.Normalize();

            if(!attacking)
                LookAt(targetOrientation);
                //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetOrientation), Time.deltaTime);

            rigidbody.AddForce(targetOrientation * speed, ForceMode.VelocityChange);
        }

        // We apply gravity manually for more tuning control
        rigidbody.AddForce(new Vector3(0, -gravity * rigidbody.mass, 0));

        grounded = false;
    }

    void LookAt(Vector3 direction, bool forze = false) 
    {
        Quaternion rotation = Quaternion.LookRotation(direction);

        if(smooth && !forze)
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.fixedDeltaTime * lookSpeed); //Look at the rotation smoothly
		else
            transform.rotation = rotation; //Just look at
 
    }

    void OnCollisionStay()
    {
        grounded = true;
    }

    public void OnMouseClick(object o)
    {
        Vector3 objetive = (Vector3)o;
        if (objetive != null)
            OnAttackStarts(objetive);
    }

    void OnAttackStarts(Vector3 objetive)
    {
        attacking = true;
        Vector3 objetiveDirection = objetive - transform.position;
        objetiveDirection.y = 0;

        LookAt(objetiveDirection, true);

        //<HACK>
        StartCoroutine(Attacking());
        //<//HACK>
    }

    IEnumerator Attacking() { yield return new WaitForSeconds(Random.Range(2, 4)); OnAttackEnds(); }

    void OnAttackEnds()
    {
        attacking = false;
    }
}