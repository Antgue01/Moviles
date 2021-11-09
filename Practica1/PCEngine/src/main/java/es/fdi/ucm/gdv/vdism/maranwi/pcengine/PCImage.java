package es.fdi.ucm.gdv.vdism.maranwi.pcengine;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Image;

public class PCImage implements Image {
    @Override
    public int getWidth() {
        return 0;
    }

    @Override
    public int getHeigth() {
        //_awtImage.get
        return 0;
    }

    public void setAwtImage(java.awt.Image img){
        _awtImage = img;
    }

    private java.awt.Image _awtImage;
}
