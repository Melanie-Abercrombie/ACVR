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
            Canvas = 5,
            File = 6
        }
    }

    namespace Data
    {
        [System.Serializable]
        public class StateData
        {
            public OVRPlayerController OVRPlayer;
            public OVRInput.Controller hand;
            public OVRInput.Controller hand2;
            public Grabber grabber;
            public PocketInventoryManager pocketInventory;
            public CanvasInventoryManager canvasInventory;
            public FileBrowserMenu fileBrowser;

            StateData(OVRPlayerController OVRPlayer, OVRInput.Controller hand, OVRInput.Controller hand2, Grabber grabber, PocketInventoryManager pocketInventory, CanvasInventoryManager canvasInventory, FileBrowserMenu fileBrowser)
            {
                this.OVRPlayer = OVRPlayer;
                this.hand = hand;
                this.hand2 = hand2;
                this.grabber = grabber;
                this.pocketInventory = pocketInventory;
                this.canvasInventory = canvasInventory;
                this.fileBrowser = fileBrowser;
            }
        }
    }
}
