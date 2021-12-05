using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameBox : MonoBehaviour
{
    public enum BoxType { Bridge, Hollow, Flow, Empty }

    public void setInitType(BoxType t)
    {
        _initType = t;
        _type = t;
    }
    public void setType(BoxType t)
    {
        _type = t;
    }

    public bool isBridge()
    {
        return _type == BoxType.Bridge;
    }

    public bool isHollow()
    {
        return _type == BoxType.Hollow;
    }

    //public void setFigureImageSize(float w, float h)
    //{
    //    _figureImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
    //    _figureImage.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
    //}
    public void setFigureSprite(Sprite s)
    {
        if (s == null)
        {
            _figureImage.SetActive(false);
        }
        else
        {
            if (!_figureImage.activeSelf)
                _figureImage.SetActive(true);
            _figureImage.GetComponent<SpriteRenderer>().sprite = s;
        }
    }

    public void setFigureColor(Color c)
    {
        _figureImage.GetComponent<SpriteRenderer>().color = c;
    }

    //public void setPos(Vector2 p)
    //{
    //    _position = p;
    //    transform.position = _position;
    //}

    public void reset()
    {
        if (_type != BoxType.Hollow)
        {
            _type = _initType;
        }
    }
    //public void setIndex(int ind)
    //{
    //    if (_type == BoxType.Flow && ind > -1 && ind < NumFlows)
    //        _flowIndex = ind;
    //}
    //public int getIndex()
    //{
    //    return _flowIndex;
    //}
    private Vector2 _position;
    int _flowIndex;
    private BoxType _type;
    private BoxType _initType;
    const int NumFlows = 16;

    [SerializeField] private GameObject _figureImage;
}

