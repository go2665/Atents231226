using UnityEngine;

public class Factory : Singleton<Factory>
{
    BulletHolePool bulletHolePool;
    AssaultRiflePool assaultRiflePool;
    ShotgunPool shotgunPool;
    HealPackPool healPackPool;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        bulletHolePool = GetComponentInChildren<BulletHolePool>();
        bulletHolePool?.Initialize();

        assaultRiflePool = GetComponentInChildren<AssaultRiflePool>();
        assaultRiflePool?.Initialize();

        shotgunPool = GetComponentInChildren<ShotgunPool>();
        shotgunPool?.Initialize();

        healPackPool = GetComponentInChildren<HealPackPool>();
        healPackPool?.Initialize();
    }

    public BulletHole GetBulletHole()
    {
        return bulletHolePool?.GetObject();
    }

    public BulletHole GetBulletHole(Vector3 position, Vector3 normal, Vector3 reflect)
    {
        BulletHole hole = bulletHolePool?.GetObject();
        hole.Initialize(position, normal, reflect);
        return hole;
    }

    public GunItem GetAssaultRifleItem(Vector3 position)
    {
        GunItem item = assaultRiflePool?.GetObject();
        item.transform.position = position;
        return item;
    }

    public GunItem GetShotgunItem(Vector3 position)
    {
        GunItem item = shotgunPool?.GetObject();
        item.transform.position = position;
        return item;
    }

    public HealItem GetHealPackItem(Vector3 position)
    {
        HealItem item = healPackPool?.GetObject();
        item.transform.position = position;
        return item;
    }
}
