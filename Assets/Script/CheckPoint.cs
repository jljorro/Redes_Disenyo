using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class CheckPoint : NetworkBehaviour {
    
    public bool Checked { get; set; }

	// Use this for initialization
	void Awake () {
        Checked = false;
	}

    void OnTriggerEnter(Collider other) {
        Checked = true;
    }
}
