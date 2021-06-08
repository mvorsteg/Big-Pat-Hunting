using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuButton : MonoBehaviour, IMenuButton
{

    private TextMeshProUGUI tmp;
    private static TextMeshProUGUI selectedText;

    private bool selected = false;
    private bool clicked = false;

    // Start is called before the first frame update
    void Start()
    {
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
    }

    void OnDisable()
    {
        tmp.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0.125f);
        tmp.UpdateMeshPadding();
    }

    public void OnPointerEnter()
    {
        selected = true;
        if (!clicked)
        {
            tmp.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0.65f);
        }
        tmp.UpdateMeshPadding();
    }

    public void OnPointerExit()
    {
        selected = false;
        if (!clicked)
        {
            tmp.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0.125f);
        }
        tmp.UpdateMeshPadding();
    }

    public void OnPointerDown()
    {
        clicked = true;
        tmp.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 1.0f);
        tmp.UpdateMeshPadding();
    }

    public void OnPointerUp()
    {
        clicked = false;
        if (selected)
        {
            tmp.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0.65f);
        }
        else
        {
            tmp.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0.125f);
        }
        tmp.UpdateMeshPadding();
    }

    
}
