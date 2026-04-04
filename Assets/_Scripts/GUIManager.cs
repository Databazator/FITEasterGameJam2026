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

    private Tweener _fadeTweener;
    private Tweener _typingTweener;

    private static GUIManager _instance;
    public static GUIManager Instance => _instance;
    private void Awake()
    {
        if(_instance != null && _instance != this)
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
    }

    public Tweener Fade(float targetAlpha, float duration, Ease easeType)
    {
        if(_fadeTweener != null && _fadeTweener.IsActive()) {
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


    
}
