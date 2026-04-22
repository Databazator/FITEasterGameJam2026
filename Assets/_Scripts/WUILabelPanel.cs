using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class WUILabelPanel : MonoBehaviour
{
    public UIDocument Document;

    public bool StartVisible = false;
    public UnityEvent StartEvent;
    private Label _textLabel;    

    private Tweener _colorTweener;

    bool _visible = false;

    private void Awake()
    {
        if (!Document) Document = GetComponent<UIDocument>();

        _textLabel = Document.rootVisualElement.Q("Label") as Label;        
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartEvent?.Invoke();

        _visible = !StartVisible;
        if (StartVisible)
            ShowPanel(0f);
        else
            HidePanel(0f);
    }

    public void ShowPanelWithDelay(float delay)
    {
        DOVirtual.DelayedCall(delay, () => ShowPanel(1f));
    }

    public void HidePanelWithDelay(float delay)
    {
        DOVirtual.DelayedCall(delay, () => HidePanel(1f));
    }

    public void DisablePanelWithDelay(float delay)
    {
        DOVirtual.DelayedCall(delay, () => this.gameObject.SetActive(false));
    }

    public void ShowPanel(float duration)
    {
        if (!_visible)
        {
            _visible = !_visible;
            if(_colorTweener != null && _colorTweener.IsActive())
            {
                _colorTweener.Kill();
            }            
            _colorTweener = DOTween.ToAlpha(() => _textLabel.resolvedStyle.color, (Color clr) => _textLabel.style.color = new StyleColor(clr), 1f, duration).SetEase(Ease.InOutQuad);
            transform.localScale = new Vector3(transform.localScale.x, 0f, transform.localScale.z);
            transform.DOScaleY(1, duration + 0.1f).SetEase(Ease.OutQuad);
        }
    }

    public void HidePanel(float duration)
    {
        if (_visible)
        {
            _visible = !_visible;
            if (_colorTweener != null && _colorTweener.IsActive())
            {
                _colorTweener.Kill();
            }            
            _colorTweener = DOTween.ToAlpha(() => _textLabel.resolvedStyle.color, (Color clr) => _textLabel.style.color = new StyleColor(clr), 0f, duration).SetEase(Ease.InOutQuad);

            //transform.localScale = new Vector3(transform.localScale.x, 1f, transform.localScale.z);
            transform.DOScaleY(1.5f, duration + 0.1f).SetEase(Ease.InQuad).SetDelay(0.1f);

        }
    }

}
