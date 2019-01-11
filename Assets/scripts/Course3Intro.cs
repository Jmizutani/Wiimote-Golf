using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Course3Intro : MonoBehaviour {

    private float count = 0;
    private float theta = 10f;
    private Vector3 pos = new Vector3(360f, 20f, 490f);

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (count > 240f && count < 560f)
        {
            theta += 0.5f;
        }
        if (count > 690f)
        {
            Invoke("Loadnext", 3);
        }
        else
        {
            pos += new Vector3(-Mathf.Sin(Mathf.Deg2Rad * theta), 0f, -Mathf.Cos(Mathf.Deg2Rad * theta));
            GetComponent<Transform>().position = pos;
            GetComponent<Transform>().eulerAngles = new Vector3(20f, 180f + theta, 0f);
        }
        count += 1f;
        
        //690
    }
    void Loadnext()
    {
        SceneManager.LoadScene("course3");
    }
}
