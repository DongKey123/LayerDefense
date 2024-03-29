using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(CustomUtils.ScreenShot))]
public class ScreenShotEditor : Editor 
{
    CustomUtils.ScreenShot screenShot;
	void OnEnable() => screenShot = target as CustomUtils.ScreenShot;

	public override void OnInspectorGUI()
	{
        base.OnInspectorGUI();
        if (GUILayout.Button("ScreenShot"))
        {
            screenShot.ScreenShotClick();
            EditorApplication.ExecuteMenuItem("Assets/Refresh");
        } 
	}
}
#endif

namespace CustomUtils
{
    public class ScreenShot : MonoBehaviour
    {
        [SerializeField] string folderPath = string.Empty;
        [SerializeField] string screenShotName;

        public void ScreenShotClick()
        {
            RenderTexture renderTexture = GetComponent<Camera>().targetTexture;
            Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            RenderTexture.active = renderTexture;
            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture.Apply();

            File.WriteAllBytes($"{folderPath}/{screenShotName}.png", texture.EncodeToPNG());
        }
    }
}