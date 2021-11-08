package es.fdi.ucm.gdv.vdism.maranwi.pcengine;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Engine;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Input;

public class PCEngine implements Engine {

    private void Init(){
        _input = new PCInput();
        _graphics = new PCGraphics(_myAppName, _myGame.getLogicWidth(), _myGame.getLogicHeight());
        _graphics.addMouseListener(_input);
        _graphics.addMouseMotionListener(_input);
        _myGame.onInit(_graphics);
    }

    public PCEngine(){}

    public void SetApplication(String applicationName, Application a){
        _myAppName = applicationName;
        _myGame = a;
    }

    public void Play(){
        Init();
        //hace deltatime

        float deltaTime = 0.0f;
        boolean play = true;
        while(play){
            _graphics.adjustToScreen(_myGame);
            _myGame.onInput(_input);
            _myGame.onUpdate(deltaTime);
            _graphics.draw(_myGame);
            if(!_myGame.onExit()) play = false;
        }
        _myGame.onRelease();
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
