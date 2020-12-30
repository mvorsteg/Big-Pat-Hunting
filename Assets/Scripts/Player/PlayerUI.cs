using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public GameObject hud;
    public GameObject bulletPrefab;
    public GameObject bulletParent;

    public Color disableColor;

    private Image[] bullets;
    private int bulletIdx;

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
}