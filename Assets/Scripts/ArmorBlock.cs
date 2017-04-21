using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ArmorBlock : Block
{
    void HitBig(Bullet _bullet)
    {
        _bullet.DestroyMy();
        if (_bullet.IsArmorPiercing)
            NetworkServer.Destroy(gameObject);
    }
}
