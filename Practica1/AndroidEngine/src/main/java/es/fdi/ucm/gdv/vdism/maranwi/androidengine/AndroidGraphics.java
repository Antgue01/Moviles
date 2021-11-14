package es.fdi.ucm.gdv.vdism.maranwi.androidengine;

import android.content.Context;
import android.content.res.AssetManager;
import android.graphics.Bitmap;
import android.graphics.BitmapFactory;
import android.graphics.Canvas;
import android.graphics.Paint;
import android.graphics.Rect;
import android.graphics.Typeface;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Color;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Font;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Image;

import android.os.Build;
import android.view.SurfaceHolder;
import android.view.SurfaceView;
import android.view.View;

import androidx.annotation.RequiresApi;

import java.io.InputStream;
import java.util.HashMap;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;

public class AndroidGraphics implements Graphics {
    public AndroidGraphics(Context context, AssetManager assets, int logicWidth, int logicHeight) {
        _view = new SurfaceView(context);
        _holder = _view.getHolder();
        _logicWidth = logicWidth;
        _logicHeight = logicHeight;
        _assets = assets;

        _paint = new Paint();
        _fonts = new HashMap<String, AndroidFont>();

        _adjustRequest = true;
    }

    /**
     * Pintado de frame
     * @param app
     */
    public void draw(Application app) {

        while (!_holder.getSurface().isValid())
            ;
        _canvas = _holder.lockCanvas();

        clearAll(app.getBackgroundColor());

        if(_adjustRequest){
            adjustToScreen();
            _adjustRequest = false;
        }
        translate(_translationX,_translationY);
        scale(_scaleX,_scaleY);

        app.onRender(this);

        _holder.unlockCanvasAndPost(_canvas);
    }

    @Override
    public Image newImage(String name) {

        Bitmap sprite = null;

        try (InputStream is = _assets.open("images/" + name)) {
            sprite = BitmapFactory.decodeStream(is);
        } catch (Exception e) {
            e.printStackTrace();
        }

        return sprite != null ? new AndroidImage(sprite.getWidth(), sprite.getHeight(), sprite) : null;
    }

    @Override
    public Font newFont(String filename, int size, boolean isBold) {
        String id = filename + size + isBold + "";
        AndroidFont font = _fonts.get(id);
        if (font == null) {
            font = new AndroidFont("fonts/" + filename, size, isBold, _assets);
            _fonts.put(id, font);
        }

        return font;
    }

    @Override
    public void clear(int rgba) {
        setColor(rgba);
        _canvas.drawRect(0, 0, _logicWidth, _logicHeight, _paint);
    }

    @Override
    public void clear(int r, int g, int b, int a) {
        setColor(r,g,b,a);
        _canvas.drawRect(0, 0, _logicWidth, _logicHeight, _paint);
    }

