using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Runtime.Common.UI
{
    public class PolishingElement : MonoBehaviour
    {
        #region Members

        private Vector2 _originalPosition;
        private Vector3 _originalRotation;
        private Vector3 _originalScale;
        private Vector2 _originalAnchorPosition;
        private float _originalAlpha;
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private Tween _tween;

        #endregion Members

        #region Properties

        public Vector2 OriginalAnchorPosition => _originalAnchorPosition;

        #endregion Properties


        #region Class Methods

        private void Awake()
        {
            Build();
        }

        public void Build()
        {
            var thisTransform = transform;
            _originalPosition = thisTransform.position;
            _originalRotation = thisTransform.eulerAngles;
            _originalScale = thisTransform.localScale;
            _rectTransform = GetComponent<RectTransform>();
            if (_rectTransform) _originalAnchorPosition = _rectTransform.anchoredPosition;
        }

        private void OnDestroy()
        {
            if (_canvasGroup != null)
            {
                Destroy(_canvasGroup);
                _canvasGroup = null;
            }
        }

        private void OnEnable()
        {
            _tween = null;
        }

        private void OnDisable()
        {
            if (_tween != null) _tween.Kill();
        }

        public void ResetOrigin()
        {
            var thisTransform = transform;
            thisTransform.DOKill();
            this.DOKill();
            thisTransform.position = _originalPosition;
            thisTransform.eulerAngles = _originalRotation;
            thisTransform.localScale = _originalScale;
            if (_canvasGroup != null) _canvasGroup.alpha = _originalAlpha;
            if (_rectTransform) _rectTransform.anchoredPosition = _originalAnchorPosition;
        }

        // Fade.

        private void CheckCanvasGroupComponent()
        {
            _canvasGroup = GetComponent<CanvasGroup>(); 
            if (_canvasGroup == null) _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            _originalAlpha = 1;
            _canvasGroup.alpha = _originalAlpha;
        }
        
        public void SetAlpha(float value)
        {
            CheckCanvasGroupComponent();
            if (_canvasGroup != null) _canvasGroup.alpha = value;
        }

        public Tween DOFade(float targetValue, float duration)
        {
            CheckCanvasGroupComponent();
            if (_canvasGroup != null)
            {
                _tween = _canvasGroup.DOFade(targetValue, duration).SetUpdate(true);
                return _tween;
            }

            return null;
        }
        
        // Scale.
        public void SetScale(Vector3 value)
        {
            transform.localScale = value;
        }

        public Tween DOScale(Vector3 value, float duration)
        {
            _tween = transform.DOScale(value, duration).SetUpdate(true);
            return _tween;
        }
        
        // Transform Pos.
        public void SetPos(Vector3 value)
        {
            transform.position = value;
        }

        public Tween DOMove(Vector3 value, float duration)
        {
            _tween = transform.DOMove(value, duration).SetUpdate(true);
            return _tween;
        }
        
        // Anchor Pos.
        public void SetAnchorPos(Vector3 value)
        {
            if(_rectTransform)
                _rectTransform.anchoredPosition = value;
        }

        public Tween DOAnchorMove(Vector3 value, float duration)
        {
            if (_rectTransform)
            {
                _tween = _rectTransform.DOAnchorPos(value, duration).SetUpdate(true);
                return _tween;
            }

            return null;
        }
        
        public void SetAnchorPosX(float value)
        {
            if (_rectTransform)
            {
                var pos = _rectTransform.anchoredPosition;
                pos.x = value;
                _rectTransform.anchoredPosition = pos;
            }
        }

        public Tween DOAnchorMoveX(float value, float duration)
        {
            if (_rectTransform)
            {
                _tween = _rectTransform.DOAnchorPosX(value, duration).SetUpdate(true);
                return _tween;
            }

            return null;
        }
        
        public void SetAnchorPosY(float value)
        {
            if (_rectTransform)
            {
                var pos = _rectTransform.anchoredPosition;
                pos.y = value;
                _rectTransform.anchoredPosition = pos;
            }
        }

        public Tween DOAnchorMoveY(float value, float duration)
        {
            if (_rectTransform)
            {
                _tween = _rectTransform.DOAnchorPosY(value, duration).SetUpdate(true);
                return _tween;
            }

            return null;
        }

        #endregion Class Methods
    }
}

