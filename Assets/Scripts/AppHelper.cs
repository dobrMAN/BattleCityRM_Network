using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public static class AppHelper {

	#if UNITY_WEBPLAYER
	public static string webplayerQuitURL = "http://google.com";
	#endif
	private static Dictionary<string, string> _parameters;




	//public static void Load(string sceneName, Dictionary<string, string> parameters = null) {
	//	_parameters = parameters;
	//	DpLoadScreen.Instance.LoadLevel(sceneName, false, "LoadScreen");
	//	//SceneManager.LoadScene(sceneName,LoadSceneMode.Single);
	//}

	//public static void Load(string sceneName, string paramKey, string paramValue)
	//{
	//	SetParam(paramKey,paramValue);
	//	DpLoadScreen.Instance.LoadLevel(sceneName, false, "LoadScreen");
 //       //SceneManager.LoadScene(sceneName,LoadSceneMode.Single);
	//}

    //public static void Unload(string _sceneName)
    //{
    //    SceneManager.UnloadSceneAsync(_sceneName);
    //}

	public static Dictionary<string, string> GetSceneParameters()
	{
		return _parameters;
	}

	public static string GetParam(string paramKey)
	{
		if (_parameters == null) return "";
		return _parameters[paramKey];
	}

	public static void SetParam(string paramKey, string paramValue)
	{
		if (_parameters == null)
		{
			_parameters = new Dictionary<string, string> ();
			//Debug.Log ("Новый словарь параметров");
		}
		_parameters [paramKey] = paramValue;//.Add (paramKey, paramValue);
	}

	public static void Quit()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#elif UNITY_WEBPLAYER
		Application.OpenURL(webplayerQuitURL);
		#else
		Application.Quit();
		#endif
	}
}
