package es.fdi.ucm.gdv.vdism.maranwi.pcengine;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Image;

public class PCImage implements Image {

    public PCImage(int width, int height, java.awt.Image img){
        _width = width;
        _height = height;
        _awtImage = img;
    }
    @Override
    public int getWidth() {
        return _width;
    }

    @Override
    public int getHeigth() { return _height; }

    public java.awt.Image getAwtImage() { return _awtImage; }

    private int _width;
    private int _height;
    private java.awt.Image _awtImage;
}
