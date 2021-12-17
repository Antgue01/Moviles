using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelVisuals : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum LevelVisualElement { Background, Border, Tick, Lock, Number }

    /// <summary>
    /// sets an element's visibility
    /// </summary>
    /// <param name="elem">the element to affect</param>
    /// <param name="visible">true if we want to see it, false otherwhise</param>
    public void setVisualVisible(LevelVisualElement elem, bool visible)
    {
        switch (elem)
        {
            case LevelVisualElement.Background:
                _bg.gameObject.SetActive(visible);
                break;
            case LevelVisualElement.Border:
                _border.gameObject.SetActive(visible);
                break;
            case LevelVisualElement.Tick:
                _tick.gameObject.SetActive(visible);
                break;
            case LevelVisualElement.Lock:
                _lock.gameObject.SetActive(visible);
                break;
            case LevelVisualElement.Number:
                _number.gameObject.SetActive(visible);
                break;
            default:
                Debug.LogWarning("Invalid elem passed");
                break;
        }
    }
    /// <summary>
    /// set the color of an element
    /// </summary>
    /// <param name="elem">the element whose color will be changed</param>
    /// <param name="color">the new color</param>
    public void setVisualColor(LevelVisualElement elem, Color color)
    {
        switch (elem)
        {
            case LevelVisualElement.Background:
                _bgColor = color;
                _bg.color = color;
                break;
            case LevelVisualElement.Border:
                _borderColor = color;
                _border.color = color;
                break;
            case LevelVisualElement.Tick:
                _tickColor = color;
                _tick.color = color;
                break;
            case LevelVisualElement.Lock:
                _lockColor = color;
                _lock.color = color;
                break;
            case LevelVisualElement.Number:
                _numberColor = color;
                _number.color = color;
                break;
            default:
                Debug.LogWarning("Invalid elem passed");
                break;
        }
    }
    /// <summary>
    /// sets the level number
    /// </summary>
    /// <param name="lvl">the level we want to play</param>
    public void setLevel(int lvl)
    {
        _number.text = lvl.ToString();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _border.color = _borderColor;
        _bg.color = _bgColor;
        _number.color = _numberColor;
        _lock.color = _lockColor;
        _tick.color = _tickColor;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _border.color = Color.white;
        _bg.color = Color.white;
        _number.color = Color.black;
        _lock.color = Color.white;
        _tick.color = Color.white;
    }
    Color _bgColor;
    Color _borderColor;
    Color _lockColor;
    Color _numberColor;
    Color _tickColor;
    [SerializeField] Image _bg;
    [SerializeField] Image _border;
    [SerializeField] Image _tick;
    [SerializeField] Image _lock;
    [SerializeField] Text _number;

}
