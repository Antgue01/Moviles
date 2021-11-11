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
        _hasBottomCircle = true;
    }

    public void render(Graphics g){
        if(_hasBottomCircle){
            g.setColor(_baseColor);
            g.fillCircle(_xPos , _yPos , _radius);
        }
        if(_hasText){
            if(_font != null) g.setFont(_font);
            g.setColor(_fontColor);
            if(_text.length() > 2)
                g.drawText(_text, _xPos + (_radius / 2) - ((_font.getSize() / 2) + 10), _yPos + (_radius / 2) + (_font.getSize()/4));
            else
                g.drawText(_text, _xPos + (_radius / 2) - _font.getSize() / 4, _yPos + (_radius / 2) + _font.getSize() / 4);
        }
        if(_hasImg && _showImg){
            g.drawImage(_image, _xPos + (_radius / 2) - (_imageWidth / 2), _yPos + (_radius / 2) - (_imageHeight / 2),
                    _imageWidth, _imageHeight);
        }
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
    public void setBottomCircle(boolean b){ _hasBottomCircle = false; }
    public void setShowImg(boolean sImg) { _showImg = sImg; }

    public void setImage(Image img, int imgW, int imgH, boolean showImg){
        _image = img;
        _imageWidth = imgW;
        _imageHeight = imgH;
        _showImg = showImg;
        _hasImg = true;
    }

    public void setText(String text, Font font, int fontColor){
        _text = text;
        _hasText = true;
        _font = font;
        _fontColor = fontColor;
    }

    private String _id;

    private String _text;
    private Font _font;
    private int _fontColor;
    private boolean _hasText;

    private Image _image;
    private int _imageWidth;
    private int _imageHeight;
    private boolean _hasImg;
    private boolean _hasAnimation;
    private boolean _showImg;

    private int _radius;
    private int _xPos;
    private int _yPos;
    private int _baseColor;
    private boolean _hasBottomCircle;

    private int _boardX;
    private int _boardY;
}
