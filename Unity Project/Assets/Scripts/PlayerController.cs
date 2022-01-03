﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float shootPower;
    [SerializeField] float shootRate;
    float MoveAmount { get => Time.deltaTime * moveSpeed; }
    float RotationAmount { get => Time.deltaTime * rotationSpeed; }
    public GameObject bulletPrefab, missilePrefab;
    Transform weapon, body;
    Camera mainCam;
    IEnumerator shootCoroutine = null;
    [SerializeField] float autoLockDuration;

    void Start()
    {
        mainCam = Camera.main;
        weapon = transform.Find("Body").Find("Weapon");
    }

    void Update()
    {
        AimAtCursor();
        HandleMovement();
        HandleShoot();
    }

    void HandleMovement()
    {
        var verticalMovement = Input.GetAxis("Vertical") * MoveAmount;
        var horizontalMovement = Input.GetAxis("Horizontal") * RotationAmount;
        // var rotationMovement = Input.GetAxis("Mouse X") * RotationAmount;
        transform.Rotate(new Vector3(-verticalMovement, horizontalMovement));
    }

    void HandleShoot()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(shootCoroutine != null)
                StopCoroutine(shootCoroutine);
            shootCoroutine = ShootCoroutine();
            StartCoroutine(shootCoroutine);
        }
        if(Input.GetMouseButtonUp(0))
        {
            StopCoroutine(shootCoroutine);
        }
        if(Input.GetMouseButtonDown(1))
        {
            ShootMissile();
        }
    }

    float lockDuration = 0f;
    Vector3 aim;
    Transform target = null;
    void AimAtCursor()
    {
        Vector3 mousePos = Input.mousePosition;
        target = GetPointedObjectTransform(mousePos);
        if(target != null)
        {
            lockDuration = autoLockDuration;
            aim = target.position;
        }
        else
            lockDuration -= Time.deltaTime;

        if (lockDuration <= 0f) 
            aim = mainCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, mainCam.farClipPlane));

        weapon.LookAt(aim);
    }

    void ShootWeapon()
    {
        var bulletPos = weapon.position + 2*weapon.forward;
        var bullet = GameObject.Instantiate(bulletPrefab, bulletPos, Quaternion.identity);
        var bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(weapon.forward * shootPower, ForceMode.Impulse);
    }

    void ShootMissile()
    {
        if(target == null) return;
        var pos = weapon.position + 2*weapon.forward;
        var obj = GameObject.Instantiate(missilePrefab, pos, Quaternion.identity);
        var missile = obj.GetComponent<Missile>();
        missile.LockOn(target);
    }

    IEnumerator ShootCoroutine()
    {
        while(true)
        {
            ShootWeapon();
            yield return new WaitForSeconds(1f / shootRate);
        }
    }

    Transform GetPointedObjectTransform(Vector3 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 200))
            if (hit.collider != null)
                return hit.collider.transform;
        return null;
    }
}
