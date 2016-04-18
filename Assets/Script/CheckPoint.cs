using UnityEngine;

public class CheckPoint : MonoBehaviour {
    
    public bool Checked { get; set; }

	// Use this for initialization
	void Awake () {
        Checked = false;
	}

    void OnTriggerEnter(Collider other) {
        Checked = true;
    }
}
