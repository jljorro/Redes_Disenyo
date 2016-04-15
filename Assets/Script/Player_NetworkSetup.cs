using UnityEngine;
using UnityEngine.Networking;

public class Player_NetworkSetup : NetworkBehaviour {

	// Use this for initialization
	void Start () {
	
        if(isLocalPlayer) {           
            GetComponent<CharacterController> ().enabled = true;
            GetComponent<PlayerController> ().enabled = true;
            
            // Asignar la camara al personaje
            CameraController cameraCont= GameObject.Find ("Main Camera").GetComponent<CameraController> ();
            cameraCont.AddTarget (gameObject);
        }
    
	}
	
	// Update is called once per frame
	void Update () {}
    
}
