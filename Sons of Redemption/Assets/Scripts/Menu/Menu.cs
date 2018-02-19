using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour {

    public void StartGame()
    {
        Application.LoadLevel(1);

    }
    public void StartMenu()
    {
        Application.LoadLevel(0);
    }

    public void quit()
    {

        Application.Quit();
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
