using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

/// <summary>
/// Clase que controla la meta y los tiempos de vuelta que se
/// van haciendo en el juego.
/// </summary>
public class FinishLine : NetworkBehaviour {
    
    public Dictionary<GameObject, int> CarsLaps { get; set; }
    public Dictionary<GameObject, float> CarsLastLapTime { get; set; }
    public Dictionary<GameObject, float> CarsTime { get; set; }
    private GUIInfo _guiInfo;
    private GameObject[] _checkpoints;
    private int _currentLap = 0;

	// Use this for initialization
	void Awake () {
        CarsLaps = new Dictionary<GameObject, int>();
        CarsLastLapTime = new Dictionary<GameObject, float>();
        CarsTime = new Dictionary<GameObject, float>();
	}

    void Start() {
        _checkpoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        _guiInfo = GetComponent<GUIInfo> ();
    }

    [Command]
    public void CmdAddCar(string carName) {
        CarsLaps.Add(GameObject.Find(carName), 0);
        CarsLastLapTime.Add(GameObject.Find(carName), 99f);
        CarsTime.Add(GameObject.Find(carName), 0f);
    }

    void OnTriggerEnter(Collider other) {
        
        // Comprobamos que ha pasado por todos los puntos de control
        foreach (GameObject cp in _checkpoints) {
            if (!cp.GetComponent<CheckPoint>().Checked)
                return;
        }
        
        //Marcamos el número de vueltas
        int lap = ++CarsLaps[other.gameObject];
        _guiInfo.Lap (lap);
        
        // Marcamos el tiempo de la última vuelta
        CarsLastLapTime[other.gameObject] = Time.time - CarsTime[other.gameObject];
        
        // Marcamos el tiempo de la vuelta
        CarsTime[other.gameObject] = Time.time;
        _guiInfo.LapTime (CarsLastLapTime[other.gameObject]);
        
        // Si la vuelta es mejor que la mejor vuelta le marcamos como lider
        if (lap > _currentLap) {
            _currentLap = lap;
            _guiInfo.NewLider (other.gameObject.name);
        }

        // Reseteamos todos los checkpoints para ese coche
        foreach (GameObject cp in _checkpoints) {
            cp.GetComponent<CheckPoint>().Checked = false;
        }
    }
}
