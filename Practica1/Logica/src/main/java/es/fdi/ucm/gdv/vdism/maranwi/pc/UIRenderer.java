package es.fdi.ucm.gdv.vdism.maranwi.pc;
import  es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;

public class UIRenderer {
    public UIRenderer(Tablero tablero){
        _matrizJuego = tablero.getMatrizJuego();
    }
    public void render(Graphics g){
        int tamanyoFicha= g.getWidth()/( _matrizJuego[0].length +1);
        for (int i = 0; i< _matrizJuego[0].length; i++)
              for(int j = 0; j< _matrizJuego[1].length; j++){
                  g.setColor(GetColorFromInt(_matrizJuego[i][j].getTipoCelda()));
                  g.fillCircle((i*tamanyoFicha)+tamanyoFicha/2,(j*tamanyoFicha)+tamanyoFicha/2,tamanyoFicha);
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
    private Celda[][] _matrizJuego;
}
