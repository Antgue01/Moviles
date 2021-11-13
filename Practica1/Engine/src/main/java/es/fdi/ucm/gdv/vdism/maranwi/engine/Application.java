package es.fdi.ucm.gdv.vdism.maranwi.engine;

public interface Application {
    public void onInit(Engine engine);
    public void onInput(Input input);
    public void onUpdate(double deltaTime);
    public void onRender(Graphics graphics);
    public void onExit();
    public void onRelease();
    public int getLogicWidth();
    public int getLogicHeight();
    public  int getBackgroundColor();
}
