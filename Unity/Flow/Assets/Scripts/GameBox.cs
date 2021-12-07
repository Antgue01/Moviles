using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameBox : MonoBehaviour
{
    public enum BoxType { Bridge, Hollow, FlowPoint, Flow, Empty }

    public void setInitType(BoxType t)
    {
        _initType = t;
        _type = t;
    }
    public void setType(BoxType t)
    {
        _type = t;
    }

    public BoxType getType()
	{
        return _type;
	}

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
        _myColor = c;
        _figureImage.GetComponent<SpriteRenderer>().color = _myColor;
        if (_type == BoxType.Flow)
            _pathImage.GetComponent<SpriteRenderer>().color = _myColor;
    }

    public Color getFigureColor()
	{
        return _myColor;
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
        _pathImage.SetActive(false);
    }

    public void setPathFrom(Vector2Int dir)
	{
        _pathImage.SetActive(true);

        switch (dir)
		{
            case Vector2Int v when v.Equals(Vector2Int.up):
                _pathImage.transform.localPosition = new Vector3(0, 0.5f, 0);
                _pathImage.transform.localEulerAngles = new Vector3(0, 0, -90);
                break;
            case Vector2Int v when v.Equals(Vector2Int.down):
                _pathImage.transform.localPosition = new Vector3(0, -0.5f, 0);
                _pathImage.transform.localEulerAngles = new Vector3(0, 0, 90);
                break;
            case Vector2Int v when v.Equals(Vector2Int.right):
                _pathImage.transform.localPosition = new Vector3(-0.5f, 0, 0);
                _pathImage.transform.localEulerAngles = new Vector3(0, 0, 0);
                break;
            case Vector2Int v when v.Equals(Vector2Int.left):
                _pathImage.transform.localPosition = new Vector3(0.5f, 0, 0);
                _pathImage.transform.localEulerAngles = new Vector3(0, 0, 180);
                break;
        }
	}

    public void setNextGB(GameBox gb)
	{
        _nextGameBox = gb;
	}

    public GameBox getNextGB()
	{
        return _nextGameBox;
	}

    /// <summary>
    /// Unlinks all the GameBox from this Tile
    /// </summary>
    public void cutFromThisTile()
	{
        GameBox aux = this;
		while (aux.getNextGB() != null)
		{
            GameBox nextAux = aux.getNextGB();
            aux.setNextGB(null);
            nextAux.reset();
            aux = nextAux;
		}
        if (_otherFlowPoint != null && _otherFlowPoint.getNextGB() != null) _otherFlowPoint.cutFromThisTile();
	}

    public void setOtherFlowPoint(GameBox other)
	{
        _otherFlowPoint = other;
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
    private Color _myColor = Color.white;
    private BoxType _type;
    private BoxType _initType;
    private GameBox _nextGameBox = null;
    //The other flow point of this color
    private GameBox _otherFlowPoint = null;
    const int NumFlows = 16;

    [SerializeField] private GameObject _figureImage;
    [SerializeField] private GameObject _pathImage;
}

