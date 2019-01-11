using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Course2Intro : MonoBehaviour {

    private float count = 0;
    private float theta = 20f;
    private Vector3 pos = new Vector3(40f, 20f, 380f);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (count>190f&&count < 370f)
        {
            theta += 0.3f;
        }
        if (count > 470f)
        {
            Invoke("Loadnext", 3);
        }
        else
        {
            pos += new Vector3(Mathf.Sin(Mathf.Deg2Rad * theta), 0f, -Mathf.Cos(Mathf.Deg2Rad * theta));
            GetComponent<Transform>().position = pos;
            GetComponent<Transform>().eulerAngles = new Vector3(20f, 180f - theta, 0f);
        }
        count += 1f;
        
        //470
	}
    void Loadnext()
    {
        SceneManager.LoadScene("course2");
    }
}