    @Override
    public void clear(Color color) {
        setColor(color);
        _canvas.drawRect(0, 0, _logicWidth, _logicHeight, _paint);
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
            Bitmap sprite = ((AndroidImage) (img)).getBitmap();
            Rect dest = new Rect(x, y, x + width, y + height);
            _canvas.drawBitmap(sprite, null, dest, _paint);
        }
    }

    @Override
    public void drawImage(Image img, int x, int y, int width, int height, int alpha) {
        if (img != null) {
            _paint.setAlpha(alpha);
            Bitmap sprite = ((AndroidImage) img).getBitmap();
            Rect dest = new Rect(x, y, x + width, y + height);
            _canvas.drawBitmap(sprite, null, dest, _paint);
            _paint.setAlpha(255);
        }
    }

    @Override
    public void setColor(int rgba) {
        int argb = (rgba & 0xFFFFFF00) >>> 8;
        int alpha = (rgba & 0x000000FF) << 24;
        argb = argb | alpha;

        _paint.setColor(argb);
    }

    @Override
    public void setColor(int r, int g, int b, int a) {
        if (r > -1 && r < 256 && g > -1 && g < 256 && b > -1 && b < 256 && a > -1 && a < 256) {
            _paint.setColor(android.graphics.Color.argb(a, r, g, b));
        }
    }

    @Override
    public void setColor(Color color) {
        int r = color.getRed();
        int b = color.getBlue();
        int g = color.getGreen();
        int a = color.getAlpha();
        if (r > -1 && r < 256 && g > -1 && g < 256 && b > -1 && b < 256 && a > -1 && a < 256) {
            _paint.setColor(android.graphics.Color.argb(a, r, g, b));
        }
    }

    @RequiresApi(api = Build.VERSION_CODES.LOLLIPOP)
    @Override
    public void fillOval(int cx, int cy, int rx, int ry) {
        _paint.setStyle(Paint.Style.FILL);
        _canvas.drawOval(cx, cy, cx + rx, cy + ry, _paint);
    }

    @RequiresApi(api = Build.VERSION_CODES.LOLLIPOP)
    @Override
    public void drawOval(int cx, int cy, int rx, int ry) {
        _paint.setStyle(Paint.Style.STROKE);
        _canvas.drawOval(cx, cy, cx + rx, cy + ry, _paint);
    }

    @Override
    public void drawText(String text, int x, int y) {
        String[] lines = text.split("\n");
        int verticalOffset = (int)_paint.descent() - (int)_paint.ascent();

        for (int i = 0; i<lines.length; ++i){
            _canvas.drawText(lines[i], x, y + (verticalOffset * i), _paint);
        }
    }

    @Override
    public int getWindowWidth() {
        return _view.getWidth();
    }

    @Override
    public int getWindowHeight() {
        return _view.getHeight();
    }

    @Override
    public int getCanvasWidth() {
        return (int) _canvasWidth;
    }

    @Override
    public int getCanvasHeight() {
        return (int) _canvasHeight;
    }


    @Override
    public void setFont(Font font) {
        Typeface typeface = ((AndroidFont) font).getAndroidFont();

        if (typeface != null) {
            _paint.setTypeface(typeface);
            _paint.setTextSize(font.getSize());
            _paint.setFakeBoldText(font.getIsBold());
        }
    }

    public void setTouchListener(View.OnTouchListener listener) {
        _view.setOnTouchListener(listener);
    }

    /**
     * Limpia el fondo entero con un color
     * @param color
     */
    private void clearAll(int color) {
        int argb = (color & 0xFFFFFF00) >>> 8;
        int alpha = (color & 0x000000FF) << 24;
        argb = argb | alpha;
        _canvas.drawColor(argb);
    }

    /**
     * Calculos para ajustar la translacion y escala
     */
    private void adjustToScreen() {
        int frameW = _view.getWidth();
        int frameH = _view.getHeight();
        //Hacemos la regla de tres para ver si cabría

        double newH = _logicHeight * frameW / _logicWidth;
        double newW = _logicWidth * frameH / _logicHeight;

        //Si escalando la Y no cabríamos

        if (newH >= frameH) {
            //Factor de escala
            _scaleY = frameH / (double) _logicHeight;
            _scaleX = _scaleY;
        } else if (newW >= frameW) {
            //Factor de escala
            _scaleX = frameW / (double) _logicWidth;
            _scaleY = _scaleX;
        }
        _canvasWidth = _scaleX * _logicWidth;
        _canvasHeight = _scaleY * _logicHeight;

        _translationX = ((double)frameW - _canvasWidth)/2;
        _translationY = ((double)frameH - _canvasHeight)/2;
    }

    public SurfaceView getView() {
        return _view;
    }

    double _translationX = 0;
    double _translationY = 0;
    double _scaleX = 1;
    double _scaleY = 1;
    private HashMap<String, AndroidFont> _fonts;
    private SurfaceView _view;
    private SurfaceHolder _holder;
    private Canvas _canvas;
    private Paint _paint;
    private AssetManager _assets;
    private int _logicWidth;
    private int _logicHeight;
    private double _canvasWidth;
    private double _canvasHeight;
    private boolean _adjustRequest;
}
