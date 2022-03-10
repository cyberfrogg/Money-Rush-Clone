using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptDefinedAutomover : MonoBehaviour {

    AutoMover mover;
    AutoMover childMover;

	// Use this for initialization
	void Start ()
    {
        //Find the child object
        GameObject child = null;
        Transform[] children = GetComponentsInChildren<Transform>();
        for (int i = 0; i< children.Length; ++i)
        {
            if (children[i].name == "ScriptChild")
            {
                child = children[i].gameObject;
                break;
            }
        }
        
        //Create components and disable moving immediately
        mover = gameObject.AddComponent<AutoMover>();
        mover.RunOnStart = false;
        childMover = child.AddComponent<AutoMover>();
        childMover.RunOnStart = false;

        //Add anchor points to the current positions
        mover.AddAnchorPoint();
        childMover.AddAnchorPoint();

        //Add some others a bit next to them
        mover.AddAnchorPoint(transform.position + new Vector3(0, 1, 0), new Vector3(0, 0, 90));
        childMover.AddAnchorPoint(childMover.transform.position + new Vector3(0, -1, 0), new Vector3(0, 0, -90));
        mover.InsertAnchorPoint(1, transform.position + new Vector3(1, 0, 0), new Vector3(0, 0, 180));
        childMover.DuplicateAnchorPoint(1);
        childMover.SetAnchorPoint(1, childMover.transform.position + new Vector3(-1, 0, 0), new Vector3(0, 0, -180));

        //Have the anchor points be defined in local space
        mover.AnchorPointSpace = AutoMoverAnchorPointSpace.local;
        childMover.AnchorPointSpace = AutoMoverAnchorPointSpace.local;

        //Change some of the properties
        mover.Length = 2.5f;
        mover.LoopingStyle = AutoMoverLoopingStyle.bounce;
        childMover.LoopingStyle = AutoMoverLoopingStyle.loop;
        mover.CurveStyle = AutoMoverCurve.Linear; 
        childMover.CurveStyle = AutoMoverCurve.Linear;
        mover.PositionNoiseAmplitude = new Vector3(0.2f, 0.2f, 0.2f);
        mover.PositionNoiseType = AutoMoverNoiseType.sine;

        //Start moving the objects
        mover.StartMoving();
        childMover.StartMoving();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
