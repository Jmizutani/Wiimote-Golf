using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W5Controller : MonoBehaviour
{
    private float theta;
    private float alpha = 0;
    private float speed;
    private float temp_speed = 0f;
    private float angular_y = 0;
    //private float temp_angular = 0f;
    private float offset_yaw = 7920f;
    private float roll;
    private float offset_roll = 8850f;
    private float angle_roll = 0f;
    private float old_speed_roll = 0f;
    private float new_speed_roll = 0f;
    GameObject game;
    GameObject cliant;
    GameObject golfer;
    GameObject ball;
    private bool flag = false;
    protected Animator animator;

    void Start()
    {
        game = GameObject.Find("GameController");
        cliant = GameObject.Find("TCPServerLancher");
        ball = GameObject.Find("ball");
        golfer = GameObject.Find("golfer_W5");
        animator = golfer.GetComponent<Animator>();
    }

    void Update()
    {

        theta = game.GetComponent<GameController>().theta;
        alpha = game.GetComponent<GameController>().alpha;
        temp_speed = (cliant.GetComponent<WiimoteInfoDisplayBase>().balanceBoardData.yaw - offset_yaw) / 750f;
        if (speed < temp_speed)
        {
            speed = temp_speed;
        }
        //speed = cliant.GetComponent<WiimoteInfoDisplayBase>().balanceBoardData.xacc * 2;
        /*new_speed_roll = (cliant.GetComponent<WiimoteInfoDisplayBase>().balanceBoardData.roll - offset_roll) * 60;
        roll = (new_speed_roll + old_speed_roll) * Time.deltaTime / 2 / 1000;
        angle_roll += roll;
        old_speed_roll = new_speed_roll;
        if (cliant.GetComponent<WiimoteInfoDisplayBase>().balanceBoardData.a)
        {
            angle_roll = 0f;
        }*/
        //Debug.Log(angle_roll);
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime > 0.9f)
        {
            if (flag == false)
            {
                ball.GetComponent<Rigidbody>().AddForce(-4.5f * speed * Mathf.Sin(Mathf.Deg2Rad * (theta + alpha)), 2.25f * speed, -4.5f * speed * Mathf.Cos(Mathf.Deg2Rad * (theta + alpha)), ForceMode.VelocityChange);
                //ball.GetComponent<Rigidbody>().AddTorque(0f, angle_roll, 0f, ForceMode.VelocityChange);
                flag = true;
            }
        }
    }

    /*

    void OnTriggerEnter(Collider hit)
    {
        if (hit.CompareTag("ball"))
        {
            if (flag == false)
            {
                hit.gameObject.GetComponent<Rigidbody>().AddForce(-4.5f * speed * Mathf.Sin(Mathf.Deg2Rad * (theta + alpha)), 2.25f * speed, -4.5f * speed * Mathf.Cos(Mathf.Deg2Rad * (theta + alpha)), ForceMode.VelocityChange);
                hit.gameObject.GetComponent<Rigidbody>().AddTorque(0f, angular_y, 0f, ForceMode.VelocityChange);
                flag = true;
            }
        }
    }
    void OnTriggerStay(Collider hit)
    {
        if (hit.CompareTag("ball"))
        {
            if (flag == false)
            {
                hit.gameObject.GetComponent<Rigidbody>().AddForce(-4.5f * speed * Mathf.Sin(Mathf.Deg2Rad * (theta + alpha)), 2.25f * speed, -4.5f * speed * Mathf.Cos(Mathf.Deg2Rad * (theta + alpha)), ForceMode.VelocityChange);
                hit.gameObject.GetComponent<Rigidbody>().AddTorque(0f, angular_y, 0f, ForceMode.VelocityChange);
                flag = true;
            }
        }
    }
    void OnTriggerExit(Collider hit)
    {
        if (hit.CompareTag("ball"))
        {
            if (flag == false)
            {
                hit.gameObject.GetComponent<Rigidbody>().AddForce(-4.5f * speed * Mathf.Sin(Mathf.Deg2Rad * (theta + alpha)), 2.25f * speed, -4.5f * speed * Mathf.Cos(Mathf.Deg2Rad * (theta + alpha)), ForceMode.VelocityChange);
                hit.gameObject.GetComponent<Rigidbody>().AddTorque(0f, angular_y, 0f, ForceMode.VelocityChange);
                flag = true;
            }
        }
    }*/
}
