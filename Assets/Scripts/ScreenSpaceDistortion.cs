using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpaceDistortion : MonoBehaviour 
{
	Material material;
	Camera mainCamera;
	RenderTexture renderTexture;

	void Awake()
	{
		mainCamera = GetComponent<Camera>();
		renderTexture = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.Default);
		mainCamera.SetTargetBuffers(renderTexture.colorBuffer, renderTexture.depthBuffer);
		material = new Material(Shader.Find("ScreenSpace/ScreenSpaceDistortion"));
	}

	void OnPostRender()
	{
		Graphics.Blit(renderTexture, (RenderTexture)null, material);
	}
}