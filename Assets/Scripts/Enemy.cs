using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class Enemy : Unit {
    
   // private Rigidbody rb;
   private Vector3[] _directionsArray = new[] 
   {
       new Vector3(0f, 0f, 0f),     // stop
       new Vector3(0f, 0f, -1f),    // down
       new Vector3(0f, 0f, 1f),     // up
       new Vector3(-1f, 0f, 0f),    // left
       new Vector3(1f, 0f, 0f),     // right
       new Vector3(0f, 0f, -1f)     // down again
   };
    private float _distance;

    public bool IsPanzer = false;
    private int _panzerHits = 0;

    public override void Start () {
        // rb = GetComponent<Rigidbody>();
        base.Start(); // Initialisation from base class (Unit.cs)

        IsEnemy = true;
        Direction = Vector3.zero;
        Lastdirection = new Vector3(0f, 0f, -1f);
        InitialiseMove();
    }

    public override void OnStartLocalPlayer()
    {

        base.OnStartLocalPlayer();
    }
    // Update is called once per frame
    void Update () {

       // if (!isLocalPlayer)
       // {
       //     return;
       // }

        EngineAudio ();
	}

    private void FixedUpdate()
    {
        //TODO Use coroutine to fire
        if (Random.Range(0, 50) > 48) Fire();
        //rb.velocity = direction * speed;
        if (isServer)
        Move();

        _distance -= Speed * Time.deltaTime;
        if (_distance <= 0) InitialiseMove();
    }

    private void InitialiseMove()
    {
        float _dist = Random.Range(0f, 5f);
        int _dir = Random.Range(0, 4+1); // Max not included in range on INT implementation!

        _distance = _dist;

        if ((Direction != Vector3.zero) && (_dir > 0)) Lastdirection = Direction;
        Direction = _directionsArray[_dir];

    }

    void Hit(Bullet _bullet)
    {
        if (!_bullet.IsEnemy)
        {
            _bullet.DestroyMy();

            if (!IsPanzer || (++_panzerHits > 4))
            {
                UnitDestroy();
                MGM.EnemyCount--;
            }
            else
            {
                //TODO Танк бронированый, обработать попадание пули в броню
            }
        }
        else
        {
            //Ignore collisions for enemys with enemy's bullets.
            Physics.IgnoreCollision(_bullet.GetComponent<Collider>(), GetComponent<Collider>(), true);
        }
    }
}
