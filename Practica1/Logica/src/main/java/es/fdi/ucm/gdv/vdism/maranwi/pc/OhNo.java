package es.fdi.ucm.gdv.vdism.maranwi.pc;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.GameState;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Engine;
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
    public void onInit(Engine engine) {
        _engine = engine;
        _graphics = _engine.getGraphics();
        goToMenuState();
        //goToPlayState(4,4);
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
                        _currentState.identifyEvent((int) xInput, (int) yInput);
                        //System.out.println("Event x :" + xInput + " Event y: " + yInput);
                        //System.out.println("Event x :" + t.get_posX() + " Event y: " + t.get_posY());
                }
            }
        }
    }

    @Override
    public void onUpdate(double deltaTime) {
        _currentState.update(deltaTime);
    }

    @Override
    public void onRender(Graphics graphics) {
        graphics.clear(0x008800);
        _currentState.render(graphics);
    }

    public void goToMenuState(){
        _currentState = new MenuState();
        _currentState.setMainApplicaton(this);
        _currentState.start(_graphics);
    }

    public void goToPlayState(int boardRows, int boardCols){
        PlayState game = new PlayState();
        game.setMainApplicaton(this);
        game.setBoardSize(boardRows, boardCols, BOX_LOGIC_WIDTH, BOX_LOGIC_HEIGHT);
        game.start(_graphics);
        _currentState = game;
    }

    public void setApplicationZone(double width, double height, double windowsWidth, double windowsHeigth){
        _boxGameWidth = width;
        _boxGameHeight = height;

        _windowsWidth = windowsWidth;
        _windowsHeigth = windowsHeigth;
    }

    @Override
    public int getLogicWidth() {
        return BOX_LOGIC_WIDTH;
    }

    @Override
    public int getLogicHeight() {
        return BOX_LOGIC_HEIGHT;
    }

    private Engine _engine;
    private Graphics _graphics;
    private GameState _currentState;
    private double _boxGameWidth;
    private double _boxGameHeight;
    private double _windowsWidth;
    private double _windowsHeigth;
    static private final int BOX_LOGIC_WIDTH = 400;
    static private final int BOX_LOGIC_HEIGHT = 600;
}