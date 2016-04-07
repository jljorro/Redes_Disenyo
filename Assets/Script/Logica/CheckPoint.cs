using UnityEngine;
using System.Collections;

public class CheckPoint : NetComponent 
{
    public bool Checked { get; set; }

	// Use this for initialization
	void Awake () 
    {
        Checked = false;
	}

    void OnTriggerEnter(Collider other)
    {
        Checked = true;
    }
}
