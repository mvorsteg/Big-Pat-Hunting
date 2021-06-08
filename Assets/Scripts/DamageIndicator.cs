using UnityEngine;
using System.Collections;
using System;

public class DamageIndicator : MonoBehaviour
{
    private const float maxTimer = 8f;
    private float timer = maxTimer;

    private CanvasGroup canvasGroup = null;
    protected CanvasGroup CanvasGroup 
    {
        get
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }
            return canvasGroup;
        }
    }

    private RectTransform rect = null;
    protected RectTransform Rect
    {
        get
        {
            if (rect == null)
            {
                rect = GetComponent<RectTransform>();
                if (rect == null)
                {
                    rect = gameObject.AddComponent<RectTransform>();
                }
            }
            return rect;
        }
    }

    public Transform Target { get; protected set; } = null;
    private Transform player = null;

    private IEnumerator countdown = null;
    private Action unRegister = null;

    private Quaternion tRot = Quaternion.identity;
    private Vector3 tPos = Vector3.zero;

    public void Register(Transform t, Transform p, Action unRegister)
    {
        this.Target = t;
        this.player = p;
        this.unRegister = unRegister;
        StartCoroutine(RotateToTarget());
        StartTimer();
    }

    private void StartTimer()
    {
        if (countdown != null)
        {
            StopCoroutine(countdown);
        }
        countdown = Countdown();
        StartCoroutine(countdown);
    }

    public void Restart()
    {
        timer = maxTimer;
        StartTimer();
    }

    private IEnumerator RotateToTarget()
    {
        while (enabled)
        {
            if (Target != null)
            {
                tPos = Target.position;
                tRot = Target.rotation;
            }

            Vector3 directon = player.position - Target.position;
            tRot = Quaternion.LookRotation(directon);
            tRot.z = -tRot.y;
            tRot.x = 0;
            tRot.y = 0;

            Vector3 northDirection = new Vector3(0, 0, player.eulerAngles.y);
            Rect.localRotation = tRot * Quaternion.Euler(northDirection);

            yield return null;
        }
    }

    private IEnumerator Countdown()
    {
        while (CanvasGroup.alpha  < 1f)
        {
            CanvasGroup.alpha += 4 * Time.deltaTime;
            yield return null;
        }
        while (timer > 0)
        {
            timer--;
            yield return new WaitForSeconds(1);
        }
        while (CanvasGroup.alpha > 0f)
        {
            CanvasGroup.alpha -= 2 * Time.deltaTime;
            yield return null;
        }
        unRegister();
        Destroy(gameObject);
    }

}