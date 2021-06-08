using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

public class DamageSystem : MonoBehaviour
{
    public DamageIndicator indicatorPrefab = null;
    public RectTransform holder = null;
    public Camera cam;
    public Transform player;
    public Image vignette;

    private Dictionary<Transform, DamageIndicator> indicators = new Dictionary<Transform, DamageIndicator>();

    public static Action<Transform> CreateIndicator = delegate { };
    public static Func<Transform, bool> CheckIfObjectInSight = null;
    public static Action<float> SetVignette = delegate { };
    
    private void Start()
    {
        Vignette(0f);   
    }

    private void OnEnable()
    {
        CreateIndicator += Create;
        CheckIfObjectInSight += InSight;
        SetVignette += Vignette;
    }

    private void OnDisable()
    {
        CreateIndicator -= Create;
        CheckIfObjectInSight -= InSight;
    }

    public void Vignette(float amount)
    {
        vignette.color = new Color(vignette.color.r, vignette.color.g, vignette.color.b, amount);
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