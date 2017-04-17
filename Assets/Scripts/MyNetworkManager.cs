using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;


//TODO Организовать работу по bluetooth. Ссылки по теме:
// https://github.com/DarkRay/Unity3D-bluetooth
// https://forum.unity3d.com/threads/android-bluetooth-multiplayer-new-version.188667/


public class MyNetDiscovery : NetworkDiscovery
{
    public string FindedIp = null;
    public static MyNetDiscovery singleton;

    public static bool stopConfirmed = false;

    void Start()
    {
        showGUI = false;
        useNetworkManager = false;
    }
    public override void OnReceivedBroadcast(string fromAdress, string data)
    {
        FindedIp = new string (fromAdress.ToCharArray());
    }
    void Awake()
    {
        if (singleton != null && singleton != this)
            this.enabled = false;
        else
            singleton = this;
    }

    public new void StopBroadcast()
    {
        if (running)
            base.StopBroadcast();
        ConfirmStopped();
    }

    void LateUpdate()
    {
        if (!running && !stopConfirmed)
            ConfirmStopped();
    }

    void ConfirmStopped()
    {
        try
        {
            stopConfirmed = !NetworkTransport.IsBroadcastDiscoveryRunning();
        }
        catch (Exception)
        {
            stopConfirmed = true;
        }
    }
}

[RequireComponent(typeof(MyNetDiscovery))]
public class MyNetworkManager : NetworkManager//NetworkBehaviour
{
    public MyNetDiscovery Discovery;
    ////------------------------------------------------------

    public static void StopClientAndBroadcast()
    {
        MyNetDiscovery.singleton.StopBroadcast();
        onBroadcastStopped += singleton.StopClient;
    }

    public static void StopServerAndBroadcast()
    {
        MyNetDiscovery.singleton.StopBroadcast();
        onBroadcastStopped += singleton.StopServer;
    }

    public static void StopHostAndBroadcast()
    {
        MyNetDiscovery.singleton.StopBroadcast();
        onBroadcastStopped += singleton.StopHost;
    }

    private static event Action onBroadcastStopped;

    void Update()
    {
        if (onBroadcastStopped != null)
        {
            if (!MyNetDiscovery.singleton.running && MyNetDiscovery.stopConfirmed)
            {
                onBroadcastStopped.Invoke();
                onBroadcastStopped = null;
            }
            else
            {
                if (LogFilter.logDebug)
                    Debug.Log("Waiting for broadcasting to stop completely", gameObject);
                MyNetDiscovery.singleton.StopBroadcast();
            }
        }

        if ((Discovery.isClient) && (Discovery.FindedIp != null))
        {
            Debug.Log("Нашли сервер. IP:" + Discovery.FindedIp);
            MyNetDiscovery.singleton.StopBroadcast();
            networkAddress = Discovery.FindedIp;
            networkPort = 7777;
            StartClient();
        }
    }
 
    private void Start()
    {
        Discovery = gameObject.AddComponent<MyNetDiscovery>();
    }

    public override void OnStartHost()
    {
        MyNetDiscovery.singleton.Initialize();
        MyNetDiscovery.singleton.StartAsServer();
        base.OnStartHost();
    }

    public override void OnStartClient(NetworkClient client)
    {
        base.OnStartClient(client);
    }

    public override void OnStopClient()
    {
        StopClientAndBroadcast();
        base.OnStopClient();
    }

    public override void OnStopHost()
    {
        StopHostAndBroadcast();
        base.OnStopHost();
    }

    public void FindLocalHost()
    {
        Debug.Log("Поиск хоста в локальной сети");
        MyNetDiscovery.singleton.showGUI = false;
        MyNetDiscovery.singleton.Initialize();
        MyNetDiscovery.singleton.StartAsClient();

    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        print("Player connected to server.");
        var spawnPosition = startPositions[numPlayers];
        var player = (GameObject)Instantiate(base.playerPrefab, spawnPosition.position, Quaternion.identity);
        player.GetComponent<Player>().ID = numPlayers + 1;

        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        //base.OnServerAddPlayer(conn, playerControllerId);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        print("Player connected to server with extra messages: " + extraMessageReader);
        //base.OnServerAddPlayer( conn, playerControllerId,extraMessageReader);
        var spawnPosition = startPositions[numPlayers];
        var player = (GameObject)Instantiate(base.playerPrefab, spawnPosition.position, Quaternion.identity);
        player.GetComponent<Player>().ID = numPlayers + 1;
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, PlayerController player)
    {

        base.OnServerRemovePlayer(conn, player);
        Debug.Log("Игрок отключился, на сервере осталось " + numPlayers + " игрок(а)");
    }
}
