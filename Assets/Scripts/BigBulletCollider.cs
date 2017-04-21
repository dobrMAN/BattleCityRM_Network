using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BigBulletCollider : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        //Todo избавиться от этого большого колайдера, уничтожать соседние блоки.
        other.SendMessage("HitBig", GetComponentInParent<Bullet>(), SendMessageOptions.DontRequireReceiver);
    }
}
