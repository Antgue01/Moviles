package es.fdi.ucm.gdv.vdism.maranwi.androidengine;

import android.graphics.Typeface;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Font;

public class AndroidFont implements Font {

    public AndroidFont(String id,int size, boolean bold, Typeface font) {
        _size = size;
        _isBold = bold;
        _font = font;
        _id=id;
    }

    @Override
    public String getId() {
        return _id;
    }

    public int getSize() {
        return _size;
    }

    public void setSize(int size) {
        if (size > 0)
            _size = size;
        else _size = 10;
    }

    public boolean getIsBold() {
        return _isBold;
    }

    public void setIsBold(boolean bold) {
        _isBold = bold;
    }

    public Typeface geFont() {
        return _font;
    }

    private int _size;
    private boolean _isBold;
    private Typeface _font;
    private String _id;
}
