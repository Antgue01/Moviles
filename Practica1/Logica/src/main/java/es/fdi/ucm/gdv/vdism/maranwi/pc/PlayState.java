package es.fdi.ucm.gdv.vdism.maranwi.pc;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Font;
import es.fdi.ucm.gdv.vdism.maranwi.engine.GameState;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;

public class PlayState implements GameState {

    @Override
    public void start(Graphics g) {
        //_board = new Tablero(_fils, _cols, _width, _height);
        _hints = new Pista();
        _font = g.newFont("JosefinSans-Bold.ttf", 48, false);
        _fontColor = 0x000000;
        _hintText = "";
        _board = new Tablero(_rows, _cols);
        _board.rellenaMatrizResueltaRandom(_buttonRadius, BOARD_LOGIC_OFFSET_Y, _font, _fontColor);
        _gameMatrix = _board.getMatrizJuego();
    }

    @Override
    public void render(Graphics g) {
        if(_hintText!=""){
            g.setFont(_font);
            g.setColor(0xFFFFFF);
            g.drawText(_hintText,0,0);
        }

       for (int x = 0; x < _gameMatrix.length; x++)
           for (int y = 0; y < _gameMatrix[0].length; y++)
                _gameMatrix[x][y].getButton().render(g);
    }

    @Override
    public void update(double deltaTime) {
        //_hints.aplicar(_board,true);
        //_hintText = _hints.getCurrentHint();
        _board.compruebaSolucion();
    }

    @Override
    public void identifyEvent(int x, int y) {
        //_hints.aplicar(_board,true);
        //_hintText = _hints.getCurrentHint();
        System.out.println(x + " " + y);
    }

    @Override
    public void setMainApplicaton(Application a) {
        _mainApp = a;
    }

    public void setBoardSize(int rows, int cols){
        _rows = rows;
        _cols = cols;
        _buttonRadius = _mainApp.getLogicWidth() / ( cols + 1);
    }

    private boolean clickOnButton(int xEvent, int yEvent, Interact b){
        if(xEvent >= b.getXPos() && xEvent <= b.getXPos() + (b.getRadius()) &&
                yEvent >= b.getYPos() && yEvent <= b.getYPos() + (b.getRadius())) return true;
        return false;
    }

    private Pista _hints;
    private String _hintText;
    private Tablero _board;
    private Celda[][] _gameMatrix;
    private Font _font;
    private int _fontColor;
    private Application _mainApp;
    private static final int BOARD_LOGIC_OFFSET_Y = 100;
    private int _buttonRadius;
    private int _rows;
    private int _cols;
}
