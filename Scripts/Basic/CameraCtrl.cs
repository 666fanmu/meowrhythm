using System;
using System.Collections;
using SweetCandy.Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace SweetCandy.Basic
{
    public class CameraCtrl : MonoBehaviour
    {
        public GameObject beginCam;
        public GameObject normalCam;
        public GameObject highCam;
        public GameObject bossFightCam;
        public GameObject beCatchedCam;
        private void Start()
        {
            highCam.SetActive(false);
            StartCoroutine(CamCoroutine());
            /*EventManager.Instance.StartListening("ToHighCam",ToHighCam);
            EventManager.Instance.StartListening("ToNormalCam",ToNormalCam);
            EventManager.Instance.StartListening("ToBossFightCam",ToBossFightCam);*/
        }

        private void OnDisable()
        {
            /*EventManager.Instance.StopListening("ToHighCam",ToHighCam);
            EventManager.Instance.StopListening("ToNormalCam",ToNormalCam);
            EventManager.Instance.StopListening("ToBossFightCam",ToBossFightCam);*/
        }

        public void ToNormalCam()
        {
            highCam.SetActive(false);
            bossFightCam.SetActive(false);
            beCatchedCam.SetActive(false);
        }

        public void ToBeCatchedCam()
        {
            beCatchedCam.SetActive(true);
            highCam.SetActive(false);
            bossFightCam.SetActive(false);
        }
        public void ToBossFightCam()
        {
            beCatchedCam.SetActive(false);
            highCam.SetActive(false);
            bossFightCam.SetActive(true);
        }

        public void ToHighCam()
        {
            beCatchedCam.SetActive(false);
            bossFightCam.SetActive(false);
            highCam.SetActive(true);
        }
        IEnumerator CamCoroutine()
        {
            yield return new WaitForSeconds(3f);
            beginCam.SetActive(false);
        }
    }
}