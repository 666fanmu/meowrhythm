using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SweetCandy.ShadersManager
{
    [ExecuteInEditMode]
    public class SetStaveRoad : MonoBehaviour
    {
        public Material[] StaveRodeMaterials;
        public Transform ShoppingCartTransform;
        public float SwerveX = 0f, SwerveY = 0f;

        private void Awake()
        {
            Vector4 pos = ShoppingCartTransform.position;
            foreach (var mat in StaveRodeMaterials)
            {
                mat.SetVector("_StartPosition", pos);
                mat.SetFloat("_SwerveX", SwerveX);
                mat.SetFloat("_SwerveY", SwerveY);
            }
        }
    }
}