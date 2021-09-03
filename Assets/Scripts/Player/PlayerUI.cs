using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerUI : MonoBehaviour
{
    public GameObject hud;
    public GameObject bulletPrefab;
    public GameObject bulletParent;

    public Image hitMarker;
    public float hitMarkerDuration = 1f;
    public float hitMarkerFadeTime = 0.5f;
    private bool hitMarkerCoroutineRunning = false;
    private float hitMarkerElapsedTime = 0f;

    public GameObject deathScreen;
    public TextMeshProUGUI deathtext;

    public Color disableColor;

    private Image[] bullets;
    private int bulletIdx;

    private void Awake()
    {
        hitMarker.color = new Color(hitMarker.color.r, hitMarker.color.g, hitMarker.color.b, 0);
        SetDeathScreen(false, "");
    }

    public void AddBullet(int count)
    {
        bullets = new Image[count];
        for (int i = 0; i < count; i++)
        {
            bullets[i] = Instantiate(bulletPrefab, bulletParent.transform).GetComponent<Image>();
        }
        bulletIdx = count - 1;
    }    

    public void DisableBullet()
    {
        bullets[bulletIdx--].color = disableColor;
    }

    public void Reload()
    {
        foreach (Image i in bullets)
        {
            i.color = Color.white;
        }
        bulletIdx = bullets.Length - 1;
    }

    /// <summary>
    /// enables a hitmarker around the reticle / center of screen
    /// if the first marker is not yet done fading, resets its color and refreshes its time to fade
    /// </summary>
    public void HitMarker()
    {
        hitMarker.color = new Color(hitMarker.color.r, hitMarker.color.g, hitMarker.color.b, 1.0f);
        hitMarkerElapsedTime = 0f;
        if (!hitMarkerCoroutineRunning)
        {
            StartCoroutine(HitMarkerFadeCoroutine());
        }
    }

    /// <summary>
    /// interruptible coroutine for HitMarker()
    /// </summary>
    /// <returns></returns>
    private IEnumerator HitMarkerFadeCoroutine()
    {
        hitMarkerCoroutineRunning = true;
        while (hitMarkerElapsedTime < hitMarkerDuration + hitMarkerFadeTime)
        {
            hitMarkerElapsedTime += Time.deltaTime;
            hitMarker.color = new Color(hitMarker.color.r, hitMarker.color.g, hitMarker.color.b, hitMarkerDuration + 1 - (hitMarkerElapsedTime / hitMarkerFadeTime));
            yield return null;
        }
        hitMarkerCoroutineRunning = false;
    }

    public void SetDeathScreen(bool enabled, string source)
    {
        deathScreen.SetActive(enabled);
        deathtext.text = "Killed by " + source;
        hud.SetActive(!enabled);
    }
}