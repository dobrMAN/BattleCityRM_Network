using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

    void HitBig(Bullet _bullet)
    {
        _bullet.DestroyMy();
    }
}
