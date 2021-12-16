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
    public void setConfirmedNode(LinkedListNode<GameBox> confirmedNode)
    {
        _myConfirmedNode = confirmedNode;
    }
    public BoxType getType()
    {
        return _type;
    }

    /// <summary>
    /// sets all the images color, including the path and background
    /// </summary>
    /// <param name="c">the color we want to set</param>
    public void setColor(Color c)
    {

        _figureImage.GetComponent<SpriteRenderer>().color = c;
        _animImage.GetComponent<SpriteRenderer>().color = c;
        _pathImage.GetComponent<SpriteRenderer>().color = c;
        setBackgroundColor(c);
    }

    public Color getColor()
    {
        if (_confirmedFlow != null) return _confirmedFlow.GetColor();
        else return new Color(-1, -1, -1);
    }

    public void setPathColor(Color c)
    {

        _pathImage.GetComponent<SpriteRenderer>().color = c;
    }

    public Color getPathColor()
    {
        return _pathImage.GetComponent<SpriteRenderer>().color;
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

    public void setFlow(Flow flow)
    {
        _flow = flow;
    }

    public Flow getFlow()
    {
        return _flow;
    }

    public Flow getConfirmedFlow()
    {
        return _confirmedFlow;
    }

    public void confirmFlow()
	{
        _confirmedFlow = _flow;
	}

    public void disconfirmFlow()
    {
        _confirmedFlow = null;
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

    public void setBackgroundColor(Color c)
    {
        c.a -= bgColorReduction;
        _backgroundImage.GetComponent<SpriteRenderer>().color = c;
    }

    public void setNode(LinkedListNode<GameBox> node)
	{
        _myNode = node;
    }

    public void setConfirmednode(LinkedListNode<GameBox> node)
	{
        _myConfirmedNode = node;
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
    }

    private BoxType _type;
    //Current flow dir
    private Vector2Int _flowDir;
    //Confirmed flow dir
    private Vector2Int _confirmedFlowDir;
    //A flow point always has Flow reference
    private Flow _flow;
    private Flow _confirmedFlow;
    private LinkedListNode<GameBox> _myNode;
    private LinkedListNode<GameBox> _myConfirmedNode;

    const float bgColorReduction = .35f;

    [SerializeField] private GameObject _backgroundImage;
    [SerializeField] private GameObject _figureImage;
    [SerializeField] private GameObject _pathImage;
    [SerializeField] private GameObject _animImage;
}

