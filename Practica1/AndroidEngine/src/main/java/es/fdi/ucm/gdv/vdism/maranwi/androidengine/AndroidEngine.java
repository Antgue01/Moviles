package es.fdi.ucm.gdv.vdism.maranwi.androidengine;
import android.content.Context;
import android.content.res.AssetManager;
import android.view.SurfaceView;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Engine;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Input;

public class AndroidEngine implements Engine, Runnable {
    public AndroidEngine(Application app, Context context, AssetManager assets) {
        _app = app;

        _play = false;
        _graphics = new AndroidGraphics(context,assets,_app.getLogicWidth(),_app.getLogicHeight());
        _input = new AndroidInput();
        _graphics.setTouchListener(_input);
        _lastFrameTime=0;
        System.out.println("DELTA TIME INICIALIZADO");
        _app.onInit(this);
    }

    @Override
    public Graphics getGraphics() {
        return _graphics;
    }

    @Override
    public Input getInput() {
        return _input;
    }

    @Override
    public void run() {

        _play=true;
        System.out.println("EMPIEZO A CORRER EL BUCLE");
        // Antes de saltar a la simulación, confirmamos que tenemos
        // un tamaño mayor que 0. Si la hebra se pone en marcha
        // muy rápido, la vista podría todavía no estar inicializada.
        while(_play && _graphics.getWindowsWidth() == 0)
            // Espera activa. Sería más elegante al menos dormir un poco.

            ;
        while (_play) {
            long currentTime = System.nanoTime();
            long nanoElapsedTime = currentTime - _lastFrameTime;
            _lastFrameTime = currentTime;
            double elapsedTime = (double) nanoElapsedTime / 1.0E9;

            //if(eventoReescaladoJava)

            _app.onInput(_input);
            _app.onUpdate(elapsedTime);
            _graphics.draw(_app);
            //_graphics.adjustToScreen(_app);
        }
    }

    public void stop() {
        _play = false;
    }

    public boolean getPlay() {
        return _play;
    }

    volatile boolean _play;
    long _lastFrameTime;
    Application _app;
    AndroidGraphics _graphics;
    AndroidInput _input;

}
