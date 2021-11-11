package es.fdi.ucm.gdv.vdism.maranwi.practica1;

import android.content.Context;
import android.content.res.AssetManager;
import android.view.SurfaceView;

import es.fdi.ucm.gdv.vdism.maranwi.androidengine.AndroidEngine;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;

public class ExecutionThreadManager {

    public ExecutionThreadManager(Application app, Context context,  AssetManager assets) {
        _engine = new AndroidEngine(app, context,assets);
    }

    public void resume() {
        if (_engine != null && !_engine.getPlay()) {
            _thread = new Thread(_engine);
           System.out.println("EL THREAD ES "+_thread);
            _thread.start();
        }
    }

    public void pause() {
        if (_engine != null && _engine.getPlay()) {
            _engine.stop();
            while (true) {
                try {
                    //esperamos al ultimo frame antes de cerrar

                    _thread.join();
                    _thread = null;
                    break;
                } catch (InterruptedException ie) {
                    // Esto no debería ocurrir nunca...
                }
            }
        }

    }
    public AndroidEngine getEngine(){
        return _engine;
    }
    private AndroidEngine _engine;
    private Thread _thread;
}
