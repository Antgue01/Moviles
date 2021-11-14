package es.fdi.ucm.gdv.vdism.maranwi.logica;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Font;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Image;

public class PlayState implements GameState {

    @Override
    public void start(Graphics g) {
        _finishState = false;

        _font = g.newFont("JosefinSans-Bold.ttf", 48, true);
        int numbersFontSize = (_buttonRadius * 48)/80;
        _numbersFont = g.newFont("JosefinSans-Bold.ttf", numbersFontSize, true);
        _fontColor = new MyColor(0x000000FF);


        _hintText = "";
        _hintFont = g.newFont("JosefinSans-Bold.ttf", 25, true);

        //BOARD SET UP
        _board = new Tablero(_rows, _cols);
        Image lockImg = g.newImage("lock.png");
        _board.setLockImg(lockImg);
        _board.rellenaMatrizResueltaRandom(_buttonRadius, BOARD_LOGIC_OFFSET_X, BOARD_LOGIC_OFFSET_Y, _numbersFont, new MyColor(0xFFFFFFFF));

        //BUTTONS
        _buttons = new Interact[3];
        int xOffset = (_mainApp.getLogicWidth() / 6) - 4;
        int yOffset = _mainApp.getLogicHeight() / 8;
        int xPos = xOffset;
        int yPos = _mainApp.getLogicHeight() - yOffset;

        //Exit button
        Image playImg = g.newImage("close.png");
        _buttons[0] = new Interact("Exit", _fontColor, xPos, yPos, playImg.getWidth() / 2, 0, 0,0);
        _buttons[0].setImage(playImg, playImg.getWidth() / 2, playImg.getHeigth() / 2, true, 100);
        _buttons[0].setBottomCircle(false);

        //Undo button
        xPos += xOffset * 2;
        Image undoImg = g.newImage("history.png");
        _buttons[1] = new Interact("Undo", _fontColor, xPos, yPos, undoImg.getWidth() / 2, 0, 0,0);

        _buttons[1].setImage(undoImg, undoImg.getWidth() / 2, undoImg.getHeigth() / 2, true, 100);
        _buttons[1].setBottomCircle(false);

        //Hints button
        xPos += xOffset * 2;
        Image hintsImg = g.newImage("eye.png");


        _buttons[2] = new Interact("Hints", _fontColor, xPos, yPos, hintsImg.getWidth() / 2, 0, 0,0);
        _buttons[2].setImage(hintsImg, hintsImg.getWidth() / 2, hintsImg.getHeigth() / 2, true, 100);
        _buttons[2].setBottomCircle(false);

        _animator = new Animator(_board);
    }

    @Override
    public void render(Graphics g) {
        g.setColor(_fontColor);
        if(_finishState){
            g.setFont(_hintFont);
            g.drawText(_hintText,(_mainApp.getLogicWidth() / 4) - 30,BOARD_LOGIC_OFFSET_Y - 40);
        }
        else if(_hintText == "") {
            g.setFont(_font);
            g.drawText(_boardSizeText,(_mainApp.getLogicWidth() / 2) - 50,BOARD_LOGIC_OFFSET_Y - 40);
        }else{
            g.setFont(_hintFont);
            g.drawText(_hintText,(_mainApp.getLogicWidth() / 9),BOARD_LOGIC_OFFSET_Y - 60);
        }

        _animator.render(g);

        for (int x = 0; x < _rows; x++)
           for (int y = 0; y < _cols; y++)
               _board.getMatrizJuego()[x][y].getButton().render(g);

        for(Interact b: _buttons)  b.render(g);
    }

    @Override
    public void update(double deltaTime) {
        _animator.update(deltaTime);

        if (!_finishState && _board.isBoardCompleted()){
            _finishState = true;
            _hintText = "Completed! Tap to restart";
        }
        _board.compruebaSolucion();
    }

    @Override
    public void identifyEvent(int x, int y) {
        if(_finishState){
            OhNo o = (OhNo) _mainApp;
            if (o!= null){
                System.out.println("Loading MenuState");
                if (o!= null) o.goToMenuState();;
            }
        }
        if(_hintText != "") _hintText = "";
        if(x > BOARD_LOGIC_OFFSET_X  && x < BOARD_LOGIC_OFFSET_X + (_buttonRadius * _cols) &&
           y > BOARD_LOGIC_OFFSET_Y && y < BOARD_LOGIC_OFFSET_Y + (_buttonRadius * _rows)){
                int row = (x - BOARD_LOGIC_OFFSET_X) / _buttonRadius;
                int col = (y - BOARD_LOGIC_OFFSET_Y) / _buttonRadius;
                if(checkCorrectBoard(row, col))
                _animator.addAnimationElement(row, col, false, false);
        }
        else if(clickOnButton(x, y, _buttons[0])){ //EXIT
            OhNo o = (OhNo) _mainApp;
            if (o!= null){
                System.out.println("Loading MenuState");
                if (o!= null) o.goToMenuState();;
            }
        }
        else if(clickOnButton(x, y, _buttons[1])){ //UNDO
            System.out.println("UNDO");
            Celda c = _board.restoreMove();
            if(c!= null){
                _animator.addAnimationElement(c.getButton().getBoardX(), c.getButton().getBoardY(), true, false);
            }else _animator.clickOutOfBoard();
        }
        else if(clickOnButton(x, y, _buttons[2])){ // HINTS
            System.out.println("HINTS");
            Hint hint = _board.getAHint();
            if(hint.getHintType() != HintsManager.HintType.NONE) {
                _hintText = hint.getHintMessage();
                int row = hint.getPos()[0];
                int col = hint.getPos()[1];
                if(checkCorrectBoard(row, col))
                    _animator.addAnimationElement(row, col, false, true);
            }
        }
        else _animator.clickOutOfBoard();
    }

    @Override
    public void setMainApplicaton(Application a) {
        _mainApp = a;
    }

    public void setBoardSize(int rows, int cols){
        _boardSizeText = rows + " x " + cols;
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

    private boolean checkCorrectBoard(int row, int col){
        if(row >= 0 && row < _board.getMatrizJuego()[0].length && col >= 0 && col < _board.getMatrizJuego()[1].length)
            return true;
        return false;
    }


    private Application _mainApp;
    private Font _font;
    private Font _numbersFont;
    private MyColor _fontColor;

    private String _boardSizeText;
    private Interact _buttons[];
    private Tablero _board;

    private Font _hintFont;
    private String _hintText;

    Animator _animator;

    private int _buttonRadius;
    private int _rows;
    private int _cols;
    private static final int BOARD_LOGIC_OFFSET_Y = 120;
    private int BOARD_LOGIC_OFFSET_X;

    private boolean _finishState;
}
