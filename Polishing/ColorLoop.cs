using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Common.UI
{
    public class ColorLoop : MonoBehaviour
    {
        public Image targetImage; 
        public float cycleDuration = 5f;
        private float hue = 0f;

        private void Awake()
        {
            targetImage = GetComponent<Image>();
        }

        private void Update()
        {
            hue += Time.deltaTime / cycleDuration;
            hue %= 1f;
            targetImage.color = Color.HSVToRGB(hue, 1f, 1f);
        }
    }
}

