package es.fdi.ucm.gdv.vdism.maranwi.engine;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;

public interface GameState {
    public void start(Graphics g);
    public void render(Graphics g);
    public void update(float deltaTime);
    public void identifyEvent(int x, int y);
    public void setMainApplicaton(Application a);
}
