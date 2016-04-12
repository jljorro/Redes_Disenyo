using UnityEngine;

/// <summary>
/// Clase que se encarga de mostrar la cámara que sigue al coche del
/// jugador.
/// </summary>
public class CameraController : MonoBehaviour {
    public float _defaultDistance = 18;
    
    private GameObject _target;
    private PlayerController _playerCon;

    public GameObject Target {
        get {return _target;}
        set {
            _target = value;
            _playerCon = _target.GetComponent<PlayerController>();
        }
    }

    void AddTarget(string name) {
        Target = GameObject.Find(name);
    }
	
	// Update is called once per frame
	void Update () {
        if(_target != null)
            transform.position = _target.transform.position + new Vector3(0, _defaultDistance  + _playerCon.Velocity.magnitude, 0);
	}
}
