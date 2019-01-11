using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ray : MonoBehaviour {

    private float vel;
    private Vector3 velocity;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Rigidbody rigidbody = GetComponent<Rigidbody> ();
		Transform transform = GetComponent<Transform> ();
        velocity = rigidbody.velocity;
        vel = Mathf.Sqrt(velocity.x * velocity.x + velocity.z * velocity.z);
        Ray ray = new Ray (transform.position, new Vector3 (0f, -1f, 0f));
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, 1f)) {
			switch (hit.collider.name) {
			case "fareway":
				rigidbody.drag = 0.0002f;
				rigidbody.angularDrag = 10f;
                rigidbody.sleepThreshold = 1f;
                break;
			case "green":
				rigidbody.drag = 0.02f;
				rigidbody.angularDrag = 10f;
                rigidbody.sleepThreshold = 10f;
                if (vel < 0.3) { rigidbody.Sleep(); }
                break;
			case "bunker":
				rigidbody.drag = 0.02f;
				rigidbody.angularDrag = 20f;
                rigidbody.sleepThreshold = 10f;
				break;
			case "teeshot":
				rigidbody.drag = 0.0002f;
				rigidbody.angularDrag = 1f;
                rigidbody.sleepThreshold = 2f;
                break;
			case "rough":
				rigidbody.drag = 0.0005f;
				rigidbody.angularDrag = 15f;
                rigidbody.sleepThreshold = 5f;
                break;
			default:
                rigidbody.angularDrag = 1f;
                rigidbody.sleepThreshold = 2f;
				break;
			}
		}
	}
}
