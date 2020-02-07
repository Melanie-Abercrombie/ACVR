using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDetector : MonoBehaviour {

    private bool hasCollided;

	// Use this for initialization
	void Start () {
        hasCollided = false;
	}
    
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "WallArt")
            {
                hasCollided = true;
                //gameObject.transform.Find("ColliderWarning").GetComponent<MeshRenderer>().enabled = true;
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag == "WallArt")
            {
                hasCollided = false;
                //gameObject.transform.Find("ColliderWarning").GetComponent<MeshRenderer>().enabled = false;
            }
        }

        public bool GetCollisionStatus()
        {
            return hasCollided;
        }
}
