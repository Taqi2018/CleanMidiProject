using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;
public class InputManager : MonoBehaviour
{ 
    
    public AppInput input;

    // Start is called before the first frame


    
    public static EventHandler <TapInputEventArgs>OnTapInput;

    public class TapInputEventArgs : EventArgs
    {
        public Vector2 TouchPosition;

    }
    void Start()
    { 
       
       
        input = new AppInput();
        
        input.Enable();

        input.MidiInput.Tap.performed +=OnTap;

    }

    private void OnTap(InputAction.CallbackContext obj)
    {

        Debug.Log("Tap");




        Vector2 touchPos = input.MidiInput.TapPos.ReadValue<Vector2>();
        OnTapInput?.Invoke(this, new TapInputEventArgs { TouchPosition = touchPos });







    }



}
