package es.fdi.ucm.gdv.vdism.maranwi.androidengine;

import android.graphics.Bitmap;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Image;

public class AndroidImage implements Image {

    public  AndroidImage(int w, int h, Bitmap bitmap){
        _height=h;
        _width=w;
        _bitmap=bitmap;
    }



    @Override
    public int getWidth() {
        return _width;
    }

    @Override
    public int getHeigth() {
        return _height;
    }
    public  Bitmap getBitmap(){
        return  _bitmap;
    }
    int _height;
    int _width;
    Bitmap _bitmap;
}
