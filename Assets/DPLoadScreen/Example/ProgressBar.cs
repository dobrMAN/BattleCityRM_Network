using UnityEngine;
using System.Collections;

public class ProgressBar : MonoBehaviour 
{
	UnityEngine.UI.Slider _slider;
	void Start () {
		_slider = GetComponent<UnityEngine.UI.Slider>();
	}
	
	void Update () {
		// Use the property Progress to get the load percentage! Remember the value is between 0 and 100.
		_slider.value = DpLoadScreen.Instance.Progress;
	}
}
