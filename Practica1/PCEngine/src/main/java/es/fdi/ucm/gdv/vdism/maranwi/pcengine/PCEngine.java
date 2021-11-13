package es.fdi.ucm.gdv.vdism.maranwi.pcengine;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Engine;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Input;

public class PCEngine implements Engine {

    public PCEngine(Application a, String applicationName){
        _myAppName = applicationName;
        _myGame = a;
        init();
    }

    private void init(){
        _input = new PCInput();
        _graphics = new PCGraphics(_myAppName, _myGame.getLogicWidth(), _myGame.getLogicHeight());
        _graphics.addMouseListener(_input);
        _graphics.addMouseMotionListener(_input);
        _myGame.onInit(this);
    }


    public void engineLoop(){
        boolean play = true;

        long lastFrameTime = System.nanoTime();

        while(play){
            long currentTime = System.nanoTime();
            long nanoElapsedTime = currentTime - lastFrameTime;
            lastFrameTime = currentTime;
            double elapsedTime = (double) nanoElapsedTime / 1.0E9;

            _myGame.onInput(_input);
            _myGame.onUpdate(elapsedTime);
            _graphics.draw(_myGame);

            if (_graphics.getClosed()){
                _myGame.onExit();
                play = false;
            }
        }

        release();
    }

    private void release(){
        _myGame.onRelease();
        _myGame = null;
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

    String _myAppName;
    Application _myGame;
    PCGraphics _graphics;
    PCInput _input;
}
