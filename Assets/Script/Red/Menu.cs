using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Clase que controla la GUI del menú y carga la escena correspondiente.
/// </summary>
public class Menu : MonoBehaviour {

	void OnGUI () {
        if (GUIInfo.Button(0.1f, 0.2f, 0.2f, 0.2f, "Play"))
            SceneManager.LoadScene("Circuito");
        if (GUIInfo.Button(0.4f, 0.2f, 0.2f, 0.2f, "Server"))
            SceneManager.LoadScene("Server");
        if (GUIInfo.Button(0.7f, 0.2f, 0.2f, 0.2f, "Client"))
            SceneManager.LoadScene("Client");
        if (GUIInfo.Button(0.4f, 0.6f, 0.2f, 0.2f, "Exit"))
            Application.Quit();
	}
}
