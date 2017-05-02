using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSDisplay : MonoBehaviour
{
    float deltaTime = 0.0f;
    [SerializeField]
    Text UItext;
    MyGameManager MGM;

    void Start()
    {
        MGM = GameObject.Find("MyGameManager").GetComponent<MyGameManager>();
    }
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        int _playerscount = 0;
        int _enemyscount = 0;

        if (MGM == null)
        {
            MGM = GameObject.Find("MyGameManager").GetComponent<MyGameManager>();
            _enemyscount = 0;
        }
        else _enemyscount = MGM.EnemyList.Count;
        if (MyNetworkManager.singleton != null) _playerscount = MyNetworkManager.singleton.numPlayers;
        string text = string.Format("{0:0.} fps ({1:0.0} ms) Players {2:0} Enemyes {3:0}", fps, msec, _playerscount, _enemyscount);
        UItext.text = text;
    }


}