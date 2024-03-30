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


    
    public static EventHandler <TapInputEventArgs>OnTapEndInput;
    public static EventHandler<TapInputEventArgs> OnTapStartInput;

    public static InputManager instance;
    public class TapInputEventArgs : EventArgs
    {
        public Vector2 TouchPosition;

    }

    void Start()
    {
        instance = this;
       
        input = new AppInput();
        
        input.Enable();

        input.MidiInput.Tap.performed +=OnTap;
        input.MidiInput.Tap.canceled += OnTapEnd;

    }


    private void OnTapEnd(InputAction.CallbackContext obj)
    {
       
        Vector2 touchPos = input.MidiInput.TapPos.ReadValue<Vector2>();
        OnTapEndInput?.Invoke(this, new TapInputEventArgs { TouchPosition = touchPos });
    }

    private void OnTap(InputAction.CallbackContext obj)
    {

  

        Vector2 touchPos = input.MidiInput.TapPos.ReadValue<Vector2>();
        OnTapStartInput?.Invoke(this, new TapInputEventArgs { TouchPosition = touchPos });

        /*


                Vector2 touchPos = input.MidiInput.TapPos.ReadValue<Vector2>();
                OnTapInput?.Invoke(this, new TapInputEventArgs { TouchPosition = touchPos });

        */





    }



}
