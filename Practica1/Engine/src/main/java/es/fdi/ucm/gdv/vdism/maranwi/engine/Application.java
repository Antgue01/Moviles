package es.fdi.ucm.gdv.vdism.maranwi.engine;

public interface Application {
    public void onInit(Graphics graphics);
    public boolean onExit();
    public void onRelease();
    public void onInput(Input input);
    public void onUpdate(float deltaTime);
    public void onRender(Graphics graphics);
    public void setApplicationZone(double width, double height, double windowsWidth, double windowsHeigth);
    public int getLogicWidth();
    public int getLogicHeight();
}
