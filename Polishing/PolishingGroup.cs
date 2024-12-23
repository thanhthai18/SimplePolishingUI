using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.Common.UI
{
    public enum PolishingAnimateType
    {
        None = 0,
        Fade = 1,
        Scale = 2,
        MoveAnchor = 3,
        MoveAnchorX = 4,
        MoveAnchorY = 5,
    }
    
    public class PolishingGroup : MonoBehaviour
    {
        #region Members

        [SerializeField] private PolishingAnimateType _polishingAnimateType;
        [SerializeField] private PolishingElement[] _polishingElements;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private float _delayBettwenElements;
        [SerializeField] private Ease _ease = Ease.OutBack;
        [SerializeField] private bool _customCurve;
        [SerializeField] private int _delayActiveCount;
        [ShowIf(nameof(_customCurve))][SerializeField] private AnimationCurve _curve;

        [ShowIf("@_polishingAnimateType == PolishingAnimateType.MoveAnchor ||" +
                " _polishingAnimateType == PolishingAnimateType.MoveAnchorX ||" +
                " _polishingAnimateType == PolishingAnimateType.MoveAnchorY")]
        [SerializeField]
        private bool _useOffsetPos;
        
        [ShowIf("@_polishingAnimateType == PolishingAnimateType.MoveAnchor")] [SerializeField]
        private Vector2 _beginVectorValue;
        
        [ShowIf("@_polishingAnimateType == PolishingAnimateType.MoveAnchorX || _polishingAnimateType == PolishingAnimateType.MoveAnchorY")] [SerializeField]
        private float _beginValue;
        
        private Sequence _sequence;
        private CancellationTokenSource _cancellationTokenSource;

        #endregion Members


        #region Class Methods

        private void OnEnable()
        {
            if (_delayActiveCount > 0)
            {
                _delayActiveCount--;
                return;
            }
            Trigger();
        }
        
        public void Trigger()
        {
            if (_polishingAnimateType != PolishingAnimateType.None)
            {
                _sequence.Kill();
                _polishingElements.ForEach(s => s.ResetOrigin());
                _sequence = DOTween.Sequence();
                if (_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Cancel();
                }
                
                _cancellationTokenSource = new();
            }
            else
            {
                return;
            }
          
            switch (_polishingAnimateType)
            {
                case PolishingAnimateType.Fade:
                    ExecuteFade().Forget();
                    break;
                case PolishingAnimateType.Scale:
                    ExecuteScale().Forget();
                    break;
                case PolishingAnimateType.MoveAnchor:
                    ExecuteMove().Forget();
                    break;
                case PolishingAnimateType.MoveAnchorX:
                    ExecuteMoveX().Forget();
                    break;
                case PolishingAnimateType.MoveAnchorY:
                    ExecuteMoveY().Forget();
                    break;
            }

            _sequence.SetUpdate(true);
        }

        private void OnDisable()
        {
            if (_polishingAnimateType != PolishingAnimateType.None)
            {
                _sequence.Kill();
                _polishingElements.ForEach(s => s.ResetOrigin());
                if (_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Cancel();
                }
            }
        }

        private async UniTask ExecuteFade()
        {
            for (int i = 0; i < _polishingElements.Length; i++)
            {
                int index = i;
                var element = _polishingElements[index];
                element.SetAlpha(0);
                var tween = element.DOFade(1, _duration);
                if (tween != null)
                {
                    if (_customCurve)
                        tween.SetEase(_curve);
                    else
                        tween.SetEase(_ease);
                    _sequence.Join(tween);
                }

                if(_delayBettwenElements > 0)
                 await UniTask.Delay(TimeSpan.FromSeconds(_delayBettwenElements), cancellationToken: _cancellationTokenSource.Token);
            }
        }
        
        private async UniTask ExecuteScale()
        {
            for (int i = 0; i < _polishingElements.Length; i++)
            {
                int index = i;
                var element = _polishingElements[index];
                element.SetScale(Vector3.zero);
                var tween = element.DOScale(Vector3.one, _duration);
                if (tween != null)
                {
                    if (_customCurve)
                        tween.SetEase(_curve);
                    else
                        tween.SetEase(_ease);
                    _sequence.Join(tween);
                }
                
                if(_delayBettwenElements > 0)
                    await UniTask.Delay(TimeSpan.FromSeconds(_delayBettwenElements), cancellationToken: _cancellationTokenSource.Token);
            }
        }
        
        private async UniTask ExecuteMove()
        {
            for (int i = 0; i < _polishingElements.Length; i++)
            {
                int index = i;
                var element = _polishingElements[index];
                var value = _beginVectorValue;
                if (_useOffsetPos)
                    value = element.OriginalAnchorPosition + _beginVectorValue;
                element.SetAnchorPos(value);
                var tween = element.DOAnchorMove(element.OriginalAnchorPosition, _duration);
                if (tween != null)
                {
                    if (_customCurve)
                        tween.SetEase(_curve);
                    else
                        tween.SetEase(_ease);
                    _sequence.Join(tween);
                }
                
                if(_delayBettwenElements > 0)
                    await UniTask.Delay(TimeSpan.FromSeconds(_delayBettwenElements), cancellationToken: _cancellationTokenSource.Token);
            }
        }
        
        private async UniTask ExecuteMoveX()
        {
            for (int i = 0; i < _polishingElements.Length; i++)
            {
                int index = i;
                var element = _polishingElements[index];
                var value = _beginValue;
                if (_useOffsetPos)
                    value = element.OriginalAnchorPosition.x + _beginValue;
                element.SetAnchorPosX(value);
                var tween = element.DOAnchorMoveX(element.OriginalAnchorPosition.x, _duration);
                if (tween != null)
                {
                    if (_customCurve)
                        tween.SetEase(_curve);
                    else
                        tween.SetEase(_ease);
                    _sequence.Join(tween);
                }
                
                if(_delayBettwenElements > 0)
                    await UniTask.Delay(TimeSpan.FromSeconds(_delayBettwenElements), cancellationToken: _cancellationTokenSource.Token);
            }
        }
        
        private async UniTask ExecuteMoveY()
        {
            for (int i = 0; i < _polishingElements.Length; i++)
            {
                int index = i;
                var element = _polishingElements[index];
                var value = _beginValue;
                if (_useOffsetPos)
                    value = element.OriginalAnchorPosition.y + _beginValue;
                element.SetAnchorPosY(value);
                var tween = element.DOAnchorMoveY(element.OriginalAnchorPosition.y, _duration);
                if (tween != null)
                {
                    if (_customCurve)
                        tween.SetEase(_curve);
                    else
                        tween.SetEase(_ease);
                    _sequence.Join(tween);
                }
                
                if(_delayBettwenElements > 0)
                    await UniTask.Delay(TimeSpan.FromSeconds(_delayBettwenElements), cancellationToken: _cancellationTokenSource.Token);
            }
        }

        #endregion Class Methods
    }
}

