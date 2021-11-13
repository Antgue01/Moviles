package es.fdi.ucm.gdv.vdism.maranwi.pcengine;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Engine;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Input;

public class PCEngine implements Engine {

    public PCEngine(Application a, String applicationName){
        _myApp = a;
        _input = new PCInput();
        _graphics = new PCGraphics(applicationName, _myApp.getLogicWidth(), _myApp.getLogicHeight());

        _graphics.addMouseListener(_input);
        _graphics.addMouseMotionListener(_input);
    }

    public void engineLoop(){
        _running = true;

        long lastFrameTime = System.nanoTime();

        _myApp.onInit(this);
        //MAIN LOOP
        while(_running){
            long currentTime = System.nanoTime();
            long nanoElapsedTime = currentTime - lastFrameTime;
            lastFrameTime = currentTime;
            double elapsedTime = (double) nanoElapsedTime / 1.0E9;

            _myApp.onInput(_input);
            _myApp.onUpdate(elapsedTime);
            _graphics.draw(_myApp);

            if (_graphics.getClosed()){
                _myApp.onExit();
                _running = false;
            }
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
    PCGraphics _graphics;
    PCInput _input;

    private boolean _running = false;
}
