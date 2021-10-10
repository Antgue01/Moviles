package es.fdi.ucm.gdv.vdism.maranwi.pc;

//                  0     1      2
enum TipoCelda { Azul, Rojo, Blanco}

public class Celda {
    Celda(boolean esFicha, TipoCelda tipo){
        _esFicha  = esFicha;
        _tipo = tipo;
    }

    public void siguiente(){
        if(!_esFicha)
            _tipo = TipoCelda.values()[(_tipo.ordinal() + 1) % 3];
    }

    public void anterior(){

        //PONER BIEN LO DE LOS MODULOS -> ANTONIO xD
        if(!_esFicha)
            _tipo = TipoCelda.values()[(_tipo.ordinal() + 3) - 1];
    }

    boolean _esFicha;
    TipoCelda _tipo;
}
