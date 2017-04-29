using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintMap : MonoBehaviour {
	private Texture2D targetTexture;
	// Use this for initialization
	void Start()
	{
		
	}

	// Update is called once per frame
	public void DrawPaintMap(RenderTexture tex)
	{
		// 画像データ読み込み
		targetTexture = new Texture2D(tex.width, tex.height, TextureFormat.ARGB32, false);
		RenderTexture.active = tex;
		targetTexture.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
		targetTexture.Apply();

		//GetComponent<Renderer>().material.mainTexture = targetTexture;		
		RawImage ri = GetComponent<RawImage>();
		ri.texture = targetTexture;
	}
}
