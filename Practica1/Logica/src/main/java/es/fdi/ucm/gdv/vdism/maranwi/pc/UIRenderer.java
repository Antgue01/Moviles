package es.fdi.ucm.gdv.vdism.maranwi.pc;

import java.awt.Color;

import es.fdi.ucm.gdv.vdism.maranwi.pc.Celda;
import es.fdi.ucm.gdv.vdism.maranwi.pc.Tablero;

public class UIRenderer {
    public  UIRenderer(Tablero tablero){
        _myTablero=tablero.getTablero();
    }
    public void render(){
        for (int i=0;i<_myTablero[0].length;i++)
              for(int j=0;j< _myTablero[1].length;j++){


        }
    }
    private int GetColorFromInt(TipoCelda Colorid){
    if (Colorid==TipoCelda.Azul)
        return 0x0000FFFF;
    else if(Colorid ==TipoCelda.Rojo)
        return  0xFF0000FF;
    else if(Colorid==TipoCelda.Blanco)
        return  0xFFFFFFFF;
    return -1;

    }
    private Celda[][] _myTablero;
}
