using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPin : MonoBehaviour, IMenuButton
{
    [SerializeField]
    private float scaleTime = 0.1f;
    [SerializeField]
    private GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnPointerEnter()
    {
        StartCoroutine(SetScale(2f));
        panel.SetActive(true);
    }

    public void OnPointerExit()
    {
        StartCoroutine(SetScale(1f));
        panel.SetActive(false);

    }

    public void OnPointerDown()
    {

    }

    public void OnPointerUp()
    {

    }

    // public void SetScale(float amount)
    // {
    //     GetComponent<RectTransform>().localScale = new Vector3(amount, amount, 1);
    // }

    private IEnumerator SetScale(float scale)
    {
        Vector3 start = transform.localScale;
        Vector3 end = new Vector3(scale, scale, 1);
        float elapsedTime = 0f;
        while (elapsedTime < scaleTime)
        {
            elapsedTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(start, end, elapsedTime / scaleTime);
            yield return null;
        }
    }
}
