package es.fdi.ucm.gdv.vdism.maranwi.androidengine;

import android.content.res.AssetManager;
import android.graphics.Typeface;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Font;

public class AndroidFont implements Font {

    public AndroidFont(String path, int size, boolean isBold, AssetManager assetManager) {
        _size = size;
        _isBold = isBold;

        _font = Typeface.createFromAsset(assetManager, path);
    }

    @Override
    public int getSize() {
        return _size;
    }

    @Override
    public boolean getIsBold() {
        return _isBold;
    }

    public Typeface getAndroidFont() {
        return _font;
    }

    private int _size;
    private boolean _isBold;
    private Typeface _font;
}
