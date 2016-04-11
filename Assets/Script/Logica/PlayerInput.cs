using UnityEngine;
using System.Collections;

public class PlayerInput : NetComponent 
{
    float _vInput;
    float _hInput;
	
	// Update is called once per frame
    void Update()
    {
        float vInput = Input.GetAxisRaw("Vertical");
        if (_vInput != vInput)
        {
            _vInput = vInput;
            SendMessage("SetVInput", vInput);
        }
        float hInput = Input.GetAxisRaw("Horizontal");
        if (_hInput != hInput)
        {
            _hInput = hInput;
            SendMessage("SetHInput", hInput);

        }	
	}
}
