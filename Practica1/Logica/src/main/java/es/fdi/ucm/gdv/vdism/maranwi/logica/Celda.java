package es.fdi.ucm.gdv.vdism.maranwi.logica;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Font;

//                  0     1      2
enum TipoCelda {Azul, Rojo, Blanco}

public class Celda {
    Celda(int id, boolean esFicha, TipoCelda tipo, int neighbours, int x, int y, int rad, int xPos, int yPos, Font font, MyColor fontColor) {
        _esFicha = esFicha;
        _tipo = tipo;
        _requiredNeighbours = neighbours;

        _color=newColorFromType(tipo);
        _lastColor = newColorFromType(tipo);
        _button = new Interact(id + "", _color, xPos, yPos, rad, x, y);
        if (neighbours != -1) {
            _button.setText(neighbours + "", font, fontColor);
        }
    }

    //Devuelve 0 si no hay cambios
    //Devuelve 1 si se cambia una ficha blanca a otro color - se pierde una blanca en el tablero -
    //Devuelve 2 si se cambia una ficha de otro color a blanca - se añade una blanca en el tablero -
    public int cambiarFicha(boolean siguiente) {
        int returnCode = 0;
        if (_esFicha) {
            _lastColor = newColorFromType(_tipo);
            if (_tipo == TipoCelda.Blanco) returnCode = 1;

            if (siguiente) _tipo = TipoCelda.values()[(_tipo.ordinal() + 1) % 3];
            else _tipo = TipoCelda.values()[(_tipo.ordinal() + 2) % 3];

            if (_tipo == TipoCelda.Blanco) returnCode = 2;

            transformColor(_tipo);
            _button.setBaseColor(_color);
            return returnCode;
        }
        return returnCode;
    }

    public boolean getEsFicha() {
        return _esFicha;
    }

    public TipoCelda getTipoCelda() {
        return _tipo;
    }

    public int getTipoCeldaAsInt() {
        return _tipo.ordinal();
    }

    public int getRequiredNeighbours() {
        return _requiredNeighbours;
    }


    public void setTipo(TipoCelda tipo) {
        _tipo = tipo;
    }

    public Interact getButton() {
        return _button;
    }

    public MyColor getColor(){ return _color;}

    public MyColor getLastColor() { return _lastColor;}

    private void transformColor(TipoCelda Colorid) {
        if (Colorid == TipoCelda.Azul)
            _color.setRGBA(0x20C4E4FF);
        else if (Colorid == TipoCelda.Rojo)
            _color.setRGBA(0xFF3C4CFF);
        else if (Colorid == TipoCelda.Blanco)
            _color.setRGBA(0xF0ECECFF);
    }
    private MyColor newColorFromType(TipoCelda Colorid) {
        if (Colorid == TipoCelda.Azul)
            return  new MyColor(0x20C4E4FF);
        else if (Colorid == TipoCelda.Rojo)
            return new MyColor(0xFF3C4CFF);
        else if (Colorid == TipoCelda.Blanco)
            return  new MyColor(0xF0ECECFF);
        else return  new MyColor(0);
    }

    private MyColor _color;
    private MyColor _lastColor;
    private TipoCelda _tipo;
    private Interact _button;
    private int _requiredNeighbours;
    private boolean _esFicha;
}
