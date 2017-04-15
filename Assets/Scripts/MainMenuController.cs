using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class MainMenuController : MonoBehaviour {
    [SerializeField]
    private GameObject _console;

    public MyNetworkManager NetMan;
	// Use this for initialization
	void Start () {
        _console.SetActive(true);
        DontDestroyOnLoad(_console);
        NetMan = MyNetworkManager.FindObjectOfType<MyNetworkManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			AppHelper.Quit();
		}
		if (Input.GetKeyDown(KeyCode.Return))
		{
			//AppHelper.setParam("Level","1");
			//AppHelper.Load ("Game");
		}
	}

	public void OnStart()
	{	//Destroy (gameObject);
		AppHelper.SetParam("Level","1");
        AppHelper.SetParam("Mode", "Single");
        NetMan.StartHost();
    }

    public void OnCreateHost()
    {
        AppHelper.SetParam("Level", "1");
        AppHelper.SetParam("Mode", "LocalHost");
        NetMan.StartHost();
    }

    public void OnConnectLocal()
    {
        AppHelper.SetParam("Level", "1");
        AppHelper.SetParam("Mode", "LocalClient");
        NetMan.FindLocalHost();
    }

    public void OnOptions()
	{

	}

	public void OnExit()
	{
		AppHelper.Quit();
	}
}