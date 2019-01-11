using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour {
    GameObject ball;
    private float theta;
    private Vector3 init;
    private Vector3 pos;
    private Vector3 holepos;
    private float alpha = 0;
    private bool right;
    private bool left;
    private bool home;
    private float angle;
    GameObject hole;
    GameObject cliant;

	// Use this for initialization
	void Start () {
        ball = GameObject.Find("ball");
        cliant = GameObject.Find("TCPServerLancher");
        hole = GameObject.Find("hole");
        init = ball.GetComponent<Transform>().position;
        holepos = hole.GetComponent<Transform>().position;
        theta = Mathf.Atan((holepos.x - init.x) / (holepos.z - init.z)) * Mathf.Rad2Deg;
        if (holepos.z > init.z)
        {
            theta += 180f;
        }
	}
	
	// Update is called once per frame
	void Update () {
        pos = ball.GetComponent<Transform>().position;
        right = cliant.GetComponent<WiimoteInfoDisplayBase>().balanceBoardData.right;
        left = cliant.GetComponent<WiimoteInfoDisplayBase>().balanceBoardData.left;
        home = cliant.GetComponent<WiimoteInfoDisplayBase>().balanceBoardData.home;
        if (right)
        {
            alpha += 0.5f;
        }
        if (left)
        {
            alpha -= 0.5f;
        }
        if (home)
        {
            alpha = 0f;
        }
        transform.position = new Vector3(pos.x, 0f, pos.z);
        GetComponent<Transform>().eulerAngles = new Vector3(0f, 180f + theta + alpha, 0f);
	}
}
