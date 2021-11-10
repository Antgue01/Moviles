package es.fdi.ucm.gdv.vdism.maranwi.pc;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Font;
import es.fdi.ucm.gdv.vdism.maranwi.engine.GameState;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;

public class MenuState implements GameState {
    @Override
    public void start(Graphics g) {
        _fontColor = 0x000000;
        _font = g.newFont("JosefinSans-Bold.ttf", 50, true);
        _onMainMenu = true;

        //Main menu => 1 Button : Play
        int xPos = BUTTON_RAD / 2, yPos = BUTTON_RAD;
        _buttons = new Interact[5];
        _buttons[0] = new Interact("Play", 0xFFFFFF, (_mainApp.getLogicWidth()/2) - BUTTON_RAD, (_mainApp.getLogicHeight()/2) - BUTTON_RAD, BUTTON_RAD * 2, 0, 0);
        _buttons[0].setText("Start", _font, _fontColor);

        //Sub menu => Board size selection buttons: 1 button per size
        for(int x = 1; x < _buttons.length; ++x){
            _buttons[x] = new Interact(x+"", 0xFFFFFF, xPos, yPos, BUTTON_RAD, x + 3, x + 3);
            _buttons[x].setText((x + 3) + "x" + (x + 3), _font, _fontColor);
            xPos += BUTTON_RAD * 2;
            if(xPos + BUTTON_RAD > g.getCanvasWidth()){
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
    public void update(double deltaTime) {
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
                System.out.println("Loading PlayState with BoardSize: " + b.getBoardX() + "x" + b.getBoardY());
                if (o!= null) o.goToPlayState(b.getBoardX(),b.getBoardY());
            }
        }
    }

    private Interact _buttons[];
    private Application _mainApp;
    private Font _font;
    private int _fontColor;
    private boolean _onMainMenu;
    private static final int BUTTON_RAD = 100;
}

