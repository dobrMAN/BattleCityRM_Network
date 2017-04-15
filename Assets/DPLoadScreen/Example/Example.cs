using UnityEngine;
using System.Collections;

public class Example : MonoBehaviour {

	
	public void Loadlevel()
	{
		// Replace Application.LoadLevel("BigScene"); with
		DpLoadScreen.Instance.LoadLevel("BigScene");
	}

	public void Loadlevel2()
	{
		// Alternative method showing to wait a key press to continue and using a second load scene
		DpLoadScreen.Instance.LoadLevel("BigScene", true, "LoadScreenPressToContinue");
	}

}
