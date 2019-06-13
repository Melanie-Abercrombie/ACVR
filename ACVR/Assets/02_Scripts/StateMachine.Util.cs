using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine.Util
{
    namespace Enum
    {
        public enum StateEnum
        {
            Default = 0,
            Context = 1,
            Pocket = 2,
            Inventory = 3,
            Hold = 4,
            Canvas = 5
        }
    }

    namespace Data
    {
        [System.Serializable]
        public class StateData
        {
            public OVRPlayerController OVRPlayer;
            public OVRInput.Controller hand;
            public Grabber grabber;
            public PocketInventoryManager pocketInventory;
            public CanvasInventoryManager canvasInventory;

            StateData(OVRPlayerController OVRPlayer, OVRInput.Controller hand, Grabber grabber, PocketInventoryManager pocketInventory, CanvasInventoryManager canvasInventory)
            {
                this.OVRPlayer = OVRPlayer;
                this.hand = hand;
                this.grabber = grabber;
                this.pocketInventory = pocketInventory;
                this.canvasInventory = canvasInventory;
            }
        }
    }
}
