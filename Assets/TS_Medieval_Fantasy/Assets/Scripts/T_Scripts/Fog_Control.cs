
// Fog_Control : Description : Use to change fog using triggers.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog_Control : MonoBehaviour {

	[Header ("Connect the GameObject associated with this GameObject")]
	public GameObject 				otherTrigger;					// Connect the gameObject associated with this object.
	private Fog_Control  			_script;						// access component
	[Header ("Transition Duration")]
	public float 					param_Duration = .05f;			// the duration of the transition
	[Header ("Color")	]
	public Color 					param_Color= new Color(0,.5f,1,0); // The color you want 
	[Header ("Linear")]
	public float 					param_Start = 20;				// The start fog distance you want]
	public float 					param_End = 150;				// The end fog distance you want

	private bool 					b_timer = false;				// Those variabes are used to create the transition
	private Color 					startColor;
	private Color 					endColor;
	private float 					target_fogStartDistance_01;
	private float 					target_fogStartDistance_02;
	private float 					target_fogEndDistance_01;
	private float 					target_fogEndDistance_02;
	private float 					duration = 1; 					
	private float 					t	 = 0; 		

	// Use this for initialization
	void Start () {														// --> Start : Initialisation
		_script =  otherTrigger.GetComponent<Fog_Control>();				// access Component
	}
	
	// Update is called once per frame
	void Update () {													// --> Update
		if(b_timer){													
			RenderSettings.fogColor = Color.Lerp(startColor, endColor, t);	// Change fogColor
			RenderSettings.fogStartDistance = 								// Change fogStartDistance
				Mathf.Lerp(target_fogStartDistance_01, 
					target_fogStartDistance_02, t);
			RenderSettings.fogEndDistance = 								// Change fogEndDistance
				Mathf.Lerp(target_fogEndDistance_01, 
					target_fogEndDistance_02, t);
			if (t < 1){
				t += Time.deltaTime/duration;
			}
			else{
				b_timer = false;
			}
		}
	}

	public void  ChangeFogParam(float Fog_Start,float Fog_End,Color FogColor_End, float duration_ ){	//--> Mode == FogMode.ExponentialSquared || Mode == FogMode.Exponential
		target_fogStartDistance_01 	= RenderSettings.fogStartDistance;
		target_fogStartDistance_02	= Fog_Start;
		target_fogEndDistance_01 	= RenderSettings.fogEndDistance;
		target_fogEndDistance_02 	= Fog_End;
		startColor = RenderSettings.fogColor;
		endColor = FogColor_End;
		duration = duration_;
		t = 0;
		b_timer = true;
	}

	void  OnTriggerEnter(Collider other){																// --> When player enter the trigger
		if(other.tag == "Player"){
			ChangeFogParam(param_Start,param_End,param_Color,param_Duration);								// Call ChangeFogParam
			_script.Stop();																					// Stop the transition of otherTrigger if needed
		}
	}


	public void  Stop(){b_timer = false;}																// --> Use to stop transition
}

