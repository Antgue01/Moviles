package es.fdi.ucm.gdv.vdism.maranwi.pc;

public class Interact {
    public Interact(int x, int y, int w, int h){
        _xPos = x;
        _yPos = y;
        _width = w;
        _height  = h;
    }

    public int getWidth() {
        return _width;
    }

    public int getHeight() {
        return _height;
    }

    public int getXPos() {
        return _xPos;
    }

    public int getYPos() {
        return _yPos;
    }

    private int _width;
    private int _height;
    private int _xPos;
    private int _yPos;
}
