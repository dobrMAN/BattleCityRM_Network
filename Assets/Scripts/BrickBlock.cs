using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BrickBlock : Block
{
    //[Server]
    //void OnTriggerEnter(Collider other)
    //{
    //    if (!isServer) return;
    //    BigBulletCollider bulletcollider = other.GetComponent<BigBulletCollider>();

    //    if (bulletcollider)
    //    {
    //        //Debug.Log(bulletcollider.GetType());
    //        //Обрабатываем столкновение пули с блоком
    //        bulletcollider.GetComponentInParent<Bullet>().DestroyMy();
    //        NetworkServer.Destroy(gameObject);
    //    }
    //}
    //void ExplosionDamage(bool _isArmorPiercing)
    //{
    //    NetworkServer.Destroy(gameObject);
    //}

    //void Hit(Bullet _bullet)
    //{
    //    //return;

    //    //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //    GameObject cube = new GameObject();
    //    cube.transform.localScale = new Vector3(0.45f, 0.5f, 0.1f);
    //    cube.transform.localRotation = _bullet.transform.localRotation;
    //    cube.transform.position = _bullet.transform.position;
    //    //cube.transform.position += cube.transform.forward * 0.1f;
  
    //    int layerMask = 1 << 9;
    //    Collider[] col = Physics.OverlapBox(cube.transform.position, /*new Vector3(0.21f, 0.5f, 0.05f)*/ cube.transform.localScale / 2, cube.transform.localRotation, layerMask);
    //    _bullet.DestroyMy();

    //    //Debug.Log("Соседних блоков - "+col.Length);
    //    foreach(Collider c in col)
    //    {
    //        if (gameObject != c.gameObject)
    //            c.enabled = false;
    //            c.gameObject.SendMessage("ExplosionDamage", _bullet.IsArmorPiercing, SendMessageOptions.DontRequireReceiver);
    //    }

    //    NetworkServer.Destroy(gameObject);
    //}
    //[ClientRpc]
    //void Rpc_DestroyMy()
    //{
    //    Destroy(gameObject);
    //}

    void HitBig(Bullet _bullet)
    {
        _bullet.DestroyMy();
        NetworkServer.Destroy(gameObject);
    }

 }