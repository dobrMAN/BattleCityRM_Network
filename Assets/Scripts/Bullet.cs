using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
    [SyncVar]
    public float Speed = 5.0F;
    [SyncVar]
    private Vector3 _direction;

    public Vector3 Direction {
		set { _direction = value; }
	}

    [SyncVar] //TODO оно здесь для чего?!
    public GameObject ExplodePrefab;
    [SyncVar]
    public bool IsEnemy;
    [SyncVar]
    public bool IsArmorPiercing = false;
    [SyncVar]
    private GameObject _parent;
	public GameObject Parent { set { _parent = value; }  get { return _parent; } }

	void Start ()
	{
        //Debug.Log("dsdsdsd   "+ GameObject.Find("Boundary"));
        //Physics.IgnoreCollision(GameObject.Find("Boundary").GetComponent<Collider>(), GetComponent<Collider>());
        //gameObject.GetComponent<AudioSource>().PlayOneShot(FireSound);
        //AudioSource.PlayClipAtPoint(FireSound, gameObject.transform.position);
        //_explodePsGO = Instantiate(ExplodePrefab);
        //_explodePs = _explodePsGO.GetComponent<ParticleSystem>();

        //GameObject tmp;
        //tmp = Instantiate(FireSoundPrefab, gameObject.transform);
        //tmp.GetComponent<AudioSource>().Play();
        //Destroy(tmp, tmp.GetComponent<AudioSource>().clip.length + 1);
    }
    public void ShowExplosion()
    {
        GameObject obj = Instantiate(ExplodePrefab) as GameObject;
        ParticleSystem _explodePs = obj.GetComponent<ParticleSystem>();
        _explodePs.transform.position = transform.position;
        _explodePs.gameObject.SetActive(true);
        _explodePs.Play();
        Destroy(obj, 2.0f);
        NetworkServer.Spawn(obj);
    }

    [Command]
    public void CmdDestroyMy(){
        DestroyMy();
    }

    //[Server]
    public void DestroyMy()
    {
        if (!isServer) return;
        
        if (Parent != null)
        {
            Parent.GetComponent<Unit>().NextFire = Time.time + 0.2f;
            if (!IsEnemy)
                Parent.GetComponent<Unit>().Rpc_ResetNextFire(Time.time + 0.2f);
        }
        ShowExplosion();
        NetworkServer.Destroy(gameObject);
    }

    void FixedUpdate()
	{
        //if (!isServer) return;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + _direction, Speed * Time.deltaTime);
	}

    void OnTriggerEnter(Collider other)
    {
        if (!isServer) return;
        if ((Parent == other.gameObject)) return;

        other.SendMessage("Hit", this, SendMessageOptions.DontRequireReceiver);
        
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (!isServer) return;
    //    if ((Parent == collision.collider.gameObject)) return;

    //    collision.collider.gameObject.SendMessage("Hit", this, SendMessageOptions.DontRequireReceiver);
    //}

    void Hit(Bullet _bullet)
    {
        if ((!_bullet.IsEnemy) || (!IsEnemy))
        {
            Debug.Log("Пуля - дура!");
            _bullet.DestroyMy();
            DestroyMy();
        }
    }

}