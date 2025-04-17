// Description : FollowPlayer.cs : Use this script on Camera. 
// The camera follow the player
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour {

	[Header ("Target to follow")	]
	public 	GameObject 				Target ;								// Connect the player
	[Header ("Smooth follow values")]	
	public float 					smoothTimeX 		= .1f;							
	public float 					smoothTimeY			= .4f;
	[Header ("X Y Z offset")	]
	public Vector3 					Camera_offset		= new Vector3(0,2.5f,-11); 


	// Update is called once per frame
	void FixedUpdate () {
		float newPosX = transform.localPosition.x;
		float newPosY = transform.localPosition.y;
		float newPosZ = transform.localPosition.z;

		newPosX = Mathf.Lerp (transform.localPosition.x, Target.transform.localPosition.x + Camera_offset.x, Time.deltaTime / smoothTimeX);
		newPosY = Mathf.Lerp (transform.localPosition.y, Target.transform.localPosition.y + Camera_offset.y, Time.deltaTime / smoothTimeY);
		newPosZ = Mathf.Lerp (transform.localPosition.z, Target.transform.localPosition.z + Camera_offset.z, Time.deltaTime / smoothTimeY);

		transform.localPosition = new Vector3 (newPosX, newPosY, newPosZ);
	}

	public float Return_DistanceZ(){
		return Camera_offset.z;
	}
	public float Return_DistanceY(){
		return Camera_offset.y;
	}

	public void DistanceZ(float value_Y,float value_Z){
		Camera_offset = new Vector3(Camera_offset.x,value_Y,value_Z);
	}
}



