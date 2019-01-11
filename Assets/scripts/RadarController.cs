using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarController : MonoBehaviour {
    GameObject ball;
    private Vector3 pos;

	// Use this for initialization
	void Start () {
        ball = GameObject.Find("ball");
	}
	
	// Update is called once per frame
	void Update () {
        pos = ball.GetComponent<Transform>().position;
        transform.position = pos;
	}
}
