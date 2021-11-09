package es.fdi.ucm.gdv.vdism.maranwi.pc;

import sun.util.resources.cldr.ext.CurrencyNames_to;

//                  0     1      2
enum TipoCelda {Azul, Rojo, Blanco}

public class Celda {
    Celda(int id, boolean esFicha, TipoCelda tipo, int neighbours, int x, int y, int rad, int BOARD_LOGIC_OFFSET_Y, String font, int fontColor, int fontSize) {
        _esFicha = esFicha;
        _tipo = tipo;
        _requiredNeighbours = neighbours;

        int xPos = (x * rad) + rad / 2;
        int yPos = BOARD_LOGIC_OFFSET_Y + (y * rad) + rad / 2;
        _button = new Interact(id + "", getColorFromInt(tipo), xPos, yPos, rad, x, y);
        if (neighbours != -1) {
            _button.setText(neighbours + "", font, fontColor, fontSize);
        }
    }

    //Devuelve 0 si no hay cambios
    //Devuelve 1 si se cambia una ficha blanca a otro color - se pierde una blanca en el tablero -
    //Devuelve 2 si se cambia una ficha de otro color a blanca - se añade una blanca en el tablero -
    public int cambiarFicha(boolean siguiente) {
        int returnCode = 0;
        if (_esFicha) {
            if (_tipo == TipoCelda.Blanco) returnCode = 1;

            if (siguiente) _tipo = TipoCelda.values()[(_tipo.ordinal() + 1) % 3];
            else _tipo = TipoCelda.values()[(_tipo.ordinal() + 2) % 3];

            if (_tipo == TipoCelda.Blanco) returnCode = 2;

            _button.setBaseColor(getColorFromInt(_tipo));
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

    private int getColorFromInt(TipoCelda Colorid) {
        if (Colorid == TipoCelda.Azul)
            return 0x0000FF;
        else if (Colorid == TipoCelda.Rojo)
            return 0xFF0000;
        else if (Colorid == TipoCelda.Blanco)
            return 0xFFFFFF;
        return -1;
    }

    private TipoCelda _tipo;
    private Interact _button;
    private int _requiredNeighbours;
    private boolean _esFicha;
}
