package es.fdi.ucm.gdv.vdism.maranwi.engine;

public interface Application {
    void onInit();
    boolean onExit();
    void onRelease();
    void onInput(Input input);
    void onUpdate(float deltaTime);
    void onRender(Graphics graphics);
    void setGameZone(double width, double height, double windowsWidth, double windowsHeigth);
}
