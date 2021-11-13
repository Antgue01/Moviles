package es.fdi.ucm.gdv.vdism.maranwi.logica;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;

public interface GameState {
    void start(Graphics g);
    void render(Graphics g);
    void update(double deltaTime);
    void identifyEvent(int x, int y);
    void setMainApplicaton(Application a);
}
