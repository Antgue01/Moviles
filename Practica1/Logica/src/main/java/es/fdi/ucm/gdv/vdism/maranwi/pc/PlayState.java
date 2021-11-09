package es.fdi.ucm.gdv.vdism.maranwi.pc;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Font;
import es.fdi.ucm.gdv.vdism.maranwi.engine.GameState;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Image;

public class PlayState implements GameState {

    @Override
    public void start(Graphics g) {
        _hints = new Pista();
        _font = g.newFont("JosefinSans-Bold.ttf", 48, false);
        _fontColor = 0x000000;
        _hintText = "";
        _board = new Tablero(_rows, _cols);
        _board.rellenaMatrizResueltaRandom(_buttonRadius, BOARD_LOGIC_OFFSET_X, BOARD_LOGIC_OFFSET_Y, _font, _fontColor);

        _buttons = new Interact[3];
        int xOffset = _mainApp.getLogicWidth() / 6;
        int xPos = xOffset / 2;
        int yPos = BOARD_LOGIC_OFFSET_Y + (_buttonRadius * _rows) + (BOARD_LOGIC_OFFSET_Y / 2);

        //Exit button
        _buttons[0] = new Interact("Exit", _fontColor, xPos, yPos, _buttonRadius, 0, 0);
//        Image playImg = g.newImage("");
//        _buttons[0].setImage(playImg);

        //Undo button
        xPos += xOffset * 2;
        _buttons[1] = new Interact("Undo", _fontColor, xPos, yPos, _buttonRadius, 0, 0);
//        Image undoImg = g.newImage("");
//        _buttons[0].setImage(undoImg);

        //Hints button
        xPos += xOffset * 2;
        _buttons[2] = new Interact("Hints", _fontColor, xPos, yPos, _buttonRadius, 0, 0);
//        Image hintsImg = g.newImage("");
//        _buttons[0].setImage(hintsImg);
    }

    @Override
    public void render(Graphics g) {
        if(_hintText!=""){
            g.setFont(_font);
            g.setColor(0xFFFFFF);
            g.drawText(_hintText,0,0);
        }

       for (int x = 0; x < _rows; x++)
           for (int y = 0; y < _cols; y++)
               _board.getMatrizJuego()[x][y].getButton().render(g);

       for(Interact b: _buttons)  b.render(g);

    }

    @Override
    public void update(double deltaTime) {
        //_hints.aplicar(_board,true);
        //_hintText = _hints.getCurrentHint();
        _board.compruebaSolucion();
    }

    @Override
    public void identifyEvent(int x, int y) {
        if(x >= BOARD_LOGIC_OFFSET_X && x <= BOARD_LOGIC_OFFSET_X + (_buttonRadius * _cols) &&
           y >= BOARD_LOGIC_OFFSET_Y && y <= BOARD_LOGIC_OFFSET_Y + (_buttonRadius * _rows)){
            int row = (x - BOARD_LOGIC_OFFSET_X) / _buttonRadius;
            int col = (y - BOARD_LOGIC_OFFSET_Y) / _buttonRadius;
            _board.nextColor(row, col);
            if(_hintText != "") _hintText = "";
            //System.out.println("Event x :" + x + " Event y: " + y + " ButtonRadius: " + _buttonRadius);
            //System.out.println("Row x :" + row + " Col y: " + col);
        }
        else if(clickOnButton(x, y, _buttons[0])){ //EXIT
            if(_hintText != "") _hintText = "";
            OhNo o = (OhNo) _mainApp;
            if (o!= null){
                System.out.println("Loading MenuState");
                if (o!= null) o.goToMenuState();;
            }
        }
        else if(clickOnButton(x, y, _buttons[1])){ //UNDO
            System.out.println("UNDO");
            if(_hintText != "") _hintText = "";
            _board.restoreMove();
        }
        else if(clickOnButton(x, y, _buttons[2])){ // HINTS
            System.out.println("HINTS");
            _hints.aplicar(_board,true);
            _hintText = _hints.getCurrentHint();
        }

    }

    @Override
    public void setMainApplicaton(Application a) {
        _mainApp = a;
    }

    public void setBoardSize(int rows, int cols){
        _rows = rows;
        _cols = cols;
        _buttonRadius = _mainApp.getLogicWidth() / ( cols + 1);
        BOARD_LOGIC_OFFSET_X = _buttonRadius / 2;
    }

    private boolean clickOnButton(int xEvent, int yEvent, Interact b){
        if(xEvent >= b.getXPos() && xEvent <= b.getXPos() + (b.getRadius()) &&
                yEvent >= b.getYPos() && yEvent <= b.getYPos() + (b.getRadius())) return true;
        return false;
    }

    private Application _mainApp;
    private Interact _buttons[];
    private Tablero _board;
    private Pista _hints;
    private String _hintText;
    private Font _font;
    private int _fontColor;
    private int _buttonRadius;
    private int _rows;
    private int _cols;
    private static final int BOARD_LOGIC_OFFSET_Y = 100;
    private int BOARD_LOGIC_OFFSET_X;
}
