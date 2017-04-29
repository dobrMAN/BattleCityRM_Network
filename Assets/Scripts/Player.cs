using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;

public class Player : Unit {
    [SyncVar]
    public int Lives = 3;
    [SyncVar]
    public string Name = "";
    [SyncVar]
    public int ID = -1;
    private NetworkStartPosition[] spawnPoints;

    public override void Start () {
        base.Start(); // Initialisation from base class (Unit.cs)

        Lastdirection = new Vector3(0f, 0f, 1f);
        if (isLocalPlayer)
        {
            spawnPoints = FindObjectsOfType<NetworkStartPosition>();
        }
        IsEnemy = false;
    }

    public override void OnStartLocalPlayer()
    {
        transform.Find("Tank/Body").GetComponent<MeshRenderer>().material.color = Color.blue;
        transform.Find("Tank/Cannon").GetComponent<MeshRenderer>().material.color = Color.blue;
        transform.Find("Tank/Turret").GetComponent<MeshRenderer>().material.color = Color.blue;
        transform.Find("Tank/Barrel").GetComponent<MeshRenderer>().material.color = Color.blue;

        base.OnStartLocalPlayer();
    }
    // Update is called once per frame
    void Update () {

        EngineAudio();

        if (!isLocalPlayer)
        {
            return;
        }

        float horizontal, vertical;
        horizontal = CrossPlatformInputManager.GetAxisRaw("Horizontal");
        vertical = CrossPlatformInputManager.GetAxisRaw("Vertical");


        if ((vertical >0.2f) && (Mathf.Abs(vertical)>Mathf.Abs(horizontal)))
		{
			Direction.Set(0.0f, 0.0f, 1.0f);
			Lastdirection = Direction;
		}
		if ((vertical <-0.2f) && (Mathf.Abs(vertical) > Mathf.Abs(horizontal)))
		{
			Direction.Set(0.0f, 0.0f, -1.0f);
			Lastdirection = Direction;
		}
		if ((horizontal <-0.2f) && (Mathf.Abs(vertical) <= Mathf.Abs(horizontal)))
        {
			Direction.Set(-1.0f, 0.0f, 0.0f);
			Lastdirection = Direction;
		}
		if ((horizontal >0.2f) && (Mathf.Abs(vertical) <= Mathf.Abs(horizontal)))
        {
			Direction.Set(1.0f, 0.0f, 0.0f);
			Lastdirection = Direction;
		}

        if ((Mathf.Abs(vertical)<=0.2f) && (Mathf.Abs(horizontal) <= 0.2f))
        {
            Direction.Set(0.0f, 0.0f, 0.0f);
        }

        if (CrossPlatformInputManager.GetButton("Jump")/* && Time.time > NextFire*/)
        {
            Fire();
        }

	}

    private void FixedUpdate()
    {
        Move();
    }

    void Hit(Bullet _bullet)
    {
        _bullet.DestroyMy();

        if (_bullet.IsEnemy)
        {
            PlayerDie();
        }
        else
        {
            //TODO Freeze player becouse it damaged by another player

        }
    }

    void PlayerDie()
    {
        //TODO Die player
        UnitDestroy();
        if (--Lives == 0)
            PlayerEndGame();
        else
            Rpc_Respawn();
    }

    void PlayerEndGame()
    {
        //TODO If player no have lives.
        Lives = 3;
        Rpc_Respawn();
    }

    [ClientRpc]
    void Rpc_Respawn()
    {
        if (isLocalPlayer)
        {
            // Set the player’s position to the chosen spawn point
            transform.position = spawnPoints[ID - 1].transform.position;
            transform.rotation = Quaternion.LookRotation(new Vector3(0.0f, 0.0f, 1.0f));
            Lastdirection = new Vector3(0.0f, 0.0f, 1.0f);
        }
    }
}
