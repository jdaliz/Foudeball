using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {




	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Escape) )
			QuitterApplication();
	}

	public void ChargerScene(string LevelName)
	{
		Application.LoadLevel(LevelName);
	}

	
	public void QuitterApplication()
	{
		Application.Quit ();
	}
}
