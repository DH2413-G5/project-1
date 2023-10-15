using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeVRHandColor : MonoBehaviour
{
    public void ChangeActiveColor()
    {
        Color customColor = new Color(85f / 255f, 167f / 255f, 246f / 255f);
        // Get the SkinnedMeshRenderer component
        SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        // Make sure get the component
        if (skinnedMeshRenderer)
        {
            // Go through all the materials and look for the property named "Color Top"
            foreach (Material mat in skinnedMeshRenderer.materials)
            {
                if (mat.HasProperty("_ColorTop"))
                {
                    mat.SetColor("_ColorTop", customColor);
                }
            }
        }
    }
    public void ChangeNormalColor()
    {
        Color customColor = new Color(50f / 255f, 52f / 255f, 54f / 255f);
        // Get the SkinnedMeshRenderer component
        SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        // Make sure get the component
        if (skinnedMeshRenderer)
        {
            // Go through all the materials and look for the property named "Color Top"
            foreach (Material mat in skinnedMeshRenderer.materials)
            {
                if (mat.HasProperty("_ColorTop"))
                {
                    mat.SetColor("_ColorTop", customColor);
                }
            }
        }
    }
}
