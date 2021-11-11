package es.fdi.ucm.gdv.vdism.maranwi.logica;


public class Move {
    public Move(int x, int y, TipoCelda type) {
        _X = x;
        _Y = y;
        _Type = type;
    }

    public int getX() {
        return _X;
    }

    public int getY() {
        return _Y;
    }

    public TipoCelda getType() {
        return _Type;
    }

    private int _X;
    private  int _Y;
    private TipoCelda _Type;
}
