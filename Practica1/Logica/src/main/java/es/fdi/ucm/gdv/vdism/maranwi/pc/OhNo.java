package es.fdi.ucm.gdv.vdism.maranwi.pc;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Input;
/*
    Interfaces (Engine)
        - Graphics: ok
        - Inputs: ok
        - Image: ok
        - Application: ok
        - Font: todo
    Engines: todo hacer deltatime
        - Android: todo algunos metodos
        - Pc: todo algunos metodos
    Logica
        - Tablero    :  todo FALTA GENERACIÓN DE TABLERO
        - Celdas    :  ok
        - Pistas    : todo Falta terminar la 3
        - Renderer  : todo solucionar bug de reescalamiento
        - States: todo terminar botones y states
    "Mains / Lanzadores"
        - Android: todo TODO
        - PC: ok
    Bucle principal: ok

 */

public class OhNo implements es.fdi.ucm.gdv.vdism.maranwi.engine.Application {

    @Override
    public void onInit() {
        _states = new Application[2];
        _currentState = 0;

        _menu   = new MenuState();
        _menu.setMainApplicaton(this);
        _states[0] = _menu;

        _game = new PlayState();
        _game.setMainApplicaton(this);

        _states[1] = _game;

        _states[1].onInit();
    }

    @Override
    public boolean onExit() {
        return true;
    }

    @Override
    public void onRelease() {

    }

    @Override
    public void onInput(Input input) {
        for (Input.TouchEvent t : input.getTouchEvents()) {
            if (t != null && t.get_type() == Input.TouchEvent.TouchType.pulsacion){
                double xLeftLimitBox  = (_windowsWidth / 2) - (_boxGameWidth / 2);
                double xRightLimitBox  = (_windowsWidth / 2) + (_boxGameWidth / 2);
                double yTopLimitBox = (_windowsHeigth / 2) - (_boxGameHeight / 2);
                double yBottomLimitBox = (_windowsHeigth / 2) + (_boxGameHeight / 2);

                if(t.get_posX() >= xLeftLimitBox && t.get_posX() <= xRightLimitBox &&
                   t.get_posY() >= yTopLimitBox && t.get_posY() <= yBottomLimitBox){
                        double xInput = t.get_posX() - xLeftLimitBox;
                        double yInput = t.get_posY() - yTopLimitBox;
                        System.out.println("Event x :" + xInput + " Event y: " + yInput);
                        //System.out.println("Event x :" + t.get_posX() + " Event y: " + t.get_posY());
                }
            }
        }
    }

    @Override
    public void onUpdate(float deltaTime) {
        _states[_currentState].onUpdate(deltaTime);
    }

    @Override
    public void onRender(Graphics graphics) {
        graphics.clear(0x008800);
        _states[_currentState].onRender(graphics);
    }

    public void changeState(int state){
        if(state >= 0 && state < _states.length)
            _currentState = state;
    }

    public void setGameZone(double width, double height, double windowsWidth, double windowsHeigth){
        _boxGameWidth = width;
        _windowsWidth = windowsWidth;

        _boxGameHeight = height;
        _windowsHeigth = windowsHeigth;
    }

    Application _states[];
    int _currentState;
    MenuState _menu;
    PlayState _game;
    double _boxGameWidth;
    double _boxGameHeight;
    double _windowsWidth;
    double _windowsHeigth;
}