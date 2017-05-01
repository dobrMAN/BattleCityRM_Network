using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class MapClass
{

    public static MapClass Current;
    public int Index;
    public char[] MapData;
    public List<Enemy> EnemyList;

    public MapClass()
    {
        MapData = new char[26 * 26];
        EnemyList = new List<Enemy>();
    }

}

public class MyGameManager : NetworkBehaviour {
    public GameObject MyBrickBlock;
    public GameObject MyArmorBlock;
    public GameObject MyBaseBlock;
    public int Level;
    public bool BuildingMap;

    public string MapsDir;
    private GameObject _levelMap;
    private MapClass _mapData;

    public GameObject GameOverObj;

    [SerializeField]
    public ShaderVariantCollection Shaders;
    //[SyncVar]
    public bool IsSinglePlayer = false;
    //[SyncVar]
    public bool IsGameStarted = false;
    //[SyncVar]
    public bool IsWaitPlayers = true;
    public bool IsGameOver = false;
    [SyncVar]
    public int numPlayers;

    [SerializeField]
    private GameObject _waitPlayersDialog;

    //[SyncVar]
    //public MyNetworkManager NetMan;

    [SerializeField]
    public List<GameObject> ForInitialize = new List<GameObject>();

    private int CurrentEnemySpawn = 0;
    [SerializeField]
    public List<Transform> EnemySpawns = new List<Transform>();
    //private int PlayerCount = 0;
    [SerializeField]
    private List<Player> Players = new List<Player>();

    [SerializeField]
    public List<Enemy> EnemyList;
    public int EnemyCount = 0;
    WaitForSeconds wait5;


    public bool AddPlayer(Player _player)
    {
        //if (Players.Count > 1) return false;

        //int _ID = 1;

        //if (Players.Count > 0)
        //{
        //    _ID = Players[0].ID == 1 ? 2 : 1; 
        //}
        //PlayerCount++;
        //_player.ID = _ID;
        Players.Add(_player);

        return true;
    }
    //-------------------------------------------------------------------------------------------------------------------

    public void RemovePlayer(Player _player)
    {
        Players.Remove(_player);
        if (Players.Count<1)
        {
            IsGameOver = true;
            GameOverObj.GetComponent<Animation>().Play();
        }
    }

    void SpawnBrickBlock(Vector3 pos)
    {
        var obj = Instantiate(MyBrickBlock, pos+new Vector3(-0.1f,0, -0.1f), Quaternion.identity);
        obj.transform.SetParent(_levelMap.transform);
        NetworkServer.Spawn(obj);
        obj = Instantiate(MyBrickBlock, pos + new Vector3(0.1f, 0, -0.1f), Quaternion.identity);
        obj.transform.SetParent(_levelMap.transform);
        NetworkServer.Spawn(obj);
        obj = Instantiate(MyBrickBlock, pos + new Vector3(-0.1f, 0, 0.1f), Quaternion.identity);
        obj.transform.SetParent(_levelMap.transform);
        NetworkServer.Spawn(obj);
        obj = Instantiate(MyBrickBlock, pos + new Vector3(0.1f, 0, 0.1f), Quaternion.identity);
        obj.transform.SetParent(_levelMap.transform);
        NetworkServer.Spawn(obj);
    }
    //-------------------------------------------------------------------------------------------------------------------
    void SpawnBaseBlock(Vector3 pos)
    {
        var obj = Instantiate(MyBaseBlock, pos + new Vector3(0.2f, 0, -0.65f), Quaternion.Euler(-90, 0, 180));
        obj.transform.SetParent(_levelMap.transform);
        NetworkServer.Spawn(obj);
    }
    //-------------------------------------------------------------------------------------------------------------------
    void SpawnArmorBlock(Vector3 pos)
    {
        var obj = Instantiate(MyArmorBlock, pos, Quaternion.identity);
        obj.transform.SetParent(_levelMap.transform);
        NetworkServer.Spawn(obj);
    }
    //-------------------------------------------------------------------------------------------------------------------
//#if (UNITY_WEBGL)
    [Server]
        IEnumerator LoadMap(int level)
//#else
//    [Server]
//    void LoadMap(int level)
//#endif
    {
#if UNITY_EDITOR
        Debug.Log("Загрузка карты.");
        if (BuildingMap)
        {
            _mapData.MapData = new char[26*26]{
               '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '2', '2', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '2', '2', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '1', '1', '0', '0', '1', '1', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '1', '1', '1', '0', '0', '1', '1' ,   /*  initializers for row indexed by 0 */
			   '2', '2', '0', '0', '1', '1', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '1', '1', '1', '0', '0', '2', '2' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '1', '1', '1', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '1', '1', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '1', '1', '0', '0', '1', '1', '0', '0', '0', '1', '1', '1', '1', '0', '0', '0', '1', '1', '0', '0', '1', '1', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', 'B', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0' ,   /*  initializers for row indexed by 0 */
			   '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '1', '0', '0', '1', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0', '0'    /*  initializers for row indexed by 0 */
			        };
        }
        else
        {
#endif

            string name = "Level_" + level.ToString();
#if (UNITY_ANDROID || UNITY_WEBGL) && !UNITY_EDITOR
			BinaryFormatter formatter = new BinaryFormatter();
			MemoryStream loadMap;
            Debug.Log("Map address: "+MapsDir+name);
			WWW www = new WWW(MapsDir+name);
//#if (UNITY_ANDROID)
//			while (!www.isDone && www.error == null)
//			{
//
//			}
//#else
            while (!www.isDone && www.error == null) yield return www;
//#endif
			loadMap = new MemoryStream(www.bytes);
			_mapData = (MapClass)formatter.Deserialize(loadMap);
			loadMap.Close();
#else
            Debug.Log("Map address: " + MapsDir + name);
            if (!Directory.Exists(MapsDir))
                Directory.CreateDirectory(MapsDir);

            if (File.Exists(MapsDir + name))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream loadMap = File.Open(MapsDir + name, FileMode.Open);
                _mapData = (MapClass)formatter.Deserialize(loadMap);
                loadMap.Close();
            }
            else
            {
                Debug.Log("Файл " + MapsDir + name + " не найден!");
            }
#endif

#if UNITY_EDITOR
        }
#endif
        for (int y = 0; y < 26; y++)//MapData.GetLength(0); y++)MapData.GetLength(0); y++)
        {
            for (int x = 0; x < 26; x++)//MapData.GetLength(1); x++)
            {
                var pos = new Vector3(-5.0f + 0.4f * x, 0.3f, 5.0f - 0.4f * y);
                switch (_mapData.MapData[x + 26 * y])
                {
                    case '1':
                        SpawnBrickBlock(pos);
                        break;
                    case '2':
                        SpawnArmorBlock(pos);
                        break;
                    case 'B':
                        SpawnBaseBlock(pos);
                        break;
                }
            }
        }
//#if (UNITY_WEBGL)
        yield return null;
//#endif
    }
//-------------------------------------------------------------------------------------------------------------------

    public override void OnStartServer()
    {
        Debug.Log("Map initialisation.");

        ClientScene.RegisterPrefab(MyArmorBlock);
        ClientScene.RegisterPrefab(MyBaseBlock);
        ClientScene.RegisterPrefab(MyBrickBlock);
#if UNITY_EDITOR
        MapsDir = Application.dataPath + "/StreamingAssets/Maps/";
#elif UNITY_ANDROID
		MapsDir = "jar:file://" + Application.dataPath + "!/assets/Maps/";
#else
		MapsDir = Application.dataPath + "/StreamingAssets/Maps/";
#endif
#if UNITY_EDITOR
        if (!BuildingMap)
        {
#endif

            int.TryParse(AppHelper.GetParam("Level"), out Level);
            Debug.Log("Server load level " + Level);
            if (Level < 1)
                Level = 1;

#if UNITY_EDITOR
        }
#endif
        _levelMap = GameObject.Find("Map");
        _mapData = new MapClass();
        StartCoroutine(LoadMap(Level));
    }
    private void Awake()
    {
        Shaders.WarmUp();
        //Shader.WarmupAllShaders();
        //NetMan = GameObject.Find("MyNetworkManager").GetComponent<MyNetworkManager>();
    }
    //-------------------------------------------------------------------------------------------------------------------
    // Use this for initialization
    void Start () {
        
        //_waitPlayersDialog = GameObject.Find("WaitPlayersWindow");
        _waitPlayersDialog.SetActive(true);

        wait5 = new WaitForSeconds(5.0f);

        for (int i = 0; i < ForInitialize.Count; i++)
        {
            GameObject obj = Instantiate(ForInitialize[i]);
            obj.transform.Translate(new Vector3(10, 10, 10));
            if (isServer)
            {
                NetworkServer.Spawn(obj);
                /*networkserver.unspawn(obj);
                networkserver.destroy(obj);*/
            }/* else  destroy(obj)*/;
        }

        string mode = AppHelper.GetParam("Mode");
        IsGameStarted = false;
        if (mode == "Single")
        {
            IsSinglePlayer = true;
            //if (NetMan.numPlayers < 1) IsGameStarted = false;
        }
        else
        {
            IsSinglePlayer = false;
            //if (NetMan.numPlayers < 2) IsGameStarted = false;
        };

        //if (isServer && IsGameStarted)
            //StartGame();

        Debug.Log("Start Game Manager.");
        if (isServer) Debug.Log("Server side start.");

        Time.timeScale = 0;
        //if (isClient)
        //    ClientScene.AddPlayer(NetworkManager.singleton.client.connection, 0);

    }
    //-------------------------------------------------------------------------------------------------------------------


    void SaveMap(int level)
    {
        string Name = "Level_" + level.ToString();

        if (!Directory.Exists(MapsDir))
            Directory.CreateDirectory(MapsDir);

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveMap = File.Create(MapsDir + Name);


        formatter.Serialize(saveMap, _mapData);

        saveMap.Close();
    }
    //-------------------------------------------------------------------------------------------------------------------

    void Update()
    {
        if (isServer) numPlayers = MyNetworkManager.singleton.numPlayers;
        if (!IsGameStarted)
        {
            //Debug.Log("IsWaitPlayer=" + IsWaitPlayers + " IsSinglePlayer=" + IsSinglePlayer + " numPlayers=" + numPlayers);
           
            if (!IsSinglePlayer)
            {
                //Debug.Log("Сетевой режим");
                if (!IsWaitPlayers && (numPlayers < 2))
                {
                    _waitPlayersDialog.SetActive(true);
                    IsWaitPlayers = true;
                    Debug.Log("Ждем игроков.");
                }

                if (IsWaitPlayers && (numPlayers == 2))
                {
                    IsWaitPlayers = false;
                    _waitPlayersDialog.SetActive(false);
                    Debug.Log("Запускаем игру.");
                    StartGame();

                }
            }
            else
            {
                if (!IsWaitPlayers && (MyNetworkManager.singleton.numPlayers < 1))
                {
                    _waitPlayersDialog.SetActive(true);
                    IsWaitPlayers = true;
                    Debug.Log("Ждем игроков.");
                }

                if (IsWaitPlayers && (MyNetworkManager.singleton.numPlayers == 1))
                {
                    IsWaitPlayers = false;
                    _waitPlayersDialog.SetActive(false);
                    print("Try start game");
                    StartGame();

                }
            }
        }

        //print("IsWaitPlayers=" + IsWaitPlayers + " IsGameStarted=" + IsGameStarted + " ConnectedPlayers=" + NetworkManager.singleton.numPlayers);
        
        //TODO Заменить обработчик ESCAPE, выводить меню по нажатию, а не выходить сразу в главное меню.

        if ((EnemyList.Count == 0) && (EnemyCount == 0))
        {
            //TODO You win this level!
            //Debug.Log("You WIN!");
        }

        if (isLocalPlayer || isClient)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitGame();
            }

#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Backslash))
            {
                SaveMap(Level);
                Debug.Log("Карта " + Level.ToString() + " сохранена.");
            }
#endif
        }

    }

    //private void FixedUpdate()
    //{


    //}

    public void ExitGame()
    {
        //MyNetworkManager.singleton.StopServer();
        if (isLocalPlayer)
        {
            //MyNetworkManager.singleton.StopHost();
            MyNetworkManager.singleton.StopClient();
            ClientScene.RemovePlayer(0);
            Debug.Log("Отключен локальный игрок, на сервере осталось игрков - " + MyNetworkManager.singleton.numPlayers);
        };

        if (isClient)
        {
            ClientScene.RemovePlayer(0);
            MyNetworkManager.singleton.StopClient();
            Debug.Log("Отключен клиент, на сервере осталось игрков - " + MyNetworkManager.singleton.numPlayers);
        }

        if (isServer)
            MyNetworkManager.singleton.StopServer();

        MyNetworkManager.Shutdown();
        Destroy(GameObject.Find("MyNetworkManager"));
        AppHelper.Load("MainMenu");
    }
    //-------------------------------------------------------------------------------------------------------------------

        void StartGame()
    {
        IsGameStarted = true;
        Time.timeScale = 1;
        Debug.Log("Время запущено!");
        if (isLocalPlayer) return;

        StartCoroutine(SpawnEnemys());
        Debug.Log("Игра запущена!");
    }

    IEnumerator SpawnEnemys()
    {
        if (!isServer) yield return null;
        for (;;)
        {
            if (EnemyList.Count > 0)
            {
                yield return StartCoroutine(PrespawnEnemy());
                SpawnOneEnemy();
                yield return wait5;
            }
            else
            {
               StopCoroutine(SpawnEnemys());
                yield return null;
            }
        };
    }

    void SpawnOneEnemy()
    {
        if (!isServer) return;
        var obj = Instantiate(EnemyList[EnemyList.Count - 1], EnemySpawns[CurrentEnemySpawn].position, EnemySpawns[CurrentEnemySpawn].rotation, EnemySpawns[CurrentEnemySpawn]);
        NetworkServer.Spawn(obj.gameObject);
        CurrentEnemySpawn++;
        if (CurrentEnemySpawn >= EnemySpawns.Count) CurrentEnemySpawn = 0;
        EnemyList.Remove(EnemyList[EnemyList.Count - 1]);
        EnemyCount++;
    }

    IEnumerator PrespawnEnemy()
    {
        //TODO Start GFX of spwning enemy, then check if SpawnPosition is free.
       yield return null;
    }

    void SpawnPlayer()
    {
        if (!isServer) return;


    }

    public void BaseDestructed()
    {
        IsGameOver = true;
        GameOverObj.GetComponent<Animation>().Play();
    }
}
