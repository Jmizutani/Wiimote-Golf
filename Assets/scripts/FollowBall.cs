using UnityEngine;
using System.Collections;

public class FollowBall : MonoBehaviour
{
	private float theta;
	private Vector3 init;
	private Vector3 pos;
	private Vector3 holepos;
	private float alpha = 0;
    private bool right;
    private bool left;
    private bool home;
    GameObject cliant;
    GameObject ball;
	GameObject hole;

	void Start(){
        ball = GameObject.Find("ball");
        cliant = GameObject.Find("TCPServerLancher");
        hole = GameObject.Find("hole");
		init = ball.GetComponent<Transform> ().position;
		holepos = hole.GetComponent<Transform> ().position;
		theta = Mathf.Atan ((holepos.x - init.x) / (holepos.z - init.z)) * Mathf.Rad2Deg;
		if (holepos.z > init.z) {
			theta += 180f;
		}
    }

	void Update ()
	{
		pos = ball.GetComponent<Transform> ().position;
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
        if (home) {
			alpha = 0f;
		}
		/*theta = angular.GetComponent<GameController> ().theta;
		alpha = angular.GetComponent<GameController> ().alpha;
		pos = angular.GetComponent<GameController> ().pos;*/

		GetComponent<Transform> ().position = pos + new Vector3 (12f * Mathf.Sin (Mathf.Deg2Rad * (theta + alpha)), 5f, 12f * Mathf.Cos (Mathf.Deg2Rad * (theta + alpha)));
		GetComponent<Transform> ().eulerAngles = new Vector3 (20f, 180f + theta + alpha, 0f);
	}



}