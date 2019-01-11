using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameController : MonoBehaviour {
	public UnityEngine.UI.Text distance;
	public UnityEngine.UI.Text club;
	public UnityEngine.UI.Text HoleDistance;
	public UnityEngine.UI.Text strokes;
    public UnityEngine.UI.Text clearlabel;
	public GameObject ob;
	public GameObject W1;
	public GameObject W2;
	public GameObject W3;
	public GameObject W4;
	public GameObject W5;
	public GameObject I6;
	public GameObject I7;
	public GameObject I8;
	public GameObject I9;
	public GameObject PW;
	public GameObject SW;
	public GameObject Putter;
    public GameObject arrow;
	private int count = 1;
    private bool up;
    private bool down;
    private bool right;
    private bool left;
    private bool home;
    private bool up_flag = false;
    private bool down_flag = false;
	public float theta;
	private Vector3 init;
	private Vector3 holepos;
	private Vector3 pos;
	public float alpha = 0;
	private bool tmp1 = false;
	private bool tmp2 = false;
	public static int stroke = 0;
    public static int stroke1;
    public static int stroke2;
    private int per;
	static Vector3 previous;
	static Vector3 temp;
	GameObject ball;
	GameObject hole;
	GameObject golfer;
    GameObject ring;
    GameObject cliant;
	GameObject ResetBtn;
    GameObject audiosource;
    public GameObject clearlabelobject;
	void Start(){
        ball = GameObject.Find("ball");
        hole = GameObject.Find("hole");
        audiosource = GameObject.Find("Audio Source");
        ResetBtn = GameObject.Find("ResetBtn");
		init = ball.GetComponent<Transform> ().position;
		holepos = hole.GetComponent<Transform> ().position;
		theta = Mathf.Atan ((holepos.x - init.x) / (holepos.z - init.z)) * Mathf.Rad2Deg;
		if (holepos.z > init.z) {
			theta += 180f;
		}
		stroke += 1;
        cliant = GameObject.Find("TCPServerLancher");
        if (SceneManager.GetActiveScene().name == "course2")
        {
            per = 4;
        }
        if (SceneManager.GetActiveScene().name == "course3")
        {
            per = 5;
        }

    }
	public void Update () {
        golfer = GameObject.FindGameObjectWithTag("golfer");
        ring = GameObject.FindGameObjectWithTag("ring");
        if (ring!=null)
        {
            ring.GetComponent<Transform>().position = init;
        }
		pos = ball.GetComponent<Transform> ().position;
		float yard = Mathf.Sqrt ((pos.x - init.x) * (pos.x - init.x) + (pos.z - init.z) * (pos.z - init.z));
		distance.text = yard.ToString ("F1") + " yard";
		float remaining = Mathf.Sqrt ((pos.x - holepos.x) * (pos.x - holepos.x) + (pos.z - holepos.z) * (pos.z - holepos.z));
		HoleDistance.text = "残り" + remaining.ToString ("F1") + "yard";
		strokes.text = stroke.ToString() + " 打目";
        switch (stroke-per)
        {
            case -2:
                clearlabel.text = "イーグル";
                break;
            case -1:
                clearlabel.text = "バーディ";
                break;
            case 0:
                clearlabel.text = "パー";
                break;
            case 1:
                clearlabel.text = "ボギー";
                break;
            case 2:
                clearlabel.text = "ダブルボギー";
                break;
            case 3:
                clearlabel.text = "トリプルボギー";
                break;
            default:
                clearlabel.text = "+" + (stroke - per).ToString();
                break;
        }

        up = cliant.GetComponent<WiimoteInfoDisplayBase>().balanceBoardData.up;
        down = cliant.GetComponent<WiimoteInfoDisplayBase>().balanceBoardData.down;
        right = cliant.GetComponent<WiimoteInfoDisplayBase>().balanceBoardData.right;
        left = cliant.GetComponent<WiimoteInfoDisplayBase>().balanceBoardData.left;
        home = cliant.GetComponent<WiimoteInfoDisplayBase>().balanceBoardData.home;

        if (down) {
            if (!down_flag) {
                count += 1;
                down_flag = true;
            }
		}
        else { down_flag = false; }
		if (up) {
            if (!up_flag){
                count -= 1;
                up_flag = true;
            }
		}
        else { up_flag = false; }
		if (count == 0) {
			count = 12;
		}
		if (count == 13) {
			count = 1;
		}
		switch (count) {
		case 1:
			club.text = "1W\n(約230yard)";
			W1.SetActive (true);
			Putter.SetActive (false);
			W2.SetActive (false);
			break;
		case 2:
            club.text = "2W\n(約230yard)";
			W2.SetActive (true);
			W1.SetActive (false);
			W3.SetActive (false);
			break;
		case 3:
            club.text = "3W\n(約215yard)";
			W3.SetActive (true);
			W2.SetActive (false);
			W4.SetActive (false);
			break;
		case 4:
            club.text = "4W\n(約200yard)";
			W4.SetActive (true);
			W3.SetActive (false);
			W5.SetActive (false);
			break;
		case 5:
            club.text = "5W\n(約195yard)";
			W5.SetActive (true);
			W4.SetActive (false);
			I6.SetActive (false);
			break;
		case 6:
            club.text = "6I\n(約150yard)";
			I6.SetActive (true);
			W5.SetActive (false);
			I7.SetActive (false);
			break;
		case 7:
            club.text = "7I\n(約140yard)";
			I7.SetActive (true);
			I6.SetActive (false);
			I8.SetActive (false);
			break;
		case 8:
            club.text = "8I\n(約130yard)";
			I8.SetActive (true);
			I7.SetActive (false);
			I9.SetActive (false);
			break;
		case 9:
            club.text = "9I\n(約115yard)";
			I9.SetActive (true);
			I8.SetActive (false);
			PW.SetActive (false);
			break;
		case 10:
            club.text = "PW\n(約105yard)";
			PW.SetActive (true);
			I9.SetActive (false);
			SW.SetActive (false);
			break;
		case 11:
            club.text = "SW\n(約80yard)";
			SW.SetActive (true);
			PW.SetActive (false);
			Putter.SetActive (false);
			break;
		case 12:
			club.text = "Putter";
			Putter.SetActive (true);
			SW.SetActive (false);
			W1.SetActive (false);
			break;
		default:
			break;
		}
		if(right){
			alpha += 0.5f;
		}
		if (left) {
			alpha -= 0.5f;
		}
		if (home) {
			alpha = 0f;
		}
				
		golfer.GetComponent<Transform> ().position = init + new Vector3 (4.7f * Mathf.Cos (Mathf.Deg2Rad * (theta + alpha)) + 0.4f * Mathf.Sin (Mathf.Deg2Rad * (theta + alpha)), -0.1f, -4.7f * Mathf.Sin (Mathf.Deg2Rad * (theta + alpha)) + 0.4f * Mathf.Cos (Mathf.Deg2Rad * (theta + alpha)));
		golfer.GetComponent<Transform> ().eulerAngles = new Vector3 (0, 270f + theta + alpha, 0);

		if (ball.GetComponent<Rigidbody> ().IsSleeping ()) {
			tmp1 = true;
			if (tmp2 == true) {
                if (stroke >= per*3)
                {
                    clearlabel.text = "ギブアップ";
                    clearlabelobject.SetActive(true);
                    stroke = 0;
                    Invoke("Loadnext", 3);
                }
                else
                {
                    Ray ray = new Ray(ball.GetComponent<Transform>().position, new Vector3(0f, -1f, 0f));
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit, 10f))
                    {
                        if (hit.collider.name == "ob")
                        {
                            ob.SetActive(true);
                            Invoke("loadob", 3);
                        }
                        else
                        {
                            Invoke("Loadingnext", 3);
                        }
                    }
                    
                }
			} else {
				temp = ball.GetComponent<Transform> ().position;
			}
		} 
		else {
			if (tmp1 == true) {
				tmp2 = true;
                arrow.SetActive(false);
				previous = temp;
			}
		}

		if (ball.GetComponent<Transform> ().position.y < -1f) {
			ob.SetActive (true);
			Invoke ("loadob", 3);
		}

       


	}

	void Loadingnext(){
		
		int sceneIndex = SceneManager.GetActiveScene ().buildIndex;
		SceneManager.LoadScene (sceneIndex);
	}

	void loadob(){
		ball.GetComponent<Rigidbody> ().velocity = Vector3.zero;
		ball.GetComponent<Transform> ().position = previous;
		int sceneIndex = SceneManager.GetActiveScene ().buildIndex;
		SceneManager.LoadScene (sceneIndex);
		stroke += 1;
	}

	public void GameResetButton() {
		ResetBtn.SetActive(false);
		ball.GetComponent<Transform> ().position = previous;
		int sceneIndex = SceneManager.GetActiveScene ().buildIndex;
		SceneManager.LoadScene (sceneIndex);
		stroke -= 2;
	}

    void Loadnext()
    {
        Destroy(audiosource);
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        switch (sceneIndex)
        {
            case 2:stroke1 = stroke;
                break;
            case 4:stroke2 = stroke;
                break;
            default:break;
        }
       
        SceneManager.LoadScene(sceneIndex + 1);
       
        stroke = 0;
        ball.GetComponent<Transform>().position = new Vector3(330f, 0.03f, 460f);
        
    }
    public static int GetStroke()
    {
        return stroke;
    }
}
