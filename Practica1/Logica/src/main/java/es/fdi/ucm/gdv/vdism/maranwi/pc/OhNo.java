package es.fdi.ucm.gdv.vdism.maranwi.pc;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Input;

public class OhNo implements es.fdi.ucm.gdv.vdism.maranwi.engine.Application {

    @Override
    public void onInit() {
        _myTablero=new Tablero();
        _myRenderer=new UIRenderer(_myTablero);
        _myTablero.rellenaMatrizResueltaRandom();
    }

    @Override
    public void onDestroy() {

    }

    @Override
    public void onInput(Input input) {

    }

    @Override
    public void onUpdate(float deltaTime) {
        _myTablero.compruebaSolucion();
    }

    @Override
    public void onRender(Graphics graphics) {
        _myRenderer.render(graphics);
    }
    Tablero _myTablero;
    UIRenderer _myRenderer;
}