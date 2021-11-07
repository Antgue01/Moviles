package es.fdi.ucm.gdv.vdism.maranwi.pc;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.GameState;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Input;

public class PlayState implements Application, GameState {
    public void setBoardSize(int fil, int col, int width, int height){
        _fils = fil;
        _cols = col;
        _width = width;
        _height = height;
    }

    @Override
    public void onInit() {
        //_board = new Tablero(_fils, _cols, _width, _height);
        _board = new Tablero(4, 4, 400, 600);
        _board.rellenaMatrizResueltaRandom();
        _gameMatrix = _board.getMatrizJuego();
        _hints = new Pista();
        _font = "JosefinSans-Bold";
    }

    @Override
    public boolean onExit() {
        return false;
    }

    @Override
    public void onRelease() {

    }

    @Override
    public void onInput(Input input) {

    }

    @Override
    public void onRender(Graphics g) {
        int fontS = 48;
        if (_font != "") {

            g.newFont(_font + ".ttf", _font, fontS, false);
            g.setFont(_font);
        }
        String hintText= _board.getHint();
        if(hintText!=""){
            g.setColor(0xFFFFFF);
            g.drawText(hintText,0,0);
        }
        int tamanyoFicha = g.getWidth() / (_gameMatrix.length + 1);
        for (int i = 0; i < _gameMatrix.length; i++)
            for (int j = 0; j < _gameMatrix[0].length; j++) {
                g.setColor(GetColorFromInt(_gameMatrix[i][j].getTipoCelda()));
                int X = (i * tamanyoFicha) + tamanyoFicha / 2;
                int Y = _offsetY+(j * tamanyoFicha) + tamanyoFicha / 2;
                g.fillCircle(X, Y, tamanyoFicha);
                int neighbours = _gameMatrix[i][j].getRequiredNeighbours();
                if (neighbours > -1) {
                    g.setColor(0xFFFFFF);
                    g.drawText(Integer.toString(neighbours), X + (tamanyoFicha / 2) - fontS / 4, Y + (tamanyoFicha / 2) + fontS / 4);
                }
            }
    }

    @Override
    public void setGameZone(double width, double height, double windowsWidth, double windowsHeigth) {

    }

    @Override
    public void onUpdate(float deltaTime) {
        _hints.aplicar(_board,true);

        _board.compruebaSolucion();
    }

    @Override
    public void identifyEvent(int x, int y) {

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

    private Pista _hints;
    private Tablero _board;
    private Celda[][] _gameMatrix;
    private int _fils;
    private int _cols;
    private int _width;
    private int _height;
    private String _font;
    private Application _mainApp;
    //final = const
    final private int _offsetY = 100;
}
