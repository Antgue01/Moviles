package es.fdi.ucm.gdv.vdism.maranwi.pc;

import sun.util.resources.cldr.ext.CurrencyNames_to;

//                  0     1      2
enum TipoCelda { Azul, Rojo, Blanco}

public class Celda {
    Celda(boolean esFicha, TipoCelda tipo){
        _esFicha  = esFicha;
        _tipo = tipo;
    }

    //Devuelve 0 si no hay cambios
    //Devuelve 1 si se cambia una ficha blanca a otro color - se pierde una blanca en el tablero -
    //Devuelve 2 si se cambia una ficha de otro color a blanca - se añade una blanca en el tablero -
    public int cambiarFicha(boolean siguiente){
        int returnCode = 0;
        if(_esFicha){
            if(_tipo == TipoCelda.Blanco)    returnCode = 1;

            if(siguiente) _tipo = TipoCelda.values()[(_tipo.ordinal() + 1) % 3];
            else _tipo = TipoCelda.values()[(_tipo.ordinal() + 2) % 3];

            if(_tipo == TipoCelda.Blanco) returnCode = 2;
            return returnCode;
        }
        return returnCode;
    }

    public boolean getEsFicha(){
        return _esFicha;
    }

    public TipoCelda getTipoCelda(){
        return _tipo;
    }

    public int getTipoCeldaAsInt(){
        return _tipo.ordinal();
    }

    private boolean _esFicha;
    private TipoCelda _tipo;
}
