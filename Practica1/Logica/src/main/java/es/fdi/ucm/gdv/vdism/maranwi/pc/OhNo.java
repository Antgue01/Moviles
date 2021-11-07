package es.fdi.ucm.gdv.vdism.maranwi.pc;

import java.awt.Menu;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Input;
/* DUDAS:
    Main de PC tiene un setApplication y luego un play, ¿correcto?
 */
/*
    Interfaces (Engine)
        - Graphics: ok
        - Inputs: ok
        - Image: ok
        - Application: ok
        - Font: todo
    Engines
        - Android: todo algunos metodos
        - Pc: todo algunos metodos
    Logica
        - Tablero    :  todo FALTA GENERACIÓN DE TABLERO
        - Celdas    :  ok
        - Pistas    : todo Falta terminar la 3
        - Renderer  : todo todo
    "Mains / Lanzadores"
        - Android: todo TODO
        - PC: ok
    Bucle principal: ok

 */

public class OhNo implements es.fdi.ucm.gdv.vdism.maranwi.engine.Application {

    @Override
    public void onInit() {
        _states = new GameState[2];
        _menu   = new MenuState();
        _states[0] = _menu;
        _game = new PlayState();
        _states[1] = _game;
        _currentState = 1;

        _states[1].onInit();
        //lanzar el menu
        //recoger
        //MOVERLO DE AQUÍ

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
        _states[_currentState].onEvent(input);
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

    GameState _states[];
    int _currentState;
    MenuState _menu;
    PlayState _game;
}