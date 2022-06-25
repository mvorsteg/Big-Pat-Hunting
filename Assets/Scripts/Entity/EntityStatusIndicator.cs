using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum EntityStatus
{
    Normal,
    Question,
    Exclaim,
    Dead
}

public class EntityStatusIndicator : MonoBehaviour
{
    private Camera mainCamera;
    [SerializeField]
    private Image backgroundIcon, fillIcon;
    
    [SerializeField]
    private Sprite questionIcon, exclaimIcon, deadIcon;

    [SerializeField]
    private Color questionColor1, questionColor2, exclaimColor, deadColor, backgroundColor;

    [SerializeField]
    private float shakeScale = 1f, shakeRiseTime = 0.04f, shakeFallTime = 0.1f;
    [SerializeField]
    private float fadeTime = 0.5f;
    private Vector3 baseScale;

    private EntityStatus status;
    private float statusLevel = 0f;

    public EntityStatus Status
    { 
        get => status; 
        set
        {
            // update value and UI icon 
            status = value;
            StopCoroutine("Fade");
            switch (status)
            {
                case (EntityStatus.Normal) :
                    StartCoroutine("Fade");
                    break;
                case (EntityStatus.Question) :
                    backgroundIcon.sprite = questionIcon;
                    backgroundIcon.color = backgroundColor;
                    backgroundIcon.enabled = true;
                    fillIcon.sprite = questionIcon;
                    fillIcon.enabled = true;
                    break;
                case (EntityStatus.Exclaim) :
                    StatusLevel = 1f;
                    backgroundIcon.sprite = exclaimIcon;
                    backgroundIcon.color = backgroundColor;
                    backgroundIcon.enabled = true;
                    fillIcon.sprite = exclaimIcon;
                    fillIcon.color = exclaimColor;
                    fillIcon.enabled = true;
                    StartCoroutine("Fade");
                    break;
                case (EntityStatus.Dead) :
                    StatusLevel = 1f;
                    backgroundIcon.sprite = deadIcon;
                    backgroundIcon.color = backgroundColor;
                    backgroundIcon.enabled = true;
                    fillIcon.sprite = deadIcon;
                    fillIcon.color = deadColor;
                    fillIcon.enabled = true;
                    break;
                default :
                    backgroundIcon.enabled = false;
                    fillIcon.enabled = false;
                    break;
            }
        } 
    }

    public float StatusLevel
    {
        get => statusLevel;
        set
        {
            statusLevel = value;
            if (Status == EntityStatus.Question)
            {
                fillIcon.fillAmount = value;
            }
            else
            {
                fillIcon.fillAmount = 1f;
            }
        }
    }

    private void Awake()
    {
        Status = EntityStatus.Normal;
        backgroundIcon.enabled = false;
        fillIcon.enabled = false;
        baseScale = fillIcon.transform.localScale;
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(mainCamera.transform);
        if (Status == EntityStatus.Question)
        {
            fillIcon.color = Color.Lerp(questionColor1, questionColor2, statusLevel);
        }
    }

    public void Animate()
    {
        StopCoroutine("Shake");
        StartCoroutine("Shake");
    }

    private IEnumerator Shake()
    {
        Vector3 peakScale = baseScale * shakeScale;
        // scale up
        float elapsedTime = 0;
        while (elapsedTime < shakeRiseTime)
        {
            elapsedTime += Time.deltaTime;
            fillIcon.transform.localScale = Vector3.Lerp(baseScale, peakScale, elapsedTime / shakeRiseTime);
            backgroundIcon.transform.localScale = Vector3.Lerp(baseScale, peakScale, elapsedTime / shakeRiseTime);
            yield return null;
        }
        elapsedTime = 0;
        while (elapsedTime < shakeFallTime)
        {
            elapsedTime += Time.deltaTime;
            fillIcon.transform.localScale = Vector3.Lerp(peakScale, baseScale, elapsedTime / shakeRiseTime);
            backgroundIcon.transform.localScale = Vector3.Lerp(peakScale, baseScale, elapsedTime / shakeRiseTime);
            yield return null;
        }
        fillIcon.transform.localScale = baseScale;
        backgroundIcon.transform.localScale = baseScale;
    }

    private IEnumerator Fade()
    {
        float elapsedTime = 0;
        Color fillColorstart = fillIcon.color;
        Color fillColorTrans = new Color(fillIcon.color.r, fillIcon.color.g, fillIcon.color.b, 0);
        Color bgColorTrans = new Color(backgroundIcon.color.r, backgroundIcon.color.g, backgroundIcon.color.b, 0);
        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.deltaTime;
            fillIcon.color = Color.Lerp(fillColorstart, fillColorTrans, elapsedTime / fadeTime);
            backgroundIcon.color = Color.Lerp(backgroundColor, bgColorTrans, elapsedTime / fadeTime);
            yield return null;
        }
        backgroundIcon.enabled = false;
        fillIcon.enabled = false;
    }
}