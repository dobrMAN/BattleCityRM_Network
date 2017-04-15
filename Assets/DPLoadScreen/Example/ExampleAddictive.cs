using UnityEngine;
using System.Collections;

public class ExampleAddictive : MonoBehaviour {

	UnityEngine.UI.Slider _slider;
	
	void Start () 
	{
		_slider = GetComponent<UnityEngine.UI.Slider>();
		_slider.gameObject.SetActive(false);
	}

	public void LoadAsync()
	{
		// Setup the callbacks to display and hide the status bar
		DpLoadScreen.Instance.OnStartLoadEventAddictive += () => _slider.gameObject.SetActive(true);
		DpLoadScreen.Instance.OnEndLoadEventAddictive  += () => _slider.gameObject.SetActive(false);

		// loads the scene
		DpLoadScreen.Instance.LoadLevelAddictive("AddictiveScene");
	}
		
	// Update is called once per frame
	void Update () 
	{
		_slider.value = DpLoadScreen.Instance.Progress;
	}
}
