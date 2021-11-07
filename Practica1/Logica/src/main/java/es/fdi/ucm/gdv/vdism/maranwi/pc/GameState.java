package es.fdi.ucm.gdv.vdism.maranwi.pc;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Input;

public interface GameState {
    public void onInit();
    public void onRender(Graphics graphics);
    public void onUpdate(float deltaTime);
    public void identifyEvent(int x, int y);
    public void setApplication(Application a);
}
