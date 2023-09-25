using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class color : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ChangeActiveColor()
    {
        Color customColor = new Color(85f / 255f, 167f / 255f, 246f / 255f);
        // 获取SkinnedMeshRenderer组件
        SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        // 确保获取到了组件
        if (skinnedMeshRenderer)
        {
            // 遍历所有材质并查找名为"Color Top"的属性
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
        // 获取SkinnedMeshRenderer组件
        SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();

        // 确保获取到了组件
        if (skinnedMeshRenderer)
        {
            // 遍历所有材质并查找名为"Color Top"的属性
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