package es.fdi.ucm.gdv.vdism.maranwi.pc;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.GameState;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;

public class PlayState implements GameState {

    @Override
    public void start(Graphics g) {
        //_board = new Tablero(_fils, _cols, _width, _height);
        _hints = new Pista();
        _font = "JosefinSans-Bold";
        _fontSize = 48;
        g.newFont(_font + ".ttf", _font, _fontSize, false);

        _hintText = "";
    }

    @Override
    public void render(Graphics g) {
        g.setFont(_font);

        if(_hintText!=""){
            g.setColor(0xFFFFFF);
            g.drawText(_hintText,0,0);
        }

        int tamanyoFicha = g.getWidth() / (_gameMatrix.length + 1);
        for (int i = 0; i < _gameMatrix.length; i++)
            for (int j = 0; j < _gameMatrix[0].length; j++) {
                g.setColor(GetColorFromInt(_gameMatrix[i][j].getTipoCelda()));
                int X = (i * tamanyoFicha) + tamanyoFicha / 2;
                int Y = BOARD_LOGIC_OFFSET_Y +(j * tamanyoFicha) + tamanyoFicha / 2;
                g.fillCircle(X, Y, tamanyoFicha);
                int neighbours = _gameMatrix[i][j].getRequiredNeighbours();
                if (neighbours > -1) {
                    g.setColor(0xFFFFFF);
                    g.drawText(Integer.toString(neighbours), X + (tamanyoFicha / 2) - _fontSize / 4, Y + (tamanyoFicha / 2) + _fontSize / 4);
                }
            }
    }

    @Override
    public void update(float deltaTime) {
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

    private int GetColorFromInt(TipoCelda Colorid){
        if (Colorid==TipoCelda.Azul)
            return 0x0000FF;
        else if(Colorid ==TipoCelda.Rojo)
            return  0xFF0000;
        else if(Colorid==TipoCelda.Blanco)
            return  0xFFFFFF;
        return -1;
    }

    public void setBoardSize(int rows, int cols, int width, int height){
        _board = new Tablero(rows, cols, width, height);
        _board.rellenaMatrizResueltaRandom();
        _gameMatrix = _board.getMatrizJuego();
    }

    private Pista _hints;
    String _hintText;
    private Tablero _board;
    private Celda[][] _gameMatrix;
    private String _font;
    private int _fontSize;
    private Application _mainApp;
    static private final int BOARD_LOGIC_OFFSET_Y = 100;
}
