package es.fdi.ucm.gdv.vdism.maranwi.engine;

public interface GameState {
    void start(Graphics g);
    void render(Graphics g);
    void update(double deltaTime);
    void identifyEvent(int x, int y);
    void setMainApplicaton(Application a);
}
