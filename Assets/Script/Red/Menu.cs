using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	void OnGUI ()
    {
        if (GUIInfo.Button(0.1f, 0.2f, 0.2f, 0.2f, "Play"))
            Application.LoadLevel("Circuito");
        if (GUIInfo.Button(0.4f, 0.2f, 0.2f, 0.2f, "Server"))
            Application.LoadLevel("Server");
        if (GUIInfo.Button(0.7f, 0.2f, 0.2f, 0.2f, "Client"))
            Application.LoadLevel("Client");
        if (GUIInfo.Button(0.4f, 0.6f, 0.2f, 0.2f, "Exit"))
            Application.Quit();
	}
}
