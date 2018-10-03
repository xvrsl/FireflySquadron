using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGun : ShipboardWeapon
{
    public Transform gunPoint;
    public GameObject bulletPrefab;
    List<GameObject> bulletPool = new List<GameObject>();
    public enum FireMode
    {
        single = 0,
        continuous = 1
    }
    public FireMode mode;

    public float fireRate = 10;
    public float scatter = 5;
    public List<Damage> damages = Damage.GetSingleDamageList(1,Damage.DamageType.physical);
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
        if(execute && fireBuffer < 1) {
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
        newBulletComponent.master = this.master;
        newBulletComponent.flyingSpeed = flyingSpeed;
        newBulletComponent.damages = new List<Damage>(damages);
        newBulletComponent.lifeTime = lifeTime;
        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = gunPoint.rotation;
        
        //Scatter
        newBullet.transform.Rotate(0, 0, Random.Range(-scatter, scatter));
        newBulletComponent.Initialize();
        
    }
    public void CheckStatusAndFireOnce(FireMode fireIfMode)
    {
        if (execute && mode == fireIfMode && fireBuffer >= 1)
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

    public override void OnFire()
    {
        CheckStatusAndFireOnce(FireMode.continuous);
    }

    public override void OnFireCease()
    {
        
    }

    public override void OnFireStart()
    {
        CheckStatusAndFireOnce(FireMode.single);
    }

    public override void OnPlanPhaseStart()
    {
        base.OnPlanPhaseStart();
        execute = false;
    }

    public override void OnExecutePhaseStart()
    {
        base.OnExecutePhaseStart();
        execute = true;
    }
}
