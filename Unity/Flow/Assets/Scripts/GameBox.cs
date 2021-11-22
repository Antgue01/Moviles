using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameBox
{
    [SerializeField]
    public GameObject gameBox;

    

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
        gameBox.GetComponent<Image>().sprite = _img;
    }

    public void setColor(Color c)
    {
        _color = c;
    }

    public void setPos(Vector2 p)
    {
        _position = p;
        gameBox.transform.position = _position;
    }

    private Color _color;
    private Sprite _img;
    private Vector2 _position;
    private BoxType _type;
    private enum BoxType { Bridge, Hollow, Flow, Empty }
}
