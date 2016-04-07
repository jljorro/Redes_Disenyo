using UnityEngine;
using System.Collections;

public class CreatePlayer : NetComponent
{
    public GameObject _car;
    private GameObject _finishLine;

    // Use this for initialization
    protected override void OnStart()
    {
        _finishLine = GameObject.Find("FinishLine");
    }
	
	void Create(string newName)
	{
        GameObject car = 
            Network.Instantiate(_car, transform.position, Quaternion.identity, 0)
            as GameObject;
        //car.SendMessage("ChangeName", newName);
        SendMessage(car, "ChangeName", newName);
        SendMessage(car, "IsLocalPlayer", newName);
        SendMessage(_finishLine, "AddCar", newName);
	}
}
