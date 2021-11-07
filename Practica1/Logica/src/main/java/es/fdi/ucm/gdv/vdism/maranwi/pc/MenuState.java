package es.fdi.ucm.gdv.vdism.maranwi.pc;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.GameState;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Input;

public class MenuState implements GameState {
    @Override
    public void start(Graphics g) {
        _font = "JosefinSans-Bold";
        _fontSize = 48;
        g.newFont(_font + ".ttf", _font, _fontSize, false);
    }

    @Override
    public void render(Graphics g) {
        g.setFont(_font);
        //g.drawText();
    }

    @Override
    public void update(float deltaTime) {

    }


    @Override
    public void identifyEvent(int x, int y) {

    }

    @Override
    public void setMainApplicaton(Application a) {
        _mainApp = a;
    }

    private Application _mainApp;
    private String _font;
    private int _fontSize;
    private boolean _onMainMenu;
}
