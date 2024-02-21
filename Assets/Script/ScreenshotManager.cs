using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScreenshotManager : MonoBehaviour
{
    public string gameName = "Portal Strike";
    public GameObject capturePanel;
    public GameObject showImagePanel;
    public GameObject savedImagePanel;
    public RawImage showImg;

    // Another UI elements that you want to hide
    [Header("Another UI elements")]
    public GameObject controller;
    public GameObject playerStatus;
    public GameObject minimap;
    public GameObject portalControl;
    public GameObject scoreUI;

    private byte[] currentTexture;
    private string currentFilePath;

    public string ScreenShotName()
    {
        // Generate screenshot name
        return string.Format("{0}_{1}.png", gameName, System.DateTime.Now.ToString("yyyy-mm-dd-hh-mm-ss"));
        /* File will save in C:\Users\{Desktop.Name}\AppData\LocalLow\{UnityProject.CompanyName}\{GameName}
         * For example: C:\Users\PC\AppData\LocalLow\DefaultCompany\Portal Strike */
    }
    public void Capture()
    {
        StartCoroutine(TakeScreenshot());
    }

    private IEnumerator TakeScreenshot()
    {
        // Pause your game and hide UI when capture screen
        Time.timeScale = 0;
        CaptureHideUI();

        yield return new WaitForEndOfFrame(); // Wait until this method is end
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false); // Screenshot type: RGB24
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0); // Read pixels of your screen size
        screenshot.Apply();

        currentFilePath = Path.Combine(Application.temporaryCachePath, "temp_img.png");
        currentTexture = screenshot.EncodeToPNG();
        File.WriteAllBytes(currentFilePath, currentTexture);
        
        ShowImage();

        // To avoid memory leaks
        Destroy(screenshot);
    }

    private void ShowImage()
    {
        Texture2D tex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        tex.LoadImage(currentTexture);
        showImg.material.mainTexture = tex;
        showImagePanel.SetActive(true);
    }

    public void SaveToGallery()
    {
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(currentFilePath, gameName, ScreenShotName(),
            (success, path) =>
            {
                Debug.Log("Media save result: " + success + " " + path);
                if (success)
                {
                    savedImagePanel.SetActive(true);
#if UNITY_EDITOR
                    string editorFilePath = Path.Combine(Application.persistentDataPath, ScreenShotName());
                    File.WriteAllBytes(editorFilePath, currentTexture);
#endif
                }
            });
        Debug.Log("Permission Result: " + permission);
    }

    public void ShareImage()
    {
        new NativeShare().AddFile(currentFilePath)
            .SetSubject("Share screenshot from Portal Strike")
            .SetText("Hello World")
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget)).Share();
    }

    #region -Hide and Show UI-
    public void EnterCameraMode()
    {
        // 1. In Unity's Inspector, "CameraMode (Button)" SetActive = false .
        // 2.1 Show "capturePanel" when entering CameraMode
        capturePanel.SetActive(true);
        // 2.2 Hide all UI except "Controller" and "capturePanel" when you enter CameraMode.
        playerStatus.SetActive(false);
        minimap.SetActive(false);
        portalControl.SetActive(false);
        scoreUI.SetActive(false);
    }

    public void CaptureHideUI()
    {
        // Hide all UI when you capture screen
        capturePanel.SetActive(false);
        controller.SetActive(false);
        playerStatus.SetActive(false);
        minimap.SetActive(false);
        portalControl.SetActive(false);
        scoreUI.SetActive(false);
    }

    public void ShowUI()
    {
        // When you press "Back" button after capture screen, Hide "showImagePanel" window
        showImagePanel.SetActive(false);
        // Your "controller" and "capturePanel" will back.
        capturePanel.SetActive(true);
        controller.SetActive(true);
        // Your game will resume.
        Time.timeScale = 1;
    }
    public void ResumeGame()
    {
        // When you press "Back" button while in CameraMode/"capturePanel" window, the "capturePanel" will be hidden.
        capturePanel.SetActive(false);
        // And show all UI that was previously hidden.
        controller.SetActive(true);
        playerStatus.SetActive(true);
        minimap.SetActive(true);
        portalControl.SetActive(true);
        scoreUI.SetActive(true);
    }
    #endregion
}