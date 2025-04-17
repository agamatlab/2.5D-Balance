// Cam_Control : Description : Use to change distance between player and camera using triggers.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam_Control : MonoBehaviour {


	public 	GameObject 		Cam;
	private followPlayer 	cam ;
	private Vector3 		Cam_Offset;
	public 	float 			New_Offset_Y;
	public 	float 			New_Offset_Z;
	[Header ("Connect the GameObject associated with this GameObject")]
	public 	GameObject 		otherTrigger ;					// Connect the gameObject associated with this object.
	private Cam_Control 	_script;						// access component
	[Header ("Transition Duration")]
	public float 			param_Duration  		= .5f;	// the duration of the transition


	private bool 			b_timer = false;				// Those variabes are used to create the transition
	private float 			target_Distance_01_Z;
	private float 			target_Distance_02_Z;
	private float 			target_Distance_01_Y;
	private float 			target_Distance_02_Y;
	private float 			duration				= 1; 					
	private float 			t						= 0; 		

	// Use this for initialization
	void Start () {
		if(otherTrigger)
			_script =  otherTrigger.GetComponent<Cam_Control>();				// access Component
		if(Cam == null)
			Cam = GameObject.FindWithTag("MainCamera");
		cam = Cam.GetComponent<followPlayer>();						// access Component
	}
	
	// Update is called once per frame
	void FixedUpdate () {												// --> Update
		if(b_timer){													
			Cam_Offset.z = 													// Change fogStartDistance
				Mathf.Lerp(target_Distance_01_Z, 
					target_Distance_02_Z, t);

			Cam_Offset.y = 													// Change fogStartDistance
				Mathf.Lerp(target_Distance_01_Y, 
					target_Distance_02_Y, t);

			cam.DistanceZ(Cam_Offset.y,Cam_Offset.z);

			if (t < 1){
				t += Time.deltaTime/duration;
			}
			else{
				b_timer = false;
			}
		}
	}

	public void ChangeFogParam(float Offset_Y,float Offset_Z,float duration_){	//--> Mode == FogMode.ExponentialSquared || Mode == FogMode.Exponential
		target_Distance_01_Z 	= cam.Return_DistanceZ();
		target_Distance_02_Z	= Offset_Z;

		target_Distance_01_Y 	= cam.Return_DistanceY();
		target_Distance_02_Y	= Offset_Y;

		duration = duration_;
		t = 0;
		b_timer = true;
	}

	public void OnTriggerEnter(Collider other){									// --> When player enter the trigger
		if(other.tag == "Player" && otherTrigger){
			ChangeFogParam(New_Offset_Y,New_Offset_Z,param_Duration);
			_script.Stop();														// Stop the transition of otherTrigger if needed
		}
	}


	public void Stop(){b_timer = false;}											// --> Use to stop transition
}


