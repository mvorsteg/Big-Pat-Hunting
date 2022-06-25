using EasyRoads3Dv3;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(Truck))]
public class TruckEditor : Editor
{
    private Truck truck;
    private SerializedObject truckSO;
    private ReorderableList waypointList;
    private ReorderableList wheelList;

    private GameObject roadObj;
    private ERRoad road;

    private void OnEnable()
    {
        truck = (Truck)target;
        truckSO = new SerializedObject(truck);

        wheelList = new ReorderableList(truckSO, truckSO.FindProperty("wheels"), false, true, true, true);
        wheelList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "Waypoints");
        wheelList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            rect.y += 2; //top padding
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, wheelList.serializedProperty.GetArrayElementAtIndex(index));
        };

        waypointList = new ReorderableList(truckSO, truckSO.FindProperty("waypoints"), true, true, true, true);
        waypointList.drawHeaderCallback = (rect) => EditorGUI.LabelField(rect, "Waypoints");
        waypointList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            rect.y += 2; //top padding
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(rect, waypointList.serializedProperty.GetArrayElementAtIndex(index));
        };
    }

    public override void OnInspectorGUI()
    {
        truck.movementSpeed = EditorGUILayout.FloatField("Movement Speed", truck.movementSpeed);
        truck.rotationSpeed = EditorGUILayout.FloatField("Rotation Speed", truck.rotationSpeed);
        truck.stoppingDistance = EditorGUILayout.FloatField("Stopping Distance", truck.stoppingDistance);
        EditorGUILayout.Space();
        truck.frontGroundCheck = (Transform)EditorGUILayout.ObjectField("Front Ground Check", truck.frontGroundCheck, typeof(Transform), true);
        truck.backGroundCheck = (Transform)EditorGUILayout.ObjectField("Back Ground Check", truck.backGroundCheck, typeof(Transform), true);
        EditorGUILayout.Space();
        truck.engineClip = (AudioClip)EditorGUILayout.ObjectField("Engine Clip", truck.engineClip, typeof(AudioClip), true);
        truck.hornClip = (AudioClip)EditorGUILayout.ObjectField("Horn Clip", truck.hornClip, typeof(AudioClip), true);
        truck.hitClip = (AudioClip)EditorGUILayout.ObjectField("Hit Clip", truck.hitClip, typeof(AudioClip), true);
        EditorGUILayout.Space();
        roadObj = (GameObject)EditorGUILayout.ObjectField("Road", roadObj, typeof(GameObject), true);
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Load Waypoints from road"))
        {
            //Undo.RecordObject(truck.waypointQueue, "waypointQueue");
            ERRoadNetwork roadNet = new ERRoadNetwork();
            road = roadNet.GetRoadByGameObject(roadObj);
            truck.waypoints = new List<Vector3>();
            foreach (Vector3 v in road.GetMarkerPositions())
            {
                truck.waypoints.Add(v);
            }
        }
        if (GUILayout.Button("Reverse Queue"))
        {
            truck.waypoints.Reverse();
        }
        GUILayout.EndHorizontal();

        truckSO.Update();
        truck.wheelRotationSpeed = EditorGUILayout.FloatField("Wheel Rotation Speed (RPM)", truck.wheelRotationSpeed);
        wheelList.DoLayoutList();
        EditorGUILayout.Space();
        waypointList.DoLayoutList();
        truckSO.ApplyModifiedProperties();

    }
}