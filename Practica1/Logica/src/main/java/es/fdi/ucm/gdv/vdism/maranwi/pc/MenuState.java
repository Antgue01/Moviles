package es.fdi.ucm.gdv.vdism.maranwi.pc;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.GameState;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Input;

public class MenuState implements Application, GameState {
    @Override
    public void onInit() {

    }

    @Override
    public boolean onExit() {
        return false;
    }

    @Override
    public void onRelease() {

    }

    @Override
    public void onInput(Input input) {

    }

    @Override
    public void onRender(Graphics graphics) {

    }

    @Override
    public void setGameZone(double width, double height, double windowsWidth, double windowsHeigth) {

    }

    @Override
    public void onUpdate(float deltaTime) {

    }

    @Override
    public void identifyEvent(int x, int y) {

    }

    @Override
    public void setMainApplicaton(Application a) {
        _mainApp = a;
    }

    Application _mainApp;
}
