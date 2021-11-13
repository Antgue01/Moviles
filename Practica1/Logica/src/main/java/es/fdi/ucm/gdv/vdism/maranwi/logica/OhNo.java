package es.fdi.ucm.gdv.vdism.maranwi.logica;
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
        - Font: ok
    Engines: ok
        - Android: todo algunos metodos
        - Pc: ok
    Logica
        - Tablero    :  todo FALTA GENERACIÓN DE TABLERO
        - Celdas    :  ok
        - Pistas    : todo Falta terminar la 3
        - Renderer  : ok
        - States: todo Hints -> posicionamiento texto + Imágenes
    "Mains / Lanzadores"
        - Android: todo algunas cosas
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
    public void onExit() {

    }

    @Override
    public void onRelease() {

    }

    @Override
    public void onInput(Input input) {
        for (Input.TouchEvent t : input.getTouchEvents()) {
            if (t != null && t.get_type() == Input.TouchEvent.TouchType.pulsacion){
                double windowsWidth = _engine.getGraphics().getWindowsWidth();
                double windowsHeigth = _engine.getGraphics().getWindowsHeight();
                double canvasWidth = _engine.getGraphics().getCanvasWidth();
                double canvasHeight = _engine.getGraphics().getCanvasHeight();
                double xLeftLimitBox  = (windowsWidth - canvasWidth) / 2;
                double xRightLimitBox  = xLeftLimitBox + canvasWidth;
                double yTopLimitBox = (windowsHeigth - canvasHeight) / 2;
                double yBottomLimitBox = yTopLimitBox + canvasHeight;

                //Solo queremos el input dentro del canvas (sin las bandas)
                if(t.get_posX() >= xLeftLimitBox && t.get_posX() <= xRightLimitBox &&
                   t.get_posY() >= yTopLimitBox && t.get_posY() <= yBottomLimitBox){
                        double xInput = t.get_posX() - xLeftLimitBox;
                        double yInput = t.get_posY() - yTopLimitBox;
                        double scaleFactorX =  BOX_LOGIC_WIDTH  / canvasWidth;
                        double scaleFactorY =  BOX_LOGIC_HEIGHT / canvasHeight;
                        //Lo pasamos a coordenadas logicas
                        xInput *= scaleFactorX;
                        yInput *= scaleFactorY;
                        _currentState.identifyEvent((int) xInput, (int) yInput);
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
        graphics.clear(0xFFFFFFFF);
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

    @Override
    public int getBackgroundColor() { return 0xEEEEEEFF; }

    private Engine _engine;
    private GameState _currentState;
    static private final int BOX_LOGIC_WIDTH = 400;
    static private final int BOX_LOGIC_HEIGHT = 600;
}