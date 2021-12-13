using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class GameBox : MonoBehaviour
{
    //Non variable Tile Types
    public enum BoxType { Bridge, Hollow, FlowPoint, Empty }

    public void setType(BoxType t)
    {
        _type = t;
    }

    public void setFlow(Flow flow, LinkedListNode<GameBox> node)
    {
        _myFlow = flow;
        _myNode = node;
    }
    public void setConfirmedNode(LinkedListNode<GameBox> confirmedNode)
    {
        _myConfirmedNode = confirmedNode;
    }
    public BoxType getType()
    {
        return _type;
    }

    public void setColor(Color c)
    {
        _myColor = c;

        _figureImage.GetComponent<SpriteRenderer>().color = _myColor;
        _animImage.GetComponent<SpriteRenderer>().color = _myColor;
        _pathImage.GetComponent<SpriteRenderer>().color = _myColor;

        Color colorAlphaReduced = _myColor;
        colorAlphaReduced.a = 0.3f;
        _backgroundImage.GetComponent<SpriteRenderer>().color = colorAlphaReduced;
    }

    public Color getColor()
    {
        return _myColor;
    }

    public void setPathColor(Color c)
    {
        _pathImage.GetComponent<SpriteRenderer>().color = c;
    }

    public Color getPathColor()
    {
        return _pathImage.GetComponent<SpriteRenderer>().color;
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
    public void setFlowDir(Vector2Int dir)
    {
        _flowDir = dir;
    }

    public Vector2Int getFlowDir()
    {
        return _flowDir;
    }

    public void setConfirmedFlowDir(Vector2Int dir)
    {
        _confirmedFlowDir = dir;
    }

    public Vector2Int getConfirmedFlowDir()
    {
        return _confirmedFlowDir;
    }
    public void setOriginFlowPoint(GameBox origin)
    {
        _originFlowPoint = origin;
    }
    public GameBox getOriginFlowPoint()
    {
        return _originFlowPoint;
    }

    public void setOtherFlowPoint(GameBox other)
    {
        _otherFlowPoint = other;
    }

    public void setBackgroundActive(bool b)
    {
        _backgroundImage.SetActive(b);
    }
    public void setPathActive(bool b)
    {
        _pathImage.SetActive(b);
    }

    public void setFigureSprite(Sprite s)
    {
        if (s == null)
        {
            _figureImage.SetActive(false);
            _animImage.SetActive(false);
        }
        else
        {
            if (!_animImage.activeSelf)
                _animImage.SetActive(true);
            _animImage.GetComponent<SpriteRenderer>().sprite = s;

            if (!_figureImage.activeSelf)
                _figureImage.SetActive(true);
            _figureImage.GetComponent<SpriteRenderer>().sprite = s;

        }
    }

    public Flow getFlow()
    {
        return _myFlow;
    }

    public bool getPathActive()
    {
        return _pathImage.activeSelf;
    }

    /// <summary>
    /// Sets the pipe position and rotation from a given direction to be active in this tile
    /// </summary>
    /// <param name="dir"></param>
    public void setPathFrom(Vector2Int dir)
    {
        setFlowDir(dir);
        setPathActive(true);

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

    /// <summary>
    /// Unlinks all the flows from this Tile onwards
    /// </summary>
    public void cutFromThisTile()
    {
        GameBox aux = getNextGB();
        setNextGB(null);
        while (aux != null)
        {
            aux.setPreviusGB(null);
            aux.setPathActive(false);
            GameBox originFP = aux.getOriginFlowPoint();
            if (originFP != null)
            {
                aux = aux.getNextGB();
                originFP.tryToRestoreFromOrigin();
                continue;
            }

            GameBox auxNext = aux.getNextGB();
            aux.setNextGB(null);
            aux = auxNext;
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
            if (getColor() == aux.getPathColor())
            {
                aux.setPathActive(false);
                aux.setNextGB(null);
            }
        }
    }

    /// <summary>
    /// Called if this GameBox is a Flow Point
    /// </summary>
    public void confirmFlows()
    {
        if (getType() != BoxType.FlowPoint) return;

        GameBox aux = this;
        Color confirmedColor = getColor();
        setBackgroundActive(aux.getNextGB() != null &&
            aux.getNextGB().getPathColor() == confirmedColor);

        while (aux.getNextGB() != null &&
            aux.getNextGB().getPathColor() == confirmedColor)
        {
            aux.setNextConfirmedGB(aux.getNextGB());
            aux = aux.getNextGB();
            //If it already has an origin flow point
            GameBox originFP = aux.getOriginFlowPoint();
            if (originFP != null)
            {
                originFP.disconfirmFlows();
                originFP.confirmFlows();
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
        if (getType() != BoxType.FlowPoint) return;

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
    /// Tries to restore confirmed flow tiles from origin, called if this GameBox is a Flow Point
    /// </summary>
    public void tryToRestoreFromOrigin()
    {
        GameBox aux = this;

        while (aux.getNextConfirmedGB() != null)
        {
            //If there is an active flow
            if (aux.getNextConfirmedGB().getPathActive())
            {
                //With the same color, we pass to the next confirmed tile
                if (getColor() == aux.getNextConfirmedGB().getPathColor())
                {
                    aux = aux.getNextConfirmedGB();
                }
                //With a different color, we stop restoring
                else
                {
                    break;
                }
            }
            //Restore the confirmed flow if there is no current flow at this tile
            else
            {
                aux.setNextGB(aux.getNextConfirmedGB());
                aux = aux.getNextConfirmedGB();
                aux.setPathColor(aux.getColor());
                aux.setPathFrom(aux.getConfirmedFlowDir());
            }
        }
    }
    public void setPreviusGB(GameBox gameBox)
    {
        _previusGameBox = gameBox;
    }
    public GameBox getPreviusGB()
    {
        return _previusGameBox;
    }
    public void setAsFirst()
    {
        if (!_last)
            _first = true;
        else Debug.LogError("This gameBox is the last one of the flow. Can't be the first");
    }
    public bool isLast()
    {
        return _last;
    }
    public void setAsLast()
    {
        if (!_first)
            _last = true;
        else Debug.LogError("This gameBox is the first one of the flow. Can't be the last");
    }
    public bool isFirst()
    {
        return _first;
    }

    public LinkedListNode<GameBox> getNode()
    {
        return _myNode;
    }
    public LinkedListNode<GameBox> getConfirmedNode()
    {
        return _myConfirmedNode;
    }
    public void restore()
    {
        setBackgroundActive(false);
        setPathActive(false);
        setOriginFlowPoint(null);
        setNextGB(null);
        setNextConfirmedGB(null);
    }

    private Color _myColor = Color.white;
    private BoxType _type;
    //Used to link flows not confirmed yet
    private GameBox _nextGameBox = null;
    private GameBox _previusGameBox = null;
    //Used to link flows and save the confirmed state of them
    private GameBox _nextConfirmedGameBox = null;
    //Current flow dir
    private Vector2Int _flowDir;
    //Confirmed flow dir
    private Vector2Int _confirmedFlowDir;
    //The origin flow point of this flow
    private GameBox _originFlowPoint = null;
    //Needs to know the other flow point in case this is one of them
    private GameBox _otherFlowPoint = null;
    const int NumFlows = 16;
    private Flow _myFlow;
    private LinkedListNode<GameBox> _myNode;
    private LinkedListNode<GameBox> _myConfirmedNode;
    bool _first = false;
    bool _last = false;

    [SerializeField] private GameObject _backgroundImage;
    [SerializeField] private GameObject _figureImage;
    [SerializeField] private GameObject _pathImage;
    [SerializeField] private GameObject _animImage;
}

