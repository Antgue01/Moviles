package es.fdi.ucm.gdv.vdism.maranwi.logica;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Font;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;

public class MenuState implements GameState {
    @Override
    public void start(Graphics g) {
        _titleText = "Oh no";
        _fontColor = new MyColor(0x000000FF);
        _font = g.newFont("Molle-Regular.ttf", 70, true);
        _numbersFont = g.newFont("JosefinSans-Bold.ttf",50, true);
        _onMainMenu = true;

        //Main menu => 1 Button : Play
        _buttons = new Interact[7];
        _buttons[0] = new Interact("Play", new MyColor(0), (_mainApp.getLogicWidth()/3) + 10, (_mainApp.getLogicHeight()/3) + 10, 0, 0, 0);
        _buttons[0].setText("Touch\n  To\nPlay", _font, _fontColor);

        int buttonsOffset = 10;
        int xPos = (BUTTON_RAD / 2) - buttonsOffset, yPos = BUTTON_RAD;
        //Sub menu => Board size selection buttons: 1 button per size
        for(int x = 1; x < _buttons.length; ++x){
            MyColor color = (x%2==0) ? new MyColor(0xFF3C4CFF) : new MyColor(0x20C4E4FF);
            _buttons[x] = new Interact(x+"", color, xPos, yPos, BUTTON_RAD, x + 3, x + 3);
            _buttons[x].setText(Integer.toString(x + 3), _numbersFont, new MyColor(0xFFFFFFFF));
            xPos += BUTTON_RAD + buttonsOffset;
            if(xPos + BUTTON_RAD > _mainApp.getLogicWidth()){
                xPos = (BUTTON_RAD / 2) - buttonsOffset;
                yPos += BUTTON_RAD + buttonsOffset;
            }
        }
    }

    @Override
    public void render(Graphics g) {
        g.setFont(_font);
        if(_onMainMenu){
            g.setColor(_fontColor);
            g.drawText(_titleText, (_mainApp.getLogicWidth()/3) - 35, _mainApp.getLogicHeight()/5);
            _buttons[0].render(g);
        }
        else for(int x = 1; x< _buttons.length; ++x) _buttons[x].render(g);
    }

    @Override
    public void update(double deltaTime) {
        //todo: Llamar a la animación aquí y gestionar el tiempo de dicha animación
    }


    @Override
    public void identifyEvent(int xEvent, int yEvent) {
        if(_onMainMenu)
            lookForButtonAction(_buttons[0]);
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
    private Font _numbersFont;
    private MyColor _fontColor;
    private boolean _onMainMenu;
    private String _titleText;
    private static final int BUTTON_RAD = 100;
}

