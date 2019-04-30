using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brightness : MonoBehaviour
{
    private Material m_Material;

    public Shader BrightnessShader;
    public FloatVariable BrightnessValue;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    Material Material
    {
        get
        {
            if (m_Material == null)
            {
                m_Material = new Material(BrightnessShader);
            }

            return m_Material;
        }
        set
        {
            m_Material = value;
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Material.SetFloat("_Brightness", BrightnessValue.Value);
        Graphics.Blit(source, destination, Material);
    }
}
