using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BaseBlock : Block
{
    [Tooltip("Префаб взрыва базы.")]
    public GameObject ExplodePrefab;

    void HitBig(Bullet _bullet)
    {
        _bullet.DestroyMy();
        GameObject obj = Instantiate(ExplodePrefab) as GameObject;
        ParticleSystem _explodePs = obj.GetComponent<ParticleSystem>();
        obj.GetComponent<AudioSource>().Play();
        _explodePs.transform.position = transform.position;
        //_explodePs.gameObject.SetActive(true);
        //_explodePs.Play();
        Destroy(obj, 2.0f);
        NetworkServer.Spawn(obj);
        NetworkServer.Destroy(gameObject);
        //BroadcastMessage("BaseDestructed");

        FindObjectOfType<MyGameManager>().BaseDestructed();
     }

}