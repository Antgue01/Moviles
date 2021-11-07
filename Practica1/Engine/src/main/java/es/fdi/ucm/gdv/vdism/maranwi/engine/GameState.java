package es.fdi.ucm.gdv.vdism.maranwi.engine;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Input;

public interface GameState {
    public void identifyEvent(int x, int y);
    public void setMainApplicaton(Application a);
}
