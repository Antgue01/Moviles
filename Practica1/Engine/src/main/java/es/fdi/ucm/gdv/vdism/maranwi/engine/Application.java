package es.fdi.ucm.gdv.vdism.maranwi.engine;

public interface Application {
    void onInit();
    void onDestroy();
    void onInput(Input input);
    void onUpdate(float deltaTime);
    void onRender(Graphics graphics);

}
