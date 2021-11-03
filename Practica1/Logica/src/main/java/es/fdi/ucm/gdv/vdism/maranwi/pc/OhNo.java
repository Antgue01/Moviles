package es.fdi.ucm.gdv.vdism.maranwi.pc;

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
        - Pistas    : todo FALTA HACERLAS TODAS
        - Renderer  : todo todo
    "Mains / Lanzadores"
        - Android: todo TODO
        - PC: ok
    Bucle principal: ok

 */

public class OhNo implements es.fdi.ucm.gdv.vdism.maranwi.engine.Application {

    @Override
    public void onInit() {
        //lanzar el menu
        //recoger
        //MOVERLO DE AQUÍ
        _myTablero = new Tablero(4, 4, 400, 600);
        _myRenderer = new UIRenderer(_myTablero);
        _myTablero.rellenaMatrizResueltaRandom();
        _myPista = new Pista();
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
            if (t != null && t.get_type() == Input.TouchEvent.TouchType.pulsacion)
                System.out.println(t.get_posX() + " " + t.get_posY());
        }
    }

    @Override
    public void onUpdate(float deltaTime) {
        _myPista.Aplicar(_myTablero);
        _myTablero.compruebaSolucion();
    }

    @Override
    public void onRender(Graphics graphics) {
        graphics.clear(0x008800);
        _myRenderer.render(graphics);
    }

    Pista _myPista;
    Tablero _myTablero;
    UIRenderer _myRenderer;
}