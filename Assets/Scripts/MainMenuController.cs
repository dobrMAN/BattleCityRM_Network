using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
#if (UNITY_ANDROID)
using UnityEngine.Advertisements;
#endif

public class MainMenuController : MonoBehaviour {
    [SerializeField]
    private GameObject _console;

    public MyNetworkManager NetMan;
	// Use this for initialization
	void Start () {
#if (UNITY_ANDROID)
        Advertisement.Initialize("1385614");
        Debug.Log("Поддержка рекламы: "+Advertisement.isSupported);
#endif
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
        //NetMan.useWebSockets = true;
        NetMan.StartHost();
        //MyNetworkManager.singleton.StartHost();
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

    public void ShowAd()
    {
#if (UNITY_ANDROID)
        if (Advertisement.IsReady())
        {
            Advertisement.Show();
        }
#endif
    }
}