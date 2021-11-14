package es.fdi.ucm.gdv.vdism.maranwi.logica;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Font;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Image;

public class Interact {
    public Interact(String id, MyColor baseColor, int x, int y, int rad, int boardX, int boardY, int offset) {
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
        _offset = offset;
    }

    public void render(Graphics g) {
        if (!_hasAnimation && _hasBottomCircle) {
            g.setColor(_baseColor);
            g.fillOval(_xPos , _yPos , _radius-_offset, _radius- _offset);
        }
        if (_hasText) {
            int offsetX = _offset > 0 ? (_radius / 2 - (_offset / 2)) : (_radius / 2) ;
            int offsetY = _offset > 0 ? (_radius / 2 - (_offset / 2)) : (_radius / 2) ;

            if (_font != null) g.setFont(_font);
            g.setColor(_fontColor);
            if (_text.length() > 2)
                g.drawText(_text, _xPos + offsetX -((_font.getSize() / 2) + 10), _yPos + offsetY + (_font.getSize() / 4));
            else
                g.drawText(_text, _xPos + offsetX - _font.getSize() / 4, _yPos +offsetY + _font.getSize() / 4);
        }
        if (_hasImg && _showImg) {
            int offsetX = _offset > 0 ? (_radius / 2 - (_offset / 2)) -(_imageWidth/2) : (_radius / 2) - (_imageWidth / 2);
            int offsetY = _offset > 0 ? (_radius / 2 - (_offset / 2)) -(_imageHeight/2): (_radius / 2) - (_imageHeight / 2);
            g.drawImage(_image, _xPos + offsetX, _yPos + offsetY,
                    _imageWidth, _imageHeight, _imageAlpha);
        }
    }

    public String getId() {
        return _id;
    }

    public int getRadius() {
        return _radius;
    }

    public int getXPos() {
        return _xPos;
    }

    public int getYPos() {
        return _yPos;
    }

    public int getBoardX() {
        return _boardX;
    }

    public int getBoardY() {
        return _boardY;
    }

    public int getOffset() {
        return _offset;
    }

    public void setBaseColor(MyColor bC) {
        _baseColor = bC;
    }

    public void setBottomCircle(boolean b) {
        _hasBottomCircle = false;
    }

    public void setShowImg(boolean s) {
        _showImg = s;
    }

    public void setHasAnimation(boolean a) {
        _hasAnimation = a;
    }

    public void setImage(Image img, int imgW, int imgH, boolean showImg, int alpha) {
        _image = img;
        _imageWidth = imgW;
        _imageHeight = imgH;
        _showImg = showImg;
        _hasImg = true;
        _imageAlpha = alpha;
    }

    public void setText(String text, Font font, MyColor fontColor) {
        _text = text;
        _hasText = true;
        _font = font;
        _fontColor = fontColor;
    }

    private String _id;

    private String _text;
    private Font _font;
    private MyColor _fontColor;
    private boolean _hasText;

    private Image _image;
    private int _imageWidth;
    private int _imageHeight;
    private boolean _hasImg;
    private int _imageAlpha;
    private boolean _hasAnimation;
    private boolean _showImg;

    private int _radius;
    private int _xPos;
    private int _yPos;
    private int _offset;
    private MyColor _baseColor;
    private boolean _hasBottomCircle;

    private int _boardX;
    private int _boardY;


}
