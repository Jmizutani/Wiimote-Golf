using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HoleController : MonoBehaviour {

	public GameObject ClearLabelObject;
    GameObject ball;
    GameObject controller;
    public AudioClip audioclip;
    AudioSource audiosource;
    GameObject Audio;

    void Start()
    {
        ball = GameObject.Find("ball");
        controller = GameObject.Find("GameController");
        audiosource = gameObject.GetComponent<AudioSource>();
        audiosource.clip = audioclip;
        Audio = GameObject.Find("Audio Source");
    }
	void OnTriggerEnter(Collider hit){
		if(hit.gameObject.CompareTag("ball")){
			ClearLabelObject.SetActive (true);
            audiosource.Play();
			Invoke ("Loadnext", 3);
		}
	}

	void Loadnext(){
        Destroy(Audio);
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        switch (sceneIndex)
        {
            case 2:
                GameController.stroke1 = GameController.stroke;
                break;
            case 4:
                GameController.stroke2 = GameController.stroke;
                break;
            default: break;
        }
        SceneManager.LoadScene(sceneIndex + 1);
        GameController.stroke = 0;
        ball.GetComponent<Transform>().position = new Vector3(330f, 0.03f, 460f);
	}
}
