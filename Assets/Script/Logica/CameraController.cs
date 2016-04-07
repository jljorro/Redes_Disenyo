using UnityEngine;
using System.Collections;

public class CameraController : NetComponent 
{
    public float _defaultDistance = 18;
    private GameObject _target;
    private PlayerStatus _status;

    public GameObject Target
    {
        get
        {
            return _target;
        }
        set
        {
            _target = value;
            _status = _target.GetComponent<PlayerStatus>();
        }
    }

    void AddTarget(string name)
    {
        Target = GameObject.Find(name);
    }
	
	// Update is called once per frame
	void LateUpdate () 
    {
        if(_target != null)
            transform.position = _target.transform.position + new Vector3(0, _defaultDistance  + _status.Velocity.magnitude, 0);
	}
}
