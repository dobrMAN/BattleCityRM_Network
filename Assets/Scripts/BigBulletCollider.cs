using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BigBulletCollider : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        //    BrickBlock block = other.GetComponent<BrickBlock>();
        //    if (block)
        //    {
        //        Debug.Log("Big Bullet collider.");
        //        GetComponentInParent<Bullet>().Destroy();
        //        Destroy(other.gameObject);
        //        NetworkServer.Destroy(other.gameObject);
        //    }
        //Todo избавиться от этого большого колайдера, уничтожать соседние блоки.
        other.SendMessage("HitBig", GetComponentInParent<Bullet>(), SendMessageOptions.DontRequireReceiver);
    }
//	// Use this for initialization
//	void Start () {
//		
//	}
//	
//	// Update is called once per frame
//	void Update () {
//		
//	}
}
