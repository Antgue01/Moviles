package es.fdi.ucm.gdv.vdism.maranwi.logica;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Font;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Image;

public class MenuState implements GameState {
    @Override
    public void start(Graphics g) {
        _titleText = "Oh no";
        _fontColor = new MyColor(0x000000FF);
        _font70 = g.newFont("Molle-Regular.ttf", 70, true);
        _font38 = g.newFont("JosefinSans-Bold.ttf", 38, true);
        _numbersFont = g.newFont("JosefinSans-Bold.ttf",48, true);
        _onMainMenu = true;

        //Main menu => 1 Button : Play
        _buttons = new Interact[8];
        _buttons[0] = new Interact("Play", new MyColor(0), (_mainApp.getLogicWidth()/3) + 10, (_mainApp.getLogicHeight()/3) + 10, 0, 0, 0);
        _buttons[0].setText("Touch\n  To\nPlay", _font70, _fontColor);

        int buttonsOffset = 10;
        int xPos = (BUTTON_RAD / 2) - buttonsOffset, yPos = BUTTON_RAD + (_mainApp.getLogicHeight()/4);
        //Sub menu => Board size selection buttons: 1 button per size
        for(int x = 1; x < _buttons.length - 1; ++x){
            MyColor color = (x%2==0) ? new MyColor(0xFF3C4CFF) : new MyColor(0x20C4E4FF);
            _buttons[x] = new Interact(x+"", color, xPos, yPos, BUTTON_RAD, x + FIRST_BOARD_SIZE, x + FIRST_BOARD_SIZE);
            _buttons[x].setText(Integer.toString(x + FIRST_BOARD_SIZE), _numbersFont, new MyColor(0xFFFFFFFF));
            xPos += BUTTON_RAD + buttonsOffset;
            if(xPos + BUTTON_RAD > _mainApp.getLogicWidth()){
                xPos = (BUTTON_RAD / 2) - buttonsOffset;
                yPos += BUTTON_RAD + buttonsOffset;
            }
        }

        //Exit button
        Image playImg = g.newImage("close.png");
        xPos = (_mainApp.getLogicWidth() / 2) - (playImg.getWidth() / 2);
        yPos = (_mainApp.getLogicHeight()) - (playImg.getHeigth() + (playImg.getHeigth() / 2));
        int rad = playImg.getWidth() / 2;
        _buttons[_buttons.length - 1] = new Interact("Exit", _fontColor, xPos , yPos, rad, 0, 0);
        _buttons[_buttons.length - 1].setImage(playImg, playImg.getWidth(), playImg.getHeigth() , true, 100);
        _buttons[_buttons.length - 1].setBottomCircle(false);
    }

    @Override
    public void render(Graphics g) {
        g.setFont(_font70);
        g.setColor(_fontColor);
        if(_onMainMenu){
            g.drawText(_titleText, (_mainApp.getLogicWidth()/3) - 35, _mainApp.getLogicHeight()/5);
            _buttons[0].render(g);
        }
        else{
            g.drawText(_titleText, (_mainApp.getLogicWidth()/3) - 35, _mainApp.getLogicHeight()/5);
            for(int x = 1; x< _buttons.length; ++x) _buttons[x].render(g);
            g.setFont(_font38);
            g.setColor(_fontColor);
            g.drawText("Select a size to play...", (_mainApp.getLogicWidth()/10), _mainApp.getLogicHeight()/3);
        }
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
        else if(b.getId() == "Exit") _onMainMenu = true;
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
    private Font _font70;
    private Font _font38;
    private Font _numbersFont;
    private MyColor _fontColor;
    private boolean _onMainMenu;
    private String _titleText;
    private static final int BUTTON_RAD = 100;
    private static final int FIRST_BOARD_SIZE =  3;
}

