using UnityEngine;
using System.Collections;

public class PressAnyKeyToContinue : MonoBehaviour 
{
	UnityEngine.UI.Text _text;

	bool _sceneIsReady = false;
	void Start () 
	{
		_text = GetComponent<UnityEngine.UI.Text>();
		_text.enabled = false;

		// Enable the text when the load is completed!
		DpLoadScreen.Instance.OnStartWaitingEventToActivateScene += delegate {
			_text.enabled = true;
			_sceneIsReady = true;
		};
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(_sceneIsReady && Input.anyKeyDown)
		{
			// when we got any get down and the text is enabled (ie. the event OnWaitingEventToActivateScene is fired), then we can finally activate the scene!
			DpLoadScreen.Instance.ActivateScene();
		}
	}
}
