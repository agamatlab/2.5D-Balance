﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Framerate_Limit : MonoBehaviour {


	
	void Awake () {
		Application.targetFrameRate = 30;
		
	}
}
