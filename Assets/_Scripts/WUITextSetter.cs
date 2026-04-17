using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class WUITextSetter : MonoBehaviour
{
    public UIDocument document;
    private Label _textLabel;
    public string LabelText;

    private void OnValidate()
    {
        if(document != null && !EditorApplication.isCompiling && !EditorApplication.isPlayingOrWillChangePlaymode)
        {
            if (document.rootVisualElement != null)
            {
                try
                {
                    _textLabel = document.rootVisualElement.Q("Label") as Label;
                    if (_textLabel != null)
                        _textLabel.text = LabelText;
                } catch(Exception ex)
                {

                }             
            }
        }
    }

    private void Awake()
    {
        _textLabel = document.rootVisualElement.Q("Label") as Label;
        if (_textLabel != null)
            _textLabel.text = LabelText;
    }
}
