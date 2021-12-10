using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameBox : MonoBehaviour
{
    //Non variable Tile Types
    public enum BoxType { Bridge, Hollow, FlowPoint, Empty }

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

    public void setPathActive(bool b)
    {
        _pathImage.SetActive(b);
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

    public void setPathColor(Color c)
	{
        _pathImage.GetComponent<SpriteRenderer>().color = c;
    }

    public Color getPathColor()
	{
        return _pathImage.GetComponent<SpriteRenderer>().color;
	}

	public Color getColor()
	{
        return _myColor;
	}

    public void restore()
    {
        //It was a confirmed Tile
		if (_originFlowPoint != null)
		{
            _pathImage.SetActive(false);
            _originFlowPoint.tryToRestoreFromOrigin();
		}
		else
		{
            _pathImage.SetActive(false);
        }
    }

    public void setPathFrom(Vector2Int dir)
	{
        _flowDir = dir;
        _pathImage.SetActive(true);

        switch (_flowDir)
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

    public void setNextConfirmedGB(GameBox gb)
    {
        _nextConfirmedGameBox = gb;
    }

    public GameBox getNextConfirmedGB()
    {
        return _nextConfirmedGameBox;
    }

    public void setConfirmedFlowDir(Vector2Int dir)
	{
        _confirmedFlowDir = dir;
	}

    public Vector2Int getConfirmedFlowDir()
	{
        return _confirmedFlowDir;
	}

    public Vector2Int getFlowDir()
	{
        return _flowDir;
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
            nextAux.restore();
            aux = nextAux;
		}
	}

    public void hideConfirmedFromThisTile()
	{
        GameBox aux = this;
        aux.setPathActive(false);
        aux.setNextGB(null);
        
        while (aux.getNextConfirmedGB() != null)
        {
            aux = aux.getNextConfirmedGB();
            if(getColor() == aux.getPathColor())
			{
                aux.setPathActive(false);
                aux.setNextGB(null);
            }
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
        Color confirmedColor = getColor();
        setBackgroundActive(aux.getNextGB() != null);

        while (aux.getNextGB() != null)
        {
            aux.setNextConfirmedGB(aux.getNextGB());
            aux = aux.getNextGB();
			if (aux.getOriginFlowPoint() != null)
			{
                aux.disconfirmFromThisTile();
			}
            aux.setOriginFlowPoint(this);
            aux.setBackgroundActive(true);
            aux.setColor(confirmedColor);
            aux.setConfirmedFlowDir(aux.getFlowDir());
        }
    }

    public void disconfirmFromThisTile()
	{
        GameBox aux = this;
        aux.setOriginFlowPoint(null);
        aux.setBackgroundActive(false);

        while (aux.getNextConfirmedGB() != null)
        {
            GameBox nextAux = aux.getNextConfirmedGB();
            aux.setNextConfirmedGB(null);
            aux = nextAux;
            aux.setOriginFlowPoint(null);
            aux.setBackgroundActive(false);
        }
    }

    /// <summary>
    /// Called if this GameBox is a Flow Point
    /// </summary>
    public void disconfirmFlows()
    {
        GameBox aux = this;
        setBackgroundActive(false);
        
        while (aux.getNextConfirmedGB() != null)
        {
            GameBox nextAux = aux.getNextConfirmedGB();
            aux.setNextConfirmedGB(null);
            aux = nextAux;
            aux.setOriginFlowPoint(null);
            aux.setBackgroundActive(false);
        }

        //We disconfirm and cut also from the other flow point, in case we disconfirm from one of them
        if (_otherFlowPoint != null && _otherFlowPoint.getNextConfirmedGB() != null)
        {
            _otherFlowPoint.disconfirmFlows();
            _otherFlowPoint.cutFromThisTile();
        }
    }
    /// <summary>
    /// Tries to restore confirmed tiles from origina, called if this GameBox is a Flow Point
    /// </summary>
    public void tryToRestoreFromOrigin()
	{
        GameBox aux = this;

        while (aux.getNextConfirmedGB() != null)
        {
            if(aux.getNextConfirmedGB().getPathActive())
			{
                if(getColor() == aux.getNextConfirmedGB().getPathColor())
				{
                    aux = aux.getNextConfirmedGB();
                }
				else
				{
                    break;
				}
            }
			else
			{
                aux.setNextGB(aux.getNextConfirmedGB());
                aux = aux.getNextConfirmedGB();
                aux.setPathColor(aux.getColor());
                aux.setPathFrom(aux.getConfirmedFlowDir());
            }
        }
    }

    public void setOtherFlowPoint(GameBox other)
	{
        _otherFlowPoint = other;
	}

    public bool getPathActive()
	{
        return _pathImage.activeSelf;
	}

    private Color _myColor = Color.white;
    private BoxType _type;
    private GameBox _nextGameBox = null;
    private GameBox _nextConfirmedGameBox = null;

    private Vector2Int _flowDir;
    private Vector2Int _confirmedFlowDir;
    //The origin flow point of this flow
    private GameBox _originFlowPoint = null;
    //Needs to know the other flow point in case this is one of them
    private GameBox _otherFlowPoint = null;
    const int NumFlows = 16;

    [SerializeField] private GameObject _backgroundImage;
    [SerializeField] private GameObject _figureImage;
    [SerializeField] private GameObject _pathImage;
}

