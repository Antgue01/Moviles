using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameBox: MonoBehaviour
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

    public void setImage(Sprite s)
    {
        _img = s;
        _gameBox.GetComponent<Image>().sprite = _img;
    }

    public void setColor(Color c)
    {
        _color = c;
    }

    public void setPos(Vector2 p)
    {
        _position = p;
        _gameBox.transform.position = _position;
    }

    public void reset()
    {
        if(_type != BoxType.Hollow)
        {
            _type = _initType;
        }
    }

    private Color _color;
    private Sprite _img;
    private Vector2 _position;
    private BoxType _type;
    private BoxType _initType;
    
    [SerializeField] GameObject _gameBox;
}

