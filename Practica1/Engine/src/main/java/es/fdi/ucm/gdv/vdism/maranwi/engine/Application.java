package es.fdi.ucm.gdv.vdism.maranwi.engine;

public interface Application {
    void onInit(Engine engine);
    void onInput(Input input);
    void onUpdate(double deltaTime);
    void onRender(Graphics graphics);
    void onExit();
    void onRelease();
    int getLogicWidth();
    int getLogicHeight();
    int getBackgroundColor();
}
