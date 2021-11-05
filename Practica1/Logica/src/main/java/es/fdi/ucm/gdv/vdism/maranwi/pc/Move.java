package es.fdi.ucm.gdv.vdism.maranwi.pc;


public class Move {
    public Move(int x, int y, TipoCelda type) {
        _X = x;
        _Y = y;
        _Type = type;
    }
    private int _X;
    private  int _Y;
    private TipoCelda _Type;


    public int getX() {
        return _X;
    }

    public int getY() {
        return _Y;
    }


    public TipoCelda getType() {
        return _Type;
    }
}
