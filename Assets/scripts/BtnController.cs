using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnController : MonoBehaviour {

	public GameObject GameStartBtn;
    public AudioClip audioclip;
    AudioSource audiosource;

    void Start()
    {
        audiosource = gameObject.GetComponent<AudioSource>();
        audiosource.clip = audioclip;
    }
    public void GameStartButton() {
		//GameStartBtn.SetActive(false);
        audiosource.Play();
        Invoke("loadcourse", 3);
	}

    void loadcourse()
    {
        SceneManager.LoadScene("course2_intro");
    }
}
