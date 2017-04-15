using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ArmorBlock : Block
{
    //[Server]
    //   void OnTriggerEnter(Collider other)
    //   {
    //       if (!isServer) return;
    //       BigBulletCollider bulletcollider = other.GetComponent<BigBulletCollider>();

    //	if (bulletcollider != null)
    //	{
    //		//Обрабатываем столкновение пули с блоком
    //		bulletcollider.GetComponentInParent<Bullet>().DestroyMy();

    //       }
    //}

    void HitBig(Bullet _bullet)
    {
        _bullet.DestroyMy();
        if (_bullet.IsArmorPiercing)
            NetworkServer.Destroy(gameObject);
    }
}
