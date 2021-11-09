package es.fdi.ucm.gdv.vdism.maranwi.pc;

import org.graalvm.compiler.nodes.extended.PluginFactory_RawLoadNode;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Font;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Image;
import sun.util.resources.cldr.ext.CurrencyNames_te;

public class Interact {
    public Interact(String id, int baseColor, int x, int y, int rad, int boardX, int boardY){
        _id = id;
        _baseColor = baseColor;
        _xPos = x;
        _yPos = y;
        _radius = rad;
        _boardX = boardX;
        _boardY = boardY;
        _hasText = false;
        _hasImg = false;
    }

    public void setImage(Image img){
        _image = img;
        _hasImg = true;
    }
    public void setText(String text, Font font, int fontColor){
        _text = text;
        _hasText = true;
        _font = font;
        _fontColor = fontColor;
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

    public int getBoardX() { return _boardX; }

    public int getBoardY() { return _boardY; }

    public void setBaseColor(int bC) { _baseColor = bC; }


    public void render(Graphics g){
        g.setColor(_baseColor);
        g.fillCircle(_xPos , _yPos , _radius);
        if(_hasText){
            if(_font != null) g.setFont(_font);
            g.setColor(_fontColor);
            if(_text.length() > 2)
                g.drawText(_text, _xPos + (_radius / 2) - ((_font.getSize() / 2) + 10), _yPos + (_radius / 2) + (_font.getSize()/4));
            else
                g.drawText(_text, _xPos + (_radius / 2) - _font.getSize() / 4, _yPos + (_radius / 2) + _font.getSize() / 4);
        }
        if(_hasImg){
            g.drawImage(_image, _xPos + (_radius / 2), _yPos + (_radius / 2), _image.getWidth(), _image.getHeigth());
        }
    }

    private String _id;

    private String _text;
    private Font _font;
    private int _fontColor;
    private boolean _hasText;

    private Image _image;
    private boolean _hasImg;

    private int _radius;
    private int _xPos;
    private int _yPos;
    private int _baseColor;

    private int _boardX;
    private int _boardY;
}
