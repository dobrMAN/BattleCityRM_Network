using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Unit : NetworkBehaviour
{
    [SerializeField]
    private AudioSource MMovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
    [SerializeField]
    private AudioClip MEngineIdling;            // Audio to play when the tank isn't moving.
    [SerializeField]
    private AudioClip MEngineDriving;           // Audio to play when the tank is moving.
    [SerializeField]
    private AudioSource MFireAudio;

    [SerializeField]
    private float MPitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.
    [SerializeField]
    private float MOriginalPitch;              // The pitch of the audio source at the start of the scene.

    public Vector3 Direction;
    public Vector3 Lastdirection;
    public float NextFire;

    public float Speed;

    public GameObject Shot;
    public Transform ShotSpawn;
    private Vector3 _shotRot;
    private Vector3 _shotPos;
    //[SyncVar]
    public float FireRate;

    [Tooltip("Префаб взрыва юнита.")]
    public GameObject ExplodePrefab;

    private GameObject _bullets;

    [SerializeField]
    private float _rotLerpRate = 15;
    [SerializeField]
    private bool UseRotationAnim;
    public MyGameManager MGM;

    private bool _isEnemy;
    public bool IsEnemy
    {
        get { return _isEnemy; }
        set { _isEnemy = value; }
    }


    public virtual void Start()
    {
        // Store the original pitch of the audio source.
        MOriginalPitch = MMovementAudio.pitch;
        _bullets = GameObject.Find("Bullets");
        MGM = GameObject.Find("MyGameManager").GetComponent<MyGameManager>();// FindObjectOfType<MyGameManager>();
    }

    [ClientRpc]
    public void Rpc_ResetNextFire(float _nextFire)
    {
        //NextFire = Time.time + 0.2f;
        NextFire = _nextFire;
    }

    [Command]
    private void CmdFire(Vector3 _lastdirection, float _fireRate)
    {
        DoFire(_lastdirection, _fireRate);
    }

    private void DoFire(Vector3 _lastdirection, float _fireRate)
    {
        if (!isServer) return;
        if (Time.time <= NextFire) return;
        Lastdirection = _lastdirection;
        NextFire = Time.time + 60.0F / FireRate;
        _shotPos = ShotSpawn.position;
        _shotRot = Lastdirection;
        GameObject obj = Instantiate(Shot, _shotPos, Quaternion.LookRotation(_shotRot));
        obj.transform.SetParent(_bullets.transform);
        Bullet newBullet;
        newBullet = obj.GetComponent<Bullet>();
        newBullet.Parent = gameObject;
        newBullet.Direction = Lastdirection;
        newBullet.IsEnemy = _isEnemy;
        MFireAudio.Play();
        NetworkServer.Spawn(obj);
    }

    public void Fire()
    {
        //if (isClient) CmdFire(Lastdirection,FireRate);

        if (isLocalPlayer) CmdFire(Lastdirection, FireRate);
        if (isServer) DoFire(Lastdirection, FireRate);
        //nextFire = Time.time + 60.0F / fireRate;
    }
    public void EngineAudio()
    {
        // If unit don't move...
        if (Direction == Vector3.zero)
        {
            // ... and if the audio source is currently playing the driving clip...
            if (MMovementAudio.clip == MEngineDriving)
            {
                // ... change the clip to idling and play it.
                MMovementAudio.clip = MEngineIdling;
                MMovementAudio.pitch = Random.Range(MOriginalPitch - MPitchRange, MOriginalPitch + MPitchRange);
                MMovementAudio.Play();
            }
        }
        else
        {
            // Otherwise if the tank is moving and if the idling clip is currently playing...
            if (MMovementAudio.clip == MEngineIdling)
            {
                // ... change the clip to driving and play.
                MMovementAudio.clip = MEngineDriving;
                MMovementAudio.pitch = Random.Range(MOriginalPitch - MPitchRange, MOriginalPitch + MPitchRange);
                MMovementAudio.Play();
            }
        }
    }

    public void Move()
    {
        //TODO Проверить, нужно ли поворачивать.
        if (!UseRotationAnim) transform.rotation = Quaternion.LookRotation(Lastdirection);
        else transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Lastdirection), Time.deltaTime * _rotLerpRate);

        if (Direction != Vector3.zero)
        {
            Lastdirection = Direction;
            //transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.deltaTime);

            //if (!UseRotationAnim) transform.rotation = Quaternion.LookRotation(Lastdirection);
            //else transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Lastdirection), Time.deltaTime * _rotLerpRate);

            //RaycastHit[] hit = new RaycastHit[10];
            RaycastHit hit;
            //Physics.BoxCast(Vector3.MoveTowards(transform.position, transform.position + Direction, Speed * Time.deltaTime), new Vector3(1.8f, 1.8f, 1.8f), Direction, out hit, Quaternion.identity, 0.4f);
            float _speed;
            _speed = Speed;
            int layerMask = 3 << 8;
            //if (Physics.BoxCastNonAlloc(Vector3.MoveTowards(transform.position, transform.position + Direction, _speed * Time.deltaTime), new Vector3(0.29f, 0.29f, 0.29f), Direction, hit, Quaternion.identity, 0.1f, layerMask) > 1)
            if (!Physics.BoxCast(Vector3.MoveTowards(transform.position, transform.position + Direction, _speed * Time.deltaTime), new Vector3(0.35f, 0.29f, 0.29f), Direction, out hit, transform.rotation, 0.1f, layerMask))
            {

                // if ((hit.transform.CompareTag("Player")) || (hit.transform.CompareTag("Enemy")))
                // {
                // _speed = 0;
                transform.position = Vector3.MoveTowards(transform.position, transform.position + Direction, _speed * Time.deltaTime);
                // }
            }

            //rb.rotation = Quaternion.LookRotation(direction);
        }
    }

    //public virtual void OnTriggerEnter(Collider other)
    //{

    //}

    public void UnitDestroy()
    {
        if (isServer)
        {
            GameObject obj = Instantiate(ExplodePrefab) as GameObject;
            ParticleSystem _explodePs = obj.GetComponent<ParticleSystem>();
            _explodePs.transform.position = transform.position;
            _explodePs.gameObject.SetActive(true);
            _explodePs.Play();
            Destroy(obj, 2.0f);
            NetworkServer.Spawn(obj);
            if (_isEnemy)
                NetworkServer.Destroy(gameObject);
        }
    }
}
