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

    public void setBackgroundActive(bool b)
	{
        _backgroundImage.SetActive(b);
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

    public void setColor(Color c)
    {
        _myColor = c;
        
        _figureImage.GetComponent<SpriteRenderer>().color = _myColor;
		_pathImage.GetComponent<SpriteRenderer>().color = _myColor;

        Color colorAlphaReduced = _myColor;
        colorAlphaReduced.a = 0.3f;
        _backgroundImage.GetComponent<SpriteRenderer>().color = colorAlphaReduced;
    }

	public Color getColor()
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
        _backgroundImage.SetActive(false);
        _pathImage.SetActive(false);
        _originFlowPoint = null;
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
    /// Unlinks all the GameBox from this Tile onwards
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
        //We cut also from the other flow point, in case we cut from one of them
        if (_otherFlowPoint != null && _otherFlowPoint.getNextGB() != null)
		{
            _otherFlowPoint.disconfirmFlows();
            _otherFlowPoint.cutFromThisTile();
        }
	}

    public void setOriginFlowPoint(GameBox origin)
	{
        _originFlowPoint = origin;
	}
    public GameBox getOriginFlowPoint()
    {
        return _originFlowPoint;
    }

    /// <summary>
    /// Called if this GameBox is a Flow Point
    /// </summary>
    public void confirmFlows()
	{
        GameBox aux = this;
        setBackgroundActive(aux.getNextGB() != null);

        while (aux.getNextGB() != null)
        {
            aux = aux.getNextGB();
            aux.setOriginFlowPoint(this);
            aux.setBackgroundActive(true);
        }
    }

    /// <summary>
    /// Called if this GameBox is a Flow Point
    /// </summary>
    public void disconfirmFlows()
    {
        GameBox aux = this;
        setBackgroundActive(false);

        while (aux.getNextGB() != null)
        {
            aux = aux.getNextGB();
            aux.setBackgroundActive(false);
        }
    }

    public void setOtherFlowPoint(GameBox other)
	{
        _otherFlowPoint = other;
	}

    private Color _myColor = Color.white;
    private BoxType _type;
    private BoxType _initType;
    private GameBox _nextGameBox = null;
    //The origin flow point of this flow
    private GameBox _originFlowPoint = null;
    //Needs to know the other flow point in case this is one of them
    private GameBox _otherFlowPoint = null;
    const int NumFlows = 16;

    [SerializeField] private GameObject _backgroundImage;
    [SerializeField] private GameObject _figureImage;
    [SerializeField] private GameObject _pathImage;
}

