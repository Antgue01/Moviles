package es.fdi.ucm.gdv.vdism.maranwi.androidengine;

import android.content.Context;
import android.content.res.AssetManager;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.Paint;
import android.graphics.Rect;
import android.graphics.Typeface;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Font;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Image;

import android.os.Build;
import android.view.SurfaceHolder;
import android.view.SurfaceView;

import androidx.annotation.RequiresApi;

import java.io.InputStream;
import java.util.HashMap;
import java.util.HashSet;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;

public class AndroidGraphics implements Graphics {
    public AndroidGraphics(Context context, AssetManager assets) {
        _view = new SurfaceView(context);
        _holder = _view.getHolder();
        _paint = new Paint();
        _assets = assets;
        _fonts = new HashMap<String, Typeface>();

    }

    public void draw(Application app) {
        while (!_holder.getSurface().isValid())
            //Espera activa
            ;
        _canvas = _holder.lockCanvas();
        app.onRender(this);
        _holder.unlockCanvasAndPost(_canvas);
    }

    @Override
    public Image newImage(String name) {
        Bitmap sprite = null;
        //esto debería cerrar el archivo si fallara al abrir
        try (InputStream is = _assets.open(name)) {
            sprite = BitmapFactory.decodeStream(is);
        } catch (Exception e) {
            e.printStackTrace();
        }
        return sprite != null ? new AndroidImage(sprite.getWidth(), sprite.getHeight(), sprite) : null;

    }

    @Override
    public Font newFont(String filename, int size, boolean isBold) {
        Typeface font = _fonts.get(filename);
        if (font == null) {
            font = Typeface.createFromAsset(_assets, filename);
            _fonts.put(filename, font);
        }
        AndroidFont newFont = new AndroidFont(filename, size, isBold, font);
        return newFont;
    }


    @Override
    public void clear(int color) {

        _canvas.drawColor(0xFF | color);
    }

    @Override
    public void translate(double x, double y) {
        _canvas.translate((float) x, (float) y);
    }

    @Override
    public void scale(double x, double y) {
        _canvas.scale((float) x, (float) y);
    }

    ///If width or height are -1 then it will draw the image with its original width or height
    @Override
    public void drawImage(Image img, int x, int y, int width, int height) {
        if (img != null) {
        }

        Bitmap sprite = ((AndroidImage) (img)).getBitmap();
        Rect dest = new Rect(x, y, width, height);
        Rect src = new Rect(0, 0, sprite.getWidth(), sprite.getHeight());
        _canvas.drawBitmap(sprite, src, dest, _paint);


    }


    @Override
    public void save() {

    }

    @Override
    public void restore() {

    }

    @Override
    public void setColor(int color) {
        _paint.setColor(0xFF | color);
    }

    @RequiresApi(api = Build.VERSION_CODES.LOLLIPOP)
    @Override
    public void fillCircle(int cx, int cy, int r) {
        _canvas.drawOval(cx - r, cy - r, cx + r, cy + r, _paint);
    }

    @Override
    public void drawText(String text, int x, int y) {
        _canvas.drawText(text, x, y, _paint);
    }

    @Override
    public int getWindowsWidth() {
        return 0;
    }

    @Override
    public int getWindowsHeight() {
        return 0;
    }

    @Override
    public int getCanvasWidth() {
        return 0;
    }

    @Override
    public int getCanvasHeight() {
        return 0;
    }


    @Override
    public void setFont(Font font) {
        Typeface typeface = _fonts.get(font.getId());
        if (typeface != null) {
            _paint.setTypeface(typeface);
            _paint.setTextSize(font.getSize());
            _paint.setFakeBoldText(font.getIsBold());
        }
    }

    public SurfaceView getView() {
        return _view;
    }

    private HashMap<String, Typeface> _fonts;
    private SurfaceView _view;
    private SurfaceHolder _holder;
    private Canvas _canvas;
    private Paint _paint;
    private AssetManager _assets;
    private int _logicWidth;
    private int _logicHeight;
}
