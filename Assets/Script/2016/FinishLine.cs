using UnityEngine;
using System.Collections.Generic;

public class FinishLine : MonoBehaviour {
    
    public Dictionary<GameObject, int> CarsLaps { get; set; }
    public Dictionary<GameObject, float> CarsLastLapTime { get; set; }
    public Dictionary<GameObject, float> CarsTime { get; set; }
    public GUIInfo _guiInfo;
    private GameObject[] _checkpoints;
    private int _currentLap = 0;

	// Use this for initialization
	void Awake ()
    {
        CarsLaps = new Dictionary<GameObject, int>();
        CarsLastLapTime = new Dictionary<GameObject, float>();
        CarsTime = new Dictionary<GameObject, float>();
	}

    void Start() {
        _checkpoints = GameObject.FindGameObjectsWithTag("CheckPoint");
        _guiInfo = GetComponent<GUIInfo> ();
    }

    public void AddCar(string carName)
    {
        CarsLaps.Add(GameObject.Find(carName), 0);
        CarsLastLapTime.Add(GameObject.Find(carName), 99f);
        CarsTime.Add(GameObject.Find(carName), 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        foreach (GameObject cp in _checkpoints)
        {
            if (!cp.GetComponent<CheckPoint>().Checked)
                return;
        }

        int lap = ++CarsLaps[other.gameObject];
        //SendMessage(other.gameObject, "Lap", lap);
        _guiInfo.Lap (lap);
        CarsLastLapTime[other.gameObject] = Time.time - CarsTime[other.gameObject];
        CarsTime[other.gameObject] = Time.time;
        _guiInfo.LapTime (CarsLastLapTime[other.gameObject]);
       //SendMessage(other.gameObject, "LapTime", CarsLastLapTime[other.gameObject]);
        if (lap > _currentLap)
        {
            _currentLap = lap;
            //SendMessage("NewLider", other.gameObject.name, true);
            _guiInfo.NewLider (other.gameObject.name);
        }

        foreach (GameObject cp in _checkpoints)
        {
            cp.GetComponent<CheckPoint>().Checked = false;
        }
    }
}
