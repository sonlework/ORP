using System.Collections.Generic;
using UnityEngine;

public class BulletManager : BaseManager<BulletManager>
{
    [Header("Normal Bullet Pool")]
    [SerializeField] private List<BulletController> bulletPool;
    [SerializeField] private BulletController objectToPool;
    [SerializeField] private int amountToPool = 10;

    [Header("Charged Bullet Pool")]
    [SerializeField] private List<BulletController> chargedBulletPool;
    [SerializeField] private BulletController chargedBulletPrefab;
    [SerializeField] private int chargedAmountToPool = 5;

    private void Start()
    {
        InitNormalPool();
        InitChargedPool();
    }

    private void InitNormalPool()
    {
        bulletPool = new();
        for (int i = 0; i < amountToPool; i++)
        {
            BulletController bullet = Instantiate(objectToPool);
            bulletPool.Add(bullet);
            bullet.DeActive();
        }
    }

    private void InitChargedPool()
    {
        chargedBulletPool = new();
        for (int i = 0; i < chargedAmountToPool; i++)
        {
            BulletController bullet = Instantiate(chargedBulletPrefab);
            chargedBulletPool.Add(bullet);
            bullet.DeActive();
        }
    }

    public BulletController GetPooledBullet()
    {
        foreach (var bullet in bulletPool)
        {
            if (!bullet.IsActive)
            {
                bullet.gameObject.SetActive(true);
                return bullet;
            }
        }
        BulletController newBullet = Instantiate(objectToPool);
        bulletPool.Add(newBullet);
        return newBullet;
    }

    public BulletController GetChargedBullet()
    {
        foreach (var bullet in chargedBulletPool)
        {
            if (!bullet.IsActive)
            {
                bullet.gameObject.SetActive(true);
                return bullet;
            }
        }
        BulletController newBullet = Instantiate(chargedBulletPrefab);
        chargedBulletPool.Add(newBullet);
        return newBullet;
    }
}