using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Common.UI
{
    public class ColorLoopSprite : MonoBehaviour
    {
        public SpriteRenderer targetRenderer; 
        public float cycleDuration = 5f;
        private float hue = 0f;

        private void Awake()
        {
            targetRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            hue += Time.deltaTime / cycleDuration;
            hue %= 1f;
            targetRenderer.color = Color.HSVToRGB(hue, 1f, 1f);
        }
    }
}

