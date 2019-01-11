using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCotroller : MonoBehaviour {

	private static bool created = false;
	private Vector3 anglevelocity;
	private Vector3 velocity;
	private float vel;
    //public GameObject spark;
    private Vector3 pos;

	void FixedStart(){
		Rigidbody rigidbody = GetComponent<Rigidbody> ();
		rigidbody.maxAngularVelocity = 10000f;
        pos = GetComponent<Transform>().position;
	}
	void FixedUpdate(){
		Rigidbody rigidbody = GetComponent<Rigidbody> ();
		rigidbody.maxAngularVelocity = 10000f;
		if (pos.y > 7f) {
			anglevelocity = rigidbody.angularVelocity;
			velocity = rigidbody.velocity;
			vel = Mathf.Sqrt (velocity.x * velocity.x + velocity.z * velocity.z);
			rigidbody.AddForce (-anglevelocity.y / vel * velocity.z / 10f, 0f, anglevelocity.y / vel * velocity.x / 10f);
		}
        if (vel < 0.01f)
        {
            //rigidbody.Sleep();
        }
        /*if (!rigidbody.IsSleeping())
        {
            Instantiate(spark, pos, Quaternion.identity);
        }*/
	}

	void Awake () {

		if(!created){
			DontDestroyOnLoad (this.gameObject);
			created=true;
		}else{
			Destroy(this.gameObject);
		}

	}


}
