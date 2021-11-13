package es.fdi.ucm.gdv.vdism.maranwi.androidengine;
import android.content.Context;
import android.content.res.AssetManager;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Engine;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Input;

public class AndroidEngine implements Engine, Runnable {

    public AndroidEngine(Application app, Context context, AssetManager assets) {
        _myApp = app;

        _input = new AndroidInput();
        _graphics = new AndroidGraphics(context, assets, _myApp.getLogicWidth(), _myApp.getLogicHeight());

        _graphics.setTouchListener(_input);
    }

    public void resume() {
        if (!_running) {
            // Solo hacemos algo si no nos estábamos ejecutando ya
            // (programación defensiva, nunca se sabe quién va a
            // usarnos...)
            _running = true;
            // Lanzamos la ejecución de nuestro método run()
            // en una hebra nueva.
            _thread = new Thread(this);
            _thread.start();
        } // if (!_running)
    }

    public void pause() {

        if (_running) {
            _running = false;
            while (true) {
                try {
                    _thread.join();
                    _thread = null;
                    break;
                } catch (InterruptedException ie) {
                    // Esto no debería ocurrir nunca...
                }
            } // while(true)
        } // if (_running)

    }

    @Override
    public void run() {

        if (_thread != Thread.currentThread()) {
            // ¿¿Quién es el tuercebotas que está llamando al
            // run() directamente?? Programación defensiva
            // otra vez, con excepción, por merluzo.
            throw new RuntimeException("run() should not be called directly");
        }

        // Antes de saltar a la simulación, confirmamos que tenemos
        // un tamaño mayor que 0. Si la hebra se pone en marcha
        // muy rápido, la vista podría todavía no estar inicializada.
        while(_running && _graphics.getWindowsWidth() == 0)
            // Espera activa. Sería más elegante al menos dormir un poco.
            ;

        long lastFrameTime = System.nanoTime();

        _myApp.onInit(this);
        //MAIN LOOP
        while (_running) {
            long currentTime = System.nanoTime();
            long nanoElapsedTime = currentTime - lastFrameTime;
            lastFrameTime = currentTime;
            double elapsedTime = (double) nanoElapsedTime / 1.0E9;

            _myApp.onInput(_input);
            _myApp.onUpdate(elapsedTime);
            _graphics.draw(_myApp);
        }

        release();
    }

    private void release(){
        _myApp.onRelease();
        _myApp = null;
        _input = null;
        _graphics = null;
    }

    @Override
    public Graphics getGraphics() {
        return _graphics;
    }

    @Override
    public Input getInput() {
        return _input;
    }

    Application _myApp;
    AndroidGraphics _graphics;
    AndroidInput _input;

    volatile boolean _running = false;

    Thread _thread;
}