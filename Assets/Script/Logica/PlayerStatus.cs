using UnityEngine;
using System.Collections;

public class PlayerStatus : NetComponent
{
    public Vector3 Velocity { get; set; }

    void Awake()
    {
        Velocity = new Vector3(0, 0, 0);
    }

    protected override void OnStart()
    {
        GameObject.FindWithTag("MainCamera").GetComponent<CameraController>().Target = gameObject;
        GameObject finishLine = GameObject.FindWithTag("FinishLine");
        SendMessage(finishLine, "AddCar", gameObject.name);
    }

    void ChangeName(string newName)
	{
		name = newName;
	}
}
