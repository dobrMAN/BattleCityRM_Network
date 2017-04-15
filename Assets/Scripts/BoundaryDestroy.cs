using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class BoundaryDestroy : NetworkBehaviour {

	void OnTriggerExit (Collider other) 
	{
        Bullet bullet = other.gameObject.GetComponent<Bullet>();
        if (bullet != null)
        {
            NetworkServer.Destroy(bullet.gameObject);
        };
	}
}
