package es.fdi.ucm.gdv.vdism.maranwi.pc;
/* Dudas DE CARA A LA ENTREGA:
     - Nuestro hilo de ejecución de Android, recibe el Engine como parámetro, esto provoca que varios hilos puedan tener a la vez la misma
         referencia al Engine y que consecuentemente se puedan hacer llamadas a los mismos métodos a la vez o que se estén modificando valores de atributos
         por sitios diferentes, ¿es esto correcto, habría que poner syncronze (métodos/atributos) o qué debemos hacer?
     - ¿Para qué sirve el id del input? No sabemos si los dedos que entran en los eventos están en orden de pulsación
     - ¿Debería parpadear la pantalla al hacer resize?
     - ¿Se tiene que hacer el resize el tiempo real o solo cuando """""se pare"""""" el ratón una vez iniciado el proceso de reescalado?
     - ¿Por qué cuando está reescalando a veces se pinta dos veces?
     - ¿Por qué peta en depuración? -> MOSTRAR FOTO
     - El escalado en android se comporta distinto que en pc, ya que en pc posconcatena las matrices. ¿Se puede emular este comportamiento
            en Android? ¿Cómo habría que adaptar el scale en caso contrario? (ya que no parece haber más métodos para ello)
     - Sobre la generación del tablero: Nosotros creamos el tablero inicial por un proceso en el que no intervienen las pistas, una vez creado, se ocultan
        las azules y aleatoriamente se deciden cuales de las rojas (las que no se oculten serán candados).
        Como confirmación, una vez ocultas las fichas se utilizan las pistas para verificar que da el mismo resultado (comprobación de solución única)
        ¿Es válido este proceso? Si no es así, ¿dónde lo estamos haciendo mal?
      - En nuestro proyecto, los motores son los encargados de, dado un tamaño lógico del juego y un posiconamiento de elementos en base a este tamaño lógico,
         hacer las traslaciones y escalas necesarias para que siempre estén los elementos en su sitio y con su tamaño correcto, consecuentemente la lógica
         no puede acceder a los métodos Translate y Escale ya que podría romper el trabajo que esté haciendo el motor. Dada esta situación el Save y Restore
         no se usan, ¿este planteamiento es correcto? De ser así, ¿podemos borrar los métodos Save y Restore? Y en caso contrario, ¿el trabajo del reposicionamiento
         debería hacerse en la lógica?
 */
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
        graphics.clear(0xFFFFFF);
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