package es.fdi.ucm.gdv.vdism.maranwi.pc;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;

public class Interact {
    public Interact(String id, int baseColor, int x, int y, int rad, int boardW, int boardH){
        _id = id;
        _baseColor = baseColor;
        _xPos = x;
        _yPos = y;
        _radius = rad;
        _boardWidth = boardW;
        _boardHeight = boardH;
        _hasText = false;
    }

    public void setText(String text, String font, int fontColor, int fontSize){
        _text = text;
        _hasText = true;
        _font = font;
        _fontColor = fontColor;
        _fontSize = fontSize;
    }
    public String getId(){ return _id; }

    public int getRadius() {
        return _radius;
    }

    public int getXPos() {
        return _xPos;
    }

    public int getYPos() {
        return _yPos;
    }

    public int getBoardWidth() { return _boardWidth; }

    public int getBoardHeight() { return _boardHeight; }



    public void render(Graphics g){
        g.setColor(_baseColor);
        //g.fillCircle(_xPos - (_radius / 2), _yPos - (_radius / 2), _radius);
        g.fillCircle(_xPos , _yPos , _radius);
        if(_hasText){
            if(_font !="") g.setFont(_font);
            g.setColor(_fontColor);
            g.drawText(_text, _xPos + (_radius / 2) - ((_fontSize / 2)), _yPos + (_radius / 2) + (_fontSize/4));
        }
    }

    private String _id;

    private String _text;
    private String _font;
    private int _fontColor;
    private int _fontSize;
    private boolean _hasText;

    private int _radius;
    private int _xPos;
    private int _yPos;
    private int _baseColor;

    private int _boardWidth;
    private int _boardHeight;
}
