package es.fdi.ucm.gdv.vdism.maranwi.pc;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Input;

public class PlayState implements GameState{
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
        _tracks = new Pista();
    }

    @Override
    public void onRender(Graphics g) {
        int elementSize= g.getWidth()/( _gameMatrix[0].length + 1);

        for (int i = 0; i< _gameMatrix[0].length; i++)
            for(int j = 0; j< _gameMatrix[1].length; j++){
                int color = GetColorFromInt(_gameMatrix[i][j].getTipoCelda());
                if(color != -1) {
                    g.setColor(color);
                    g.fillCircle((i*elementSize)+elementSize/2,(j*elementSize)+elementSize/2,elementSize);
                }else System.out.println("Error: Invalid color");
            }
    }

    @Override
    public void onUpdate(float deltaTime) {
        //_tracks.aplicar(_board);
        _board.compruebaSolucion();
    }

    @Override
    public void identifyEvent(int x, int y) {

        System.out.println(x + " " + y);
    }

    @Override
    public void setApplication(Application a) {

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

    private Pista _tracks;
    private Tablero _board;
    private Celda[][] _gameMatrix;
    private int _fils;
    private int _cols;
    private int _width;
    private int _height;
}
