using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroller : MonoBehaviour {

    AutoMover am;
    int phase;
    float[] stopHeights;
    float resumeTime;

	// Use this for initialization
	void Start () {
        am = GetComponent<AutoMover>(); //getting the automover component
        phase = 0; //starting at the 1st of the 10 examples
        stopHeights = new float[10] { -8.85f, 1.05f, 13.4f, 25f, 35.7f, 45.3f, 56.45f, 70.4f, 87f, 104f }; //heights where to stop the mover for a while
        resumeTime = 0; //variable to store the time when the mover can be resumed
        am.StartMoving(); //starting the movement
	}
	
	// Update is called once per frame
	void Update () {
        //if the mover is not moving and it is time to stop for a while, stop it 
        if (!am.IsPaused)
        {
            if (am.transform.position.y > stopHeights[phase])
            {
                resumeTime = Time.time + 10; //stopping for 10 seconds
                am.Pause();
            }
        }
        //resuming the mover if the 8 seconds have passed
        if (am.IsPaused && Time.time >= resumeTime)
        {
            am.Resume();
            phase++;

            //starting from start if we reached the last showcase
            if (phase > 9)
            {
                am.StopMoving();
                am.StartMoving();
                phase = 0;
            }
        } 
	}
}
