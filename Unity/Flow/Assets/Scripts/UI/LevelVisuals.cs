using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelVisuals : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
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
                _bg.color = color;
                break;
            case LevelVisualElement.Border:
                _border.color = color;
                break;
            case LevelVisualElement.Tick:
                _tick.color = color;
                break;
            case LevelVisualElement.Lock:
                _lock.color = color;
                break;
            case LevelVisualElement.Number:
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
      
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _border.color = Color.white;
        _bg.color = Color.white;
        _number.color = Color.black;
    }

    [SerializeField] Image _bg;
    [SerializeField] Image _border;
    [SerializeField] Image _tick;
    [SerializeField] Image _lock;
    [SerializeField] Text _number;

}
