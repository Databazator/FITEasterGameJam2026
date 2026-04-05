using DG.Tweening;
using UnityEngine;
using UnityEngine.UIElements;

public class GUIManager : MonoBehaviour
{
    [SerializeField] protected UIDocument _document;
    protected VisualElement _rootElement;

    public float TypingSpeed = 15f;

    Label _textLabel;
    VisualElement _fadeScreen;
    VisualElement _interactionIndicator;

    private Tweener _fadeTweener;
    private Tweener _typingTweener;
    private Tweener _indicTweener;

    private static GUIManager _instance;
    public static GUIManager Instance => _instance;
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
        }
        else
        {
            _instance = this;
        }

        _rootElement = _document.rootVisualElement;
        _textLabel = _rootElement.Q("TextLabel") as Label;
        _textLabel.text = "";

        _fadeScreen = _rootElement.Q("FadeScreen") as VisualElement;
        _fadeScreen.style.backgroundColor = Color.black;

        _interactionIndicator = _rootElement.Q("InteractionIndicator") as VisualElement;
        ShowIndicator(false, 0f);
        //_interactionIndicator.style.width;
    }

    public Tweener Fade(float targetAlpha, float duration, Ease easeType)
    {
        if (_fadeTweener != null && _fadeTweener.IsActive())
        {
            _fadeTweener.Kill();
        }
        Color targetColor = Color.black;
        targetColor.a = targetAlpha;
        _fadeTweener = DOTween.To(() => _fadeScreen.style.backgroundColor.value,
            (Color clr) => _fadeScreen.style.backgroundColor = clr, targetColor, duration).SetEase(easeType);
        return _fadeTweener;
    }

    public void WriteText(string text, float fadeInDuration)
    {
        if (_typingTweener != null && _typingTweener.IsActive())
        {
            _typingTweener.Kill();
        }

        float timeToType = text.Length / TypingSpeed;
        _typingTweener = DOTween.To(() => _textLabel.text, (string str) => _textLabel.text = str, text, timeToType);

        DOVirtual.DelayedCall(timeToType + fadeInDuration, () => _textLabel.text = "");
    }

    public void ShowIndicator(bool show, float duration = 0.25f)
    {

        if (_indicTweener != null && _indicTweener.IsActive())
        {
            _indicTweener.Kill();
        }

        Color target = show ? Color.white : Color.clear;

        _indicTweener = DOTween.To(() => _interactionIndicator.style.unityBackgroundImageTintColor.value,
            (Color clr) => _interactionIndicator.style.unityBackgroundImageTintColor = clr, target, duration).SetEase(Ease.InOutQuad);
    }
}
