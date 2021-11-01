package es.fdi.ucm.gdv.vdism.maranwi.pcengine;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Engine;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Input;

public class PCEngine implements Engine {
    public PCEngine(){
        _graphics = new PCGraphics();
        _input = new PCInput();
    }

    public void SetApplication(Application a){
        _myGame = a;
    }

    public void Play(){
        _myGame.onInit();
        _graphics.addMouseListener(_input);
        _graphics.addMouseMotionListener(_input);
        //hace deltatime

        float deltaTime = 0.0f;
        boolean play = true;
        while(play){
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

    Application _myGame;
    PCGraphics _graphics;
    PCInput _input;
}
