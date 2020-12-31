using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DamageSystem : MonoBehaviour
{
    public DamageIndicator indicatorPrefab = null;
    public RectTransform holder = null;
    public Camera cam;
    public Transform player;

    private Dictionary<Transform, DamageIndicator> indicators = new Dictionary<Transform, DamageIndicator>();

    public static Action<Transform> CreateIndicator = delegate { };
    public static Func<Transform, bool> CheckIfObjectInSight = null;

    private void OnEnable()
    {
        CreateIndicator += Create;
        CheckIfObjectInSight += InSight;
    }

    private void OnDisable()
    {
        CreateIndicator -= Create;
        CheckIfObjectInSight -= InSight;
    }

    void Create(Transform target)
    {
        if (indicators.ContainsKey(target))
        {
            indicators[target].Restart();
            return;
        }
        DamageIndicator newIndicator = Instantiate(indicatorPrefab, holder);
        newIndicator.Register(target, player, new Action( () => { indicators.Remove(target); }));

        indicators.Add(target, newIndicator);
    }

    bool InSight(Transform t)
    {
        Vector3 screenpoint = cam.WorldToViewportPoint(t.position);
        return screenpoint.z > 0 && screenpoint.x > 0 && screenpoint.x < 1 && screenpoint.y > 0 && screenpoint.y < 1;
    }
}