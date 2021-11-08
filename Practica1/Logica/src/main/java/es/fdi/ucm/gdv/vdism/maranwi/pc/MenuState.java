package es.fdi.ucm.gdv.vdism.maranwi.pc;

import java.awt.Font;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.GameState;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Input;

public class MenuState implements GameState {
    @Override
    public void start(Graphics g) {
        _font = "JosefinSans-Bold";
        _fontSize = 48;
        _fontColor = 0x000000;
        g.newFont(_font + ".ttf", _font, _fontSize, true);
        _onMainMenu = true;

        int xPos = BUTTON_RAD / 2, yPos = BUTTON_RAD;
        _buttons = new Interact[5];
        _buttons[0] = new Interact("Play", 0xFFFFFF, (g.getWidth()/2) - BUTTON_RAD, (g.getHeight()/2) - BUTTON_RAD, BUTTON_RAD * 2, 0, 0);
        _buttons[0].setText("Start", _font, _fontColor, _fontSize);

        for(int x = 1; x < _buttons.length; ++x){
            _buttons[x] = new Interact(x+"", 0xFFFFFF, xPos, yPos, BUTTON_RAD, x + 3, x + 3);
            _buttons[x].setText((x + 3) + "x" + (x + 3), _font, _fontColor, _fontSize);
            xPos += BUTTON_RAD * 2;
            if(xPos + BUTTON_RAD > g.getWidth()){
                xPos = BUTTON_RAD / 2;
                yPos += BUTTON_RAD * 2;
            }
        }
    }

    @Override
    public void render(Graphics g) {
        g.setFont(_font);
        if(_onMainMenu)_buttons[0].render(g);
        else for(int x = 1; x< _buttons.length; ++x) _buttons[x].render(g);
    }

    @Override
    public void update(float deltaTime) {
        //todo: Llamar a la animación aquí y gestionar el tiempo de dicha animación
    }


    @Override
    public void identifyEvent(int xEvent, int yEvent) {
        if(_onMainMenu && clickOnButton(xEvent,yEvent, _buttons[0])) lookForButtonAction(_buttons[0]);
        else if(!_onMainMenu){
            boolean find = false;
            int x = 1;

            for(; x <_buttons.length && !find; ++x)
                if(clickOnButton(xEvent, yEvent, _buttons[x])) find = true;

            if(find) lookForButtonAction(_buttons[x - 1]);
        }
    }

    @Override
    public void setMainApplicaton(Application a) {
        _mainApp = a;
    }

    private boolean clickOnButton(int xEvent, int yEvent, Interact b){
        if(xEvent >= b.getXPos() && xEvent <= b.getXPos() + (b.getRadius()) &&
           yEvent >= b.getYPos() && yEvent <= b.getYPos() + (b.getRadius())) return true;
        return false;
    }

    private void lookForButtonAction(Interact b){
        if(b.getId() == "Play") _onMainMenu = false;
            //System.out.println("Play Button Clicked");
        else{
            OhNo o = (OhNo) _mainApp;
            if (o!= null){
                System.out.println("Loading PlayState with BoardSize: " + b.getBoardWidth() + "x" + b.getBoardHeight());
                if (o!= null) o.goToPlayState(b.getBoardWidth(),b.getBoardHeight());
            }
        }
    }

    Interact _buttons[];
    private Application _mainApp;
    private String _font;
    private int _fontSize;
    private int _fontColor;
    private boolean _onMainMenu;
    private static final int BUTTON_RAD = 100;
}

