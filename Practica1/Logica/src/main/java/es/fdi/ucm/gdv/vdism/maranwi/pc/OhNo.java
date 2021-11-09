package es.fdi.ucm.gdv.vdism.maranwi.pc;
/* Dudas:
     - ¿Hacer las llamadas a escale y translate desde lógica o dejar que lo haga el motor cuando sea conveniente?
     -¿El engine de Android tiene que estar en un hilo (implementar runnable) o tener un hilo en el que se ejecute el bucle principal?
     -¿Para qué sirve el id del input?
     -¿Debería parpaddear la pantalla al hacer resize?
     -¿Por qué el height y el width no se centran bien a pesar de que los cálculos son correctos?
 */
import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.GameState;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Engine;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Input;
import sun.util.resources.cldr.ext.CurrencyNames_en_NG;
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
        goToMenuState();
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
                int windowsWidth = _engine.getGraphics().getWindowsWidth();
                int windowsHeigth = _engine.getGraphics().getWindowsHeight();
                int canvasWidth = _engine.getGraphics().getCanvasWidth();
                int canvasHeight = _engine.getGraphics().getCanvasHeight();
                double xLeftLimitBox  = (windowsWidth / 2) - (canvasWidth / 2);
                double xRightLimitBox  = (windowsWidth / 2) + (canvasWidth / 2);
                double yTopLimitBox = (windowsHeigth / 2) - (canvasHeight / 2);
                double yBottomLimitBox = (windowsHeigth / 2) + (canvasHeight / 2);

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
        _currentState.start(_engine.getGraphics());
    }

    public void goToPlayState(int boardRows, int boardCols){
        PlayState game = new PlayState();
        game.setMainApplicaton(this);
        game.setBoardSize(boardRows, boardCols);
        game.start(_engine.getGraphics());
        _currentState = game;
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
    private GameState _currentState;
    static private final int BOX_LOGIC_WIDTH = 400;
    static private final int BOX_LOGIC_HEIGHT = 600;
}