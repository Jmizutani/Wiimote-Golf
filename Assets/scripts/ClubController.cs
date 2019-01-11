using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClubController : MonoBehaviour {
	
	private float theta;
	private Vector3 init;
	private Vector3 holepos;
	private Vector3 pos;
	private float alpha = 0;
	GameObject ball;
	GameObject hole;
	public float speed=10;
	public float angular_x = 10;
	public float angular_y = 10;

	void Start(){
		ball = GameObject.Find ("ball");
		init = ball.GetComponent<Transform> ().position;
		hole = GameObject.Find ("hole");
		holepos = hole.GetComponent<Transform> ().position;
		theta = Mathf.Atan ((holepos.x - init.x) / (holepos.z - init.z)) * Mathf.Rad2Deg;
		if (holepos.z > init.z) {
			theta += 180f;
		}
	}

	public void Update () {
		if(Input.GetKey("right")){
			alpha += 0.5f;
		}
		if (Input.GetKey ("left")) {
			alpha -= 0.5f;
		}
		if (Input.GetKey ("backspace")) {
			alpha = 0f;
		}
	}

	void OnTriggerEnter(Collider hit) {
		if (hit.CompareTag("ball")) {
			hit.gameObject.GetComponent<Rigidbody> ().AddForce (-2 * speed * Mathf.Sin (Mathf.Deg2Rad * (theta + alpha)), 1 * speed, -2 * speed * Mathf.Cos (Mathf.Deg2Rad * (theta + alpha)), ForceMode.VelocityChange);
			hit.gameObject.GetComponent<Rigidbody> ().AddTorque (angular_x * Mathf.Cos (Mathf.Deg2Rad * (theta + alpha)), 100000 * angular_y, angular_x * Mathf.Sin (Mathf.Deg2Rad * (theta + alpha)), ForceMode.VelocityChange);
		}
	}

}
