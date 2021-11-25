using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeaderVisuals : MonoBehaviour
{
    [SerializeField] Image _myLine;
    Image _myImage;
    [SerializeField] Text _myName;
    private void Awake()
    {
        _myImage = GetComponent<Image>();
    }
    const float deltaColor = .35f;
    public void setName(string name)
    {
        if (_myName != null)
            _myName.text = name;
        else Debug.LogWarning("Text not set in header");
    }
    public void setTheme(Color color)
    {
        if (_myLine != null)
            _myLine.color = color;
        else Debug.LogWarning("Line color not set in header");
        color.r = Mathf.Clamp(color.r - deltaColor, 0, 255);
        color.g = Mathf.Clamp(color.g - deltaColor, 0, 255);
        color.b = Mathf.Clamp(color.b - deltaColor, 0, 255);
        if (_myImage != null)
            _myImage.color = color;
        else Debug.LogWarning("Image not set in header");
    }
}
