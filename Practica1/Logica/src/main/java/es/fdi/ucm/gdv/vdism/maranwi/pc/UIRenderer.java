package es.fdi.ucm.gdv.vdism.maranwi.pc;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;

public class UIRenderer {
    public UIRenderer(Tablero tablero) {
        _matrizJuego = tablero.getMatrizJuego();
        _font = "";
        _board=tablero;

    }

    public void render(Graphics g) {
        int fontS = 48;
        if (_font != "") {

            g.newFont(_font + ".ttf", _font, fontS, false);
            g.setFont(_font);
        }
        String hintText=_board.getHint();
        if(hintText!=""){
            g.setColor(0xFFFFFF);
            g.drawText(hintText,0,0);
        }
        int tamanyoFicha = g.getWidth() / (_matrizJuego.length + 1);
        for (int i = 0; i < _matrizJuego.length; i++)
            for (int j = 0; j < _matrizJuego[0].length; j++) {
                g.setColor(GetColorFromInt(_matrizJuego[i][j].getTipoCelda()));
                int X = (i * tamanyoFicha) + tamanyoFicha / 2;
                int Y = _offsetY+(j * tamanyoFicha) + tamanyoFicha / 2;
                g.fillCircle(X, Y, tamanyoFicha);
                int neighbours = _matrizJuego[i][j].getRequiredNeighbours();
                if (neighbours > -1) {
                    g.setColor(0xFFFFFF);
                    g.drawText(Integer.toString(neighbours), X + (tamanyoFicha / 2) - fontS / 4, Y + (tamanyoFicha / 2) + fontS / 4);
                }
            }
    }

    private int GetColorFromInt(TipoCelda Colorid) {
        if (Colorid == TipoCelda.Azul)
            return 0x0000FF;
        else if (Colorid == TipoCelda.Rojo)
            return 0xFF0000;
        else if (Colorid == TipoCelda.Blanco)
            return 0xFFFFFF;
        return -1;

    }

    public void setFont(String font) {
        _font = font;
    }

    //In order to ask for a hint
    private Tablero _board;
    private Celda[][] _matrizJuego;
    String _font;
    //final son const
    final private int _offsetY = 100;
}
