using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGun : ShipboardWeapon
{
    bool executing;
    public Transform gunPoint;
    public GameObject bulletPrefab;
    List<GameObject> bulletPool = new List<GameObject>();
    public float fireRate = 10;
    public float scatter = 5;
    public float damage = 1;
    public float flyingSpeed = 60;
    public float lifeTime = 5;

    public float _maxMagazine = 50;
    public float _magazine = 50;
    

    float fireBuffer = 1f;

    public override float GetMagazine()
    {
        return _magazine;
    }
    public override float GetMaxMagazine()
    {
        return _maxMagazine;
    }

    private void Start()
    {
        if(gunPoint == null)
        {
            gunPoint = this.transform;
        }
    }

    protected override void Update()
    {
        base.Update();
        if(executing && fireBuffer < 1) {
            fireBuffer += fireRate * Time.deltaTime;
        }
    }

    public void FireOnce()
    {
        GameObject newBullet = null;
        foreach(var cur in bulletPool)
        {
            if(cur.activeSelf == false)
            {
                newBullet = cur;
            }
        }
        if(newBullet == null)
        {
            newBullet = GameObject.Instantiate(bulletPrefab);
            bulletPool.Add(newBullet);
        }
        newBullet.SetActive(true);
        Bullet newBulletComponent = newBullet.GetComponent<Bullet>();
        newBulletComponent.flyingSpeed = flyingSpeed;
        newBulletComponent.damage = damage;
        newBulletComponent.lifeTime = lifeTime;
        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = gunPoint.rotation;
        
        //Scatter
        newBullet.transform.Rotate(0, 0, Random.Range(-scatter, scatter));
        newBulletComponent.Initialize();
        
    }


    public override void OnFire()
    {
        
        if(executing && fireBuffer >= 1)
        {
            fireBuffer--;
            if (_magazine > 0)
            {
                _magazine--;
                FireOnce();
            }
            else
            {
                //Empty magazine
            }
        }
    }

    public override void OnFireCease()
    {
        
    }

    public override void OnFireStart()
    {
        
    }

    public override void OnPlanPhaseStart()
    {
        executing = false;
    }

    public override void OnExecutePhaseStart()
    {
        executing = true;
    }
}
