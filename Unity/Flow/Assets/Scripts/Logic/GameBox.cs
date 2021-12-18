using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

public class GameBox : MonoBehaviour
{
    /// <summary>
    /// Non variable Tile Types
    /// </summary>
    public enum BoxType { Bridge, Hollow, FlowPoint, Empty }

    public void setType(BoxType t) { _type = t; }
    public void setConfirmedNode(LinkedListNode<GameBox> confirmedNode) { _myConfirmedNode = confirmedNode; }
    public BoxType getBoxType() { return _type; }

    /// <summary>
    /// Sets all the images color, including the path and background
    /// </summary>
    /// <param name="c">the color we want to set</param>
    public void setColor(Color c)
    {
        _figureImage.color = c;
        _animImage.color = c;
        _pathImage.color = c;
        setBackgroundColor(c);
    }

    public Color getColor()
    {
        if (_confirmedFlow != null) return _confirmedFlow.GetColor();
        else return new Color(-1, -1, -1);
    }

    public void setPathColor(Color c) { _pathImage.color = c; }

    public Color getPathColor() { return _pathImage.color; }
    public void setFlowDir(Vector2Int dir) { _flowDir = dir; }

    public Vector2Int getFlowDir() { return _flowDir; }

    public void setConfirmedFlowDir(Vector2Int dir) { _confirmedFlowDir = dir; }

    public Vector2Int getConfirmedFlowDir() { return _confirmedFlowDir; }

    public void setBackgroundActive(bool b) { _backgroundImage.enabled = b; }
    public void setPathActive(bool b) { _pathImage.enabled = b; }

    public void setStarActive(bool b) { _starImage.enabled = b; }

    public void setWallLeftActive(bool b) { _wallLeft.enabled = b; }

    public void setWallRightActive(bool b) { _wallRight.enabled = b; }

    public void setWallUpActive(bool b) { _wallUp.enabled = b; }

    public void setWallDownActive(bool b) { _wallDown.enabled = b; }

    public void initWallDirsAndColor()
    {
        _validDirs = new bool[4];
        for (int x = 0; x < _validDirs.Length; x++)
        {
            _validDirs[x] = true;
        }
        _sectionColor = GameManager.instance.getSelectedSection().themeColor;
        _sectionColor.r += _wallColorDelta;
        _sectionColor.g += _wallColorDelta;
        _sectionColor.b += _wallColorDelta;

        _wallLeft.color = _sectionColor;
        _wallRight.color = _sectionColor;
        _wallUp.color = _sectionColor;
        _wallDown.color = _sectionColor;
    }

    public bool isValidDir(Vector2Int dir)
    {
        bool isValid = true;
        switch (dir)
        {
            case Vector2Int v when v.Equals(Vector2Int.left):
                isValid = _validDirs[0];
                break;
            case Vector2Int v when v.Equals(Vector2Int.right):
                isValid = _validDirs[1];
                break;
            case Vector2Int v when v.Equals(Vector2Int.up):
                isValid = _validDirs[2];
                break;
            case Vector2Int v when v.Equals(Vector2Int.down):
                isValid = _validDirs[3];
                break;
        }
        return isValid;
    }

    public void setInvalidDir(int tileDiff)
    {
        if (tileDiff == -1) //We are goint to the Left Game Box
        {
            _validDirs[0] = false;
        }
        else if (tileDiff == 1) //We are goint to the Right Game Box
        {
            _validDirs[1] = false;
        }
        else if (tileDiff < -1) //We are goint to the Up Game Box
        {
            _validDirs[2] = false;
        }
        else if (tileDiff > 1) //We are goint to the Down Game Box
        {
            _validDirs[3] = false;
        }
    }

    public void setWallActive(int tileDiff)
    {
        if (tileDiff == -1) //Left Wall
        {
            _wallLeft.enabled = true;
        }
        else if (tileDiff == 1) //Right Wall
        {
            _wallRight.enabled = true;
        }
        else if (tileDiff < -1) //Up wall
        {
            _wallUp.enabled = true;
        }
        else if (tileDiff > 1) //Down Wall
        {
            _wallDown.enabled = true;
        }
    }

    public void setActiveAllWalls(bool b)
    {
        _wallLeft.enabled = b;
        _wallRight.enabled = b;
        _wallUp.enabled = b;
        _wallDown.enabled = b;
    }

    public void setFigureSprite(Sprite s)
    {
        if (s == null)
        {
            _figureImage.enabled = false;
            _animImage.enabled = false;
        }
        else
        {
            if (!_animImage.enabled)
                _animImage.enabled = true;
            _animImage.sprite = s;

            if (!_figureImage.enabled)
                _figureImage.enabled = true;
            _figureImage.sprite = s;
        }
    }

    public void setFlow(Flow flow) { _flow = flow; }

    public Flow getFlow() { return _flow; }

    public Flow getConfirmedFlow() { return _confirmedFlow; }

    public void confirmFlow() { _confirmedFlow = _flow; }

    public void disconfirmFlow() { _confirmedFlow = null; }

    public bool getPathActive() { return _pathImage.enabled; }
    public bool getStarActive() { return _starImage.enabled; }

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
        _backgroundImage.color = c;
    }

    public void setNode(LinkedListNode<GameBox> node) { _myNode = node; }

    public void setConfirmednode(LinkedListNode<GameBox> node) { _myConfirmedNode = node; }

    public LinkedListNode<GameBox> getNode() { return _myNode; }

    public LinkedListNode<GameBox> getConfirmedNode() { return _myConfirmedNode; }

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
    private bool[] _validDirs;
    private Color _sectionColor;
    const float bgColorReduction = .35f;

    [SerializeField] private SpriteRenderer _backgroundImage;
    [SerializeField] private SpriteRenderer _starImage;
    [SerializeField] private SpriteRenderer _wallLeft;
    [SerializeField] private SpriteRenderer _wallRight;
    [SerializeField] private SpriteRenderer _wallUp;
    [SerializeField] private SpriteRenderer _wallDown;
    [SerializeField] private SpriteRenderer _figureImage;
    [SerializeField] private SpriteRenderer _pathImage;
    [SerializeField] private SpriteRenderer _animImage;
    [SerializeField] private float _wallColorDelta = 0.2f;
}

