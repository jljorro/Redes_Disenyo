using UnityEngine;
using System.Collections;

public class Icono : MonoBehaviour 
{
    public string _icono;
	// Use this for initialization
	void Start () 
    {
	
	}

    void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, _icono);
    }
}
