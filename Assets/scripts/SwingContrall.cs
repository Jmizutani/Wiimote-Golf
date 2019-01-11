using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


public class SwingContrall : MonoBehaviour {
	protected Animator animator;
	private float i_frame = 0f;
    private float yaw = 0f;
    private float angle_yaw = 0f;
    private float offset_yaw = 7920f;
    private float old_speed_yaw = 0f;
    private float new_speed_yaw = 0f;
    private bool flag = false;
    public AudioClip audioclip;
    AudioSource audiosource;
    GameObject cliant;
    static bool top = false;
    float time;

	// Use this for initialization
	void Start () {
        cliant = GameObject.Find("TCPServerLancher");
		animator = GetComponent <Animator>();
        audiosource = gameObject.GetComponent<AudioSource>();
        audiosource.clip = audioclip;
	}

	// Update is called once per frame
	void Update () {
        new_speed_yaw = (offset_yaw - cliant.GetComponent<WiimoteInfoDisplayBase>().balanceBoardData.yaw) * 60;
        //Debug.Log(new_speed_yaw);
        yaw = (new_speed_yaw + old_speed_yaw) * Time.deltaTime / 2 / 1000;
        angle_yaw += yaw;
        old_speed_yaw = new_speed_yaw;
        if (cliant.GetComponent<WiimoteInfoDisplayBase>().balanceBoardData.a)
        {
            angle_yaw = 0f;
        }
        if (angle_yaw > 180f) { angle_yaw = 180f; }
        /*
		if (Input.GetKey ("space")) {
			i_frame += 0.2f;
		}
		if (Input.GetKey ("escape")) {
			i_frame -= 0.2f;
		}*/
        
		var clipInfoList = animator.GetCurrentAnimatorClipInfo (0);
		var clip = clipInfoList [0].clip;
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        var animationHash = stateInfo.shortNameHash;
        //time = i_frame / clip.frameRate;
        //animator.Play(animationHash, 0, time);
        time = angle_yaw / 180f * 19.2f;
        if(cliant.GetComponent<WiimoteInfoDisplayBase>().balanceBoardData.b)
        {
            //time = (19.2f + (180f - angle_yaw) / 180f * 2.4f) / clip.frameRate;
            time = 19.2f;
            flag = true;
            animator.Play(animationHash, 0, time/clip.frameRate);
        }

        if (flag == false)
        {
            animator.Play(animationHash, 0, time/clip.frameRate);
        }

        if (stateInfo.normalizedTime > 0.9f && stateInfo.normalizedTime < 0.91f)
        {
            audiosource.Play();
        }
        //Debug.Log(stateInfo.normalizedTime);

		/*if (Input.GetKey ("space")) {
			animator.SetBool ("is_swing", true);
		} else {
			animator.SetBool ("is_swing", false);
		}*/
	}
}
