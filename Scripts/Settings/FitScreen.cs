using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SweetCandy.Settings
{
    public class FitScreen : MonoBehaviour
    {
        public static float DevelopWidth = 1920f;
        public static float DevelopHeigh = 1080f;
        public static float DevelopRate = DevelopHeigh / DevelopWidth;
        public static int CurScreenHeight = Screen.height;
        public static int CurScreenWidth = Screen.width;
        public static float ScreenRate =  (float)Screen.height /  Screen.width;
        public static float CameraRectHeightRate = DevelopHeigh / (DevelopWidth / Screen.width * Screen.height);
        public static float CameraRectWidthRate = DevelopWidth / (DevelopHeigh / Screen.height * Screen.width);

        private void UpdateData(int x, int y)
        {
            CurScreenHeight = y;
            CurScreenWidth = x;
            ScreenRate =  (float)Screen.height / Screen.width;
            CameraRectHeightRate = DevelopHeigh / (DevelopWidth / Screen.width * Screen.height);
            CameraRectWidthRate =DevelopWidth / (DevelopHeigh / Screen.height * Screen.width);
        }
        
        public void FitCanvas()
        {
            CanvasScaler objectOfType = FindObjectOfType<CanvasScaler>();
            if (DevelopRate <= (double) ScreenRate)
                objectOfType.matchWidthOrHeight = 0.0f;
            else
                objectOfType.matchWidthOrHeight = 1f;
        }

        public void FitResolutionScreen(int xWidth, int yHeght) => this.StartCoroutine(this.SetResolutionAsync(xWidth, yHeght));

        private IEnumerator SetResolutionAsync(int xWidth, int yHeght)
        {
            Physics.autoSimulation = false;
            Physics.Simulate(0.02f);
            Physics.autoSimulation = true;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            UpdateData(xWidth, yHeght);
            FitCanvas();
        }
    }
}