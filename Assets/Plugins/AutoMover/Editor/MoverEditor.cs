using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AutoMover))]
public class MoverEditor : Editor {

    private static GUIContent moveUpButtonContent = new GUIContent("\u25b2", "move up");
    private static GUIContent moveDownButtonContent = new GUIContent("\u25bc", "move down");
    private static GUIContent duplicateButtonContent = new GUIContent("+", "duplicate");
    private static GUIContent deleteButtonContent = new GUIContent("-", "delete");
    private AutoMover mover;
    private bool expandedNoise;
    private bool expandedCurve;
    private bool coreThingChanged;
    SerializedProperty waypointPos;
    SerializedProperty waypointRot;
    SerializedProperty length;
    SerializedProperty lpStyle;
    SerializedProperty ipStyle;
    SerializedProperty rlStyle;
    SerializedProperty wsStyle;
    SerializedProperty ntStyle;
    SerializedProperty rntStyle;
    SerializedProperty noiseAmplitude;
    SerializedProperty noiseFrequency;
    SerializedProperty noiseAmplitudeR;
    SerializedProperty noiseFrequencyR;
    SerializedProperty sineOffset;
    SerializedProperty sineOffsetR;
    SerializedProperty runOnStart;
    SerializedProperty delayMin;
    SerializedProperty delayMax;
    SerializedProperty stopAfter;
    SerializedProperty precomputePath;
    SerializedProperty drawGizmos;
    SerializedProperty moving;
    SerializedProperty curveExpanded;
    SerializedProperty noiseExpanded;

    //This is executed always when the object is selected
    void OnEnable()
    {
        waypointPos = serializedObject.FindProperty("pos");
        waypointRot = serializedObject.FindProperty("rot");
        length = serializedObject.FindProperty("length");
        lpStyle = serializedObject.FindProperty("loopingStyle");
        ipStyle = serializedObject.FindProperty("curveStyle");
        rlStyle = serializedObject.FindProperty("rotationMethod");
        wsStyle = serializedObject.FindProperty("anchorPointSpace");
        ntStyle = serializedObject.FindProperty("positionNoiseType");
        rntStyle = serializedObject.FindProperty("rotationNoiseType");
        noiseAmplitude = serializedObject.FindProperty("positionNoiseAmplitude");
        noiseFrequency = serializedObject.FindProperty("positionNoiseFrequency");
        noiseAmplitudeR = serializedObject.FindProperty("rotationNoiseAmplitude");
        noiseFrequencyR = serializedObject.FindProperty("rotationNoiseFrequency");
        sineOffset = serializedObject.FindProperty("positionSineOffset");
        sineOffsetR = serializedObject.FindProperty("rotationSineOffset");
        moving = serializedObject.FindProperty("moving");
        runOnStart = serializedObject.FindProperty("runOnStart");
        delayMin = serializedObject.FindProperty("delayMin");
        delayMax = serializedObject.FindProperty("delayMax");
        stopAfter = serializedObject.FindProperty("stopAfter");
        precomputePath = serializedObject.FindProperty("precomputePath");
        drawGizmos = serializedObject.FindProperty("drawGizmos");
        curveExpanded = serializedObject.FindProperty("curveExpanded");
        noiseExpanded = serializedObject.FindProperty("noiseExpanded");
        expandedNoise = noiseExpanded.boolValue;
        expandedCurve = curveExpanded.boolValue;
        mover = (AutoMover)target;
    }

    public override void OnInspectorGUI()
    {
        bool pointSpaceChanged = false;
        coreThingChanged = false;
        serializedObject.Update();

        //this might happen if the user copies the component when it is moving, and pastes it in edit mode
        if (!Application.isPlaying && moving.boolValue)
        {
            Vector3 origPosition = mover.transform.localPosition;
            Vector3 origRotation = mover.transform.localEulerAngles;
            mover.StopMoving(); //this might move it to 0,0,0
            mover.transform.localPosition = origPosition;
            mover.transform.localEulerAngles = origRotation;
        }

        if (GUILayout.Button(new GUIContent("Add anchor point", "Adds the current position and rotation at the end of the anchor point list.")))
        {
            AddWaypoint();
            coreThingChanged = true;
        }
        
        CustomListField(waypointPos, waypointRot);
        
        expandedCurve = EditorGUILayout.Foldout(expandedCurve, "Curve", true);

        curveExpanded.boolValue = expandedCurve;

        if (expandedCurve)
        {
            EditorGUI.indentLevel += 1;

            float oldValueLength = length.floatValue;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Length (seconds)", "Specifies the length of the curve in seconds. Minimum value of 0.001f."));
            EditorGUILayout.PropertyField(length, new GUIContent(""));
            EditorGUILayout.EndHorizontal();
            if (oldValueLength != length.floatValue)
                coreThingChanged = true;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Delay (seconds)", "Specifies the Delay between each loop in seconds. Actual delay is a random value between the Min and Max values."));
            GUILayout.Label(new GUIContent("    Min", "Minimum value for the delay."));
            EditorGUILayout.PropertyField(delayMin, GUIContent.none);
            GUILayout.Label(new GUIContent("Max", "Maximum value for the delay."));
            EditorGUILayout.PropertyField(delayMax, GUIContent.none);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Run on Start", "Specifies if the movement is started during Start(). If the value is false, the movement will have to be manually started using the StartMoving() method."));
            EditorGUILayout.PropertyField(runOnStart, new GUIContent(""));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Stop after (loops)", "Specifies how many loops the object moves before stopping. Value of 0 means that the object will move until the StopMoving() method is called."));
            EditorGUILayout.PropertyField(stopAfter, new GUIContent(""));
            EditorGUILayout.EndHorizontal();

            bool oldValuePreC = precomputePath.boolValue;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Precompute path", "Specifies if the path should be precomputed once at the start of the movement, rather than computing it for every loop. If set to true, changing any parameter will trigger a new precalculation, so it is recommended to be set to false if the parameters are modified often."));
            EditorGUILayout.PropertyField(precomputePath, new GUIContent(""));
            EditorGUILayout.EndHorizontal();
            if (oldValuePreC != precomputePath.boolValue)
                coreThingChanged = true;

            int oldValueRotM = rlStyle.intValue;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Rotation method", "Specifies the logic that is used to rotate the objects. 'Shortest path' will never rotate over 180 degrees to reach the target, ignoring any full rotations. 'Absolute value' will always rotate to the absolute specified value, even if it means multiple 360 degree spins."));
            rlStyle.intValue = EditorGUILayout.Popup(rlStyle.intValue, new string[] { "Shortest path", "Absolute value" });
            EditorGUILayout.EndHorizontal();
            if (oldValueRotM != rlStyle.intValue)
                coreThingChanged = true;

            int oldValueLoopS = lpStyle.intValue;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Looping style", "Specifies the style that is used to loop the path. 'Loop' will create a closed loop from the end to start, 'Repeat' will start from the beginning after reaching the end, and 'Bounce' will backtrack the same route after reaching the end."));
            lpStyle.intValue = EditorGUILayout.Popup(lpStyle.intValue, new string[] { "Loop", "Repeat", "Bounce" });
            EditorGUILayout.EndHorizontal();
            if (oldValueLoopS != lpStyle.intValue)
                coreThingChanged = true;

            int oldValueCurveS = ipStyle.intValue;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Curve style", "Specifies the style of the curve. 'Linear' is simply straight lines between the anchor points, 'Curve' forms a single Bezier curve using all of the anchor points, and 'Spline' forms multiple bezier curves that are joined with C2 continuity."));
            ipStyle.intValue = EditorGUILayout.Popup(ipStyle.intValue, new string[] { "Linear", "Curve", "Spline" });
            EditorGUILayout.EndHorizontal();
            if (oldValueCurveS != ipStyle.intValue)
                coreThingChanged = true;

            int oldValuePointS = wsStyle.intValue;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Anchor point space", "Specifies the space in which the anchor points are defined. 'World' means that the points are in world space, so moving the parent of the object does not move the anchor points. 'Local' means that the points are specified in local space, so moving the parent of the object will move the anchor points."));
            wsStyle.intValue = EditorGUILayout.Popup(wsStyle.intValue, new string[] { "World", "Local" });
            EditorGUILayout.EndHorizontal();
            if (oldValuePointS != wsStyle.intValue)
            {
                pointSpaceChanged = true;
                coreThingChanged = true;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Draw gizmos", "Specifies if the path should be visualized in the editor."));
            EditorGUILayout.PropertyField(drawGizmos, new GUIContent(""));
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel -= 1;
        } 
        
        expandedNoise = EditorGUILayout.Foldout(expandedNoise, "Noise", true);

        noiseExpanded.boolValue = expandedNoise;

        if (expandedNoise)
        {
            EditorGUI.indentLevel += 1;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Position Noise type", "Specifies the type of the noise that is applied to the position of the object during movement."));
            ntStyle.intValue = EditorGUILayout.Popup(ntStyle.intValue, new string[] { "Random", "Sine" });
            EditorGUILayout.EndHorizontal();

            if (ntStyle.intValue == 1)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(new GUIContent("Position sine offset", "Specifies the offset (delay) of the sine noise. Values equal with the remainder when divided by 2*pi."));
                EditorGUILayout.PropertyField(sineOffset, new GUIContent(""));
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Position noise amplitude", "Specifies the amplitude (absolute of the maximum/minimum value) of the noise."));
            EditorGUILayout.PropertyField(noiseAmplitude, new GUIContent(""));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Position noise frequency", "Specifies the frequency of the noise."));
            EditorGUILayout.PropertyField(noiseFrequency, new GUIContent(""));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Rotation Noise type", "Specifies the type of the noise that is applied to the rotation of the object during movement."));
            rntStyle.intValue = EditorGUILayout.Popup(rntStyle.intValue, new string[] { "Random", "Sine" });
            EditorGUILayout.EndHorizontal();

            if (rntStyle.intValue == 1)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(new GUIContent("Rotation sine offset", "Specifies the offset (delay) of the sine noise. Values equal with the remainder when divided by 2*pi."));
                EditorGUILayout.PropertyField(sineOffsetR, new GUIContent(""));
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Rotation noise amplitude", "Specifies the amplitude (absolute of the maximum/minimum value) of the noise in degrees."));
            EditorGUILayout.PropertyField(noiseAmplitudeR, new GUIContent(""));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(new GUIContent("Rotation noise frequency", "Specifies the frequency of the noise."));
            EditorGUILayout.PropertyField(noiseFrequencyR, new GUIContent(""));
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel -= 1;
        }

        //safety check for some properties
        if (length.floatValue < 0.001f)
            length.floatValue = 0.001f;
        if (delayMin.floatValue < 0f)
            delayMin.floatValue = 0f;
        if (delayMax.floatValue < delayMin.floatValue)
            delayMax.floatValue = delayMin.floatValue;
        if (noiseAmplitude.vector3Value.x < 0)
            noiseAmplitude.vector3Value = new Vector3(0, noiseAmplitude.vector3Value.y, noiseAmplitude.vector3Value.z);
        if (noiseAmplitude.vector3Value.y < 0)
            noiseAmplitude.vector3Value = new Vector3(noiseAmplitude.vector3Value.x, 0, noiseAmplitude.vector3Value.z);
        if (noiseAmplitude.vector3Value.z < 0)
            noiseAmplitude.vector3Value = new Vector3(noiseAmplitude.vector3Value.x, noiseAmplitude.vector3Value.y, 0);
        if (noiseFrequency.vector3Value.x < 0)
            noiseFrequency.vector3Value = new Vector3(0, noiseFrequency.vector3Value.y, noiseFrequency.vector3Value.z);
        if (noiseFrequency.vector3Value.y < 0)
            noiseFrequency.vector3Value = new Vector3(noiseFrequency.vector3Value.x, 0, noiseFrequency.vector3Value.z);
        if (noiseFrequency.vector3Value.z < 0)
            noiseFrequency.vector3Value = new Vector3(noiseFrequency.vector3Value.x, noiseFrequency.vector3Value.y, 0);
        if (noiseAmplitudeR.vector3Value.x < 0)
            noiseAmplitudeR.vector3Value = new Vector3(0, noiseAmplitudeR.vector3Value.y, noiseAmplitudeR.vector3Value.z);
        if (noiseAmplitudeR.vector3Value.y < 0)
            noiseAmplitudeR.vector3Value = new Vector3(noiseAmplitudeR.vector3Value.x, 0, noiseAmplitudeR.vector3Value.z);
        if (noiseAmplitudeR.vector3Value.z < 0)
            noiseAmplitudeR.vector3Value = new Vector3(noiseAmplitudeR.vector3Value.x, noiseAmplitudeR.vector3Value.y, 0);
        if (noiseFrequencyR.vector3Value.x < 0)
            noiseFrequencyR.vector3Value = new Vector3(0, noiseFrequencyR.vector3Value.y, noiseFrequencyR.vector3Value.z);
        if (noiseFrequencyR.vector3Value.y < 0)
            noiseFrequencyR.vector3Value = new Vector3(noiseFrequencyR.vector3Value.x, 0, noiseFrequencyR.vector3Value.z);
        if (noiseFrequencyR.vector3Value.z < 0)
            noiseFrequencyR.vector3Value = new Vector3(noiseFrequencyR.vector3Value.x, noiseFrequencyR.vector3Value.y, 0);
        
        bool moved = false;
        if (coreThingChanged && moving.boolValue)
        {
            moved = true;
            moving.boolValue = false;
            mover.StopMoving();
        } 
        if (pointSpaceChanged)
        {
            //Convert the anchor points to world/local coordinates, depending on the variable
            if (wsStyle.intValue == 0)
            {
                //from local to global
                for (int i = 0; i < waypointPos.arraySize; ++i)
                {
                    //if there is no parent, the points are the same
                    if (mover.transform.parent != null)
                    {
                        waypointPos.GetArrayElementAtIndex(i).vector3Value = mover.transform.parent.TransformPoint(waypointPos.GetArrayElementAtIndex(i).vector3Value);
                        waypointRot.GetArrayElementAtIndex(i).vector3Value = (mover.transform.parent.rotation * Quaternion.Euler(waypointRot.GetArrayElementAtIndex(i).vector3Value)).eulerAngles;
                    }
                }
            }
            else
            {
                //from global to local
                for (int i = 0; i < waypointPos.arraySize; ++i)
                {
                    if (mover.transform.parent != null)
                    {
                        waypointPos.GetArrayElementAtIndex(i).vector3Value = mover.transform.parent.InverseTransformPoint(waypointPos.GetArrayElementAtIndex(i).vector3Value);
                        waypointRot.GetArrayElementAtIndex(i).vector3Value = (Quaternion.Inverse(mover.transform.parent.rotation) * Quaternion.Euler(waypointRot.GetArrayElementAtIndex(i).vector3Value)).eulerAngles;
                    }
                }
            }
        }
         
        serializedObject.ApplyModifiedProperties();
        
        if (moved)
        {
            mover.StartMoving();
        }  
    }



    private void AddWaypoint()
    {
        waypointPos.InsertArrayElementAtIndex(waypointPos.arraySize);
        waypointRot.InsertArrayElementAtIndex(waypointRot.arraySize);
        waypointPos.GetArrayElementAtIndex(waypointPos.arraySize-1).vector3Value = wsStyle.intValue == 0? mover.gameObject.transform.position : new Vector3(0, 0, 0);
        waypointRot.GetArrayElementAtIndex(waypointRot.arraySize-1).vector3Value = mover.gameObject.transform.localRotation.eulerAngles; 
    }

    private void CustomListField(SerializedProperty waypoints, SerializedProperty rotations)
    {
        EditorGUILayout.PropertyField(waypoints, new GUIContent("Anchor points"));

        if (waypoints.isExpanded)
        {
            EditorGUI.indentLevel += 1;
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(waypoints.FindPropertyRelative("Array.size"));
            EditorGUI.EndDisabledGroup();

            DrawHorizontalLine();

            for (int i = 0; i < waypoints.arraySize; i++)
            {
                Vector3 posOldValue = waypoints.GetArrayElementAtIndex(i).vector3Value;
                Vector3 rotOldValue = rotations.GetArrayElementAtIndex(i).vector3Value;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("    Position");
                EditorGUILayout.PropertyField(waypoints.GetArrayElementAtIndex(i), GUIContent.none);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("    Rotation");
                EditorGUILayout.PropertyField(rotations.GetArrayElementAtIndex(i), GUIContent.none);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                if (posOldValue != waypoints.GetArrayElementAtIndex(i).vector3Value || rotOldValue != rotations.GetArrayElementAtIndex(i).vector3Value)
                    coreThingChanged = true;

                if (GUILayout.Button(moveUpButtonContent))
                {
                    if (i > 0)
                    {
                        waypoints.MoveArrayElement(i, i - 1);
                        rotations.MoveArrayElement(i, i - 1);
                        coreThingChanged = true;
                    }
                }
                if (GUILayout.Button(moveDownButtonContent))
                {
                    if (i < waypoints.arraySize - 1)
                    {
                        waypoints.MoveArrayElement(i, i + 1);
                        rotations.MoveArrayElement(i, i + 1);
                        coreThingChanged = true;
                    }
                }
                if (GUILayout.Button(duplicateButtonContent))
                {
                    waypoints.InsertArrayElementAtIndex(i);
                    waypoints.GetArrayElementAtIndex(i).vector3Value = waypoints.GetArrayElementAtIndex(i + 1).vector3Value;
                    rotations.InsertArrayElementAtIndex(i);
                    rotations.GetArrayElementAtIndex(i).vector3Value = rotations.GetArrayElementAtIndex(i + 1).vector3Value;
                    coreThingChanged = true;
                }
                if (GUILayout.Button(deleteButtonContent))
                {
                    int oldSize = waypoints.arraySize;
                    waypoints.DeleteArrayElementAtIndex(i);
                    if (waypoints.arraySize == oldSize)
                    {
                        waypoints.DeleteArrayElementAtIndex(i);
                    }
                    oldSize = rotations.arraySize;
                    rotations.DeleteArrayElementAtIndex(i);
                    if (rotations.arraySize == oldSize)
                    {
                        rotations.DeleteArrayElementAtIndex(i);
                    }
                    coreThingChanged = true;
                }
                EditorGUILayout.EndHorizontal();
                DrawHorizontalLine();
            }

            EditorGUI.indentLevel -= 1;

        }
    }

    private void DrawHorizontalLine()
    {
        Rect rect = EditorGUILayout.GetControlRect(false, 1);
        rect.height = 1;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }
}
