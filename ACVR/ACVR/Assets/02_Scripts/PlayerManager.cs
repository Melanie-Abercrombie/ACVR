using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour {

    public OVRPlayerController OVRPlayer;
    public OVRInput.Controller hand;
    public GrabberControls grabber;
    public PocketInventoryManager pocketInventory;

    private bool interactingWithInventory;
    private float slowUpdateInterval = 0.25f;

    void Start()
    {
        // consider creating the inventory and the grabber in here (probably not tho)
        InvokeRepeating("SlowUpdate", 0, slowUpdateInterval);
    }

    void LateUpdate()
    {
        GrabberCheck();
    }

    void SlowUpdate()
    {
        InventoryCheck();
    }

    void GrabberCheck()
    {
        if (!IsHolding() && TriggerIsPressed())
        {
            grabber.AttemptGrab();
        }

        if (IsHolding() && GripIsPressed())
        {
            grabber.AttemptSnap();
        }

        if (IsHolding() && !TriggerIsPressed())
        {
            // constantly tries to drop
            grabber.DropObject();
        }
    }

    void InventoryCheck()
    {
        if (!IsUsingInventory())
        {
            if (TriggerIsPressed() && ThumbstickIsPressed())
                StartUsingInventory();
        }

        if (IsUsingInventory())
        {
            if (!TriggerIsPressed() || !ThumbstickIsPressed())
            {
                StopUsingInventory();
            }
            else
            {
                if (ThumbstickIsRight())
                {
                    pocketInventory.ItemForward();
                    grabber.SetHeldObject(pocketInventory.GetCurrentInventoryObject());
                }
                else if (ThumbstickIsLeft())
                {
                    pocketInventory.ItemBackward();
                    grabber.SetHeldObject(pocketInventory.GetCurrentInventoryObject());
                }
            }
        }
    }

    void StartUsingInventory()
    {
        Debug.Log("Starting inventory interaction");
        OVRPlayer.EnableLinearMovement = false;
        OVRPlayer.EnableRotation = false;
        interactingWithInventory = true;
        pocketInventory.SetCurrentToTarget(grabber.HeldObject());
    }

    void StopUsingInventory()
    {
        Debug.Log("Stopping inventory interaction");
        OVRPlayer.EnableLinearMovement = true;
        OVRPlayer.EnableRotation = true;
        interactingWithInventory = false;
    }

    bool IsUsingInventory()
    {
        return interactingWithInventory;
    }

    bool TriggerIsPressed()
    {
        return (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, hand) > 0.8f);
    }

    bool GripIsPressed()
    {
        return (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, hand) > 0.8f);
    }
    
    bool ThumbstickIsPressed()
    {
        return (OVRInput.Get(OVRInput.Button.PrimaryThumbstick, hand));
    }

    bool ThumbstickIsRight()
    {
        return (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, hand).x > 0.8);
    }

    bool ThumbstickIsLeft()
    {
        return (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, hand).x < -0.8);
    }

    public GameObject HeldObject()
    {
        return grabber.HeldObject();
    }

    public bool IsHolding()
    {
        return grabber.IsHolding();
    }
}
