package es.fdi.ucm.gdv.vdism.maranwi.pc;

import java.util.Random;

import jdk.internal.net.http.common.Pair;


public class Tablero {
    public Tablero(int filas, int columnas, int width, int height) {
        _filas = filas;
        _columnas = columnas;
        _casillasAlto = height / filas;
        _casillasAncho = width / columnas;

        _matrizSolucion = new int[_filas][_columnas];
        _matrizJuego = new Celda[_filas][_columnas];

        _numFichasBlancas = 0;
        _tracker = new MoveTracker();
    }


    /*
        0 = Azul
        1 = Rojo
     */
    public void rellenaMatrizResueltaRandom() {
        java.util.Random r = new Random();
        for (int x = 0; x < _matrizSolucion[0].length; ++x) {
            for (int j = 0; j < _matrizSolucion[1].length; ++j) {
                _matrizSolucion[x][j] = r.nextInt(2);


                boolean esFicha = r.nextBoolean();
                Celda c;

                //Si es ficha se oculta su color, si no, se deja como está
                if (esFicha) {
                    c = new Celda(esFicha, TipoCelda.Blanco, -1);
                    ++_numFichasBlancas;
                } else {
                    int neigbours = _matrizSolucion[x][j] == 0 ? r.nextInt(3) + 1 : -1;
                    c = new Celda(esFicha, TipoCelda.values()[_matrizSolucion[x][j]], neigbours);

                }

                _matrizJuego[x][j] = c;
            }
            //prueba pista 2
//            _matrizJuego[1][1] = new Celda(false, TipoCelda.Azul, 1);
//            _matrizJuego[1][2] = new Celda(true, TipoCelda.Blanco, -1);
//            _matrizJuego[2][1] = new Celda(true, TipoCelda.Blanco, -1);
//            _matrizJuego[1][3] = new Celda(true, TipoCelda.Azul, -1);
//            _matrizJuego[3][1] = new Celda(true, TipoCelda.Azul, -1);

            //pruebas pistas 6 y 7
//            _matrizJuego[1][1]=new Celda(true,TipoCelda.Azul,-1);
//            _matrizJuego[0][1]=new Celda(true,TipoCelda.Rojo,-1);
//            _matrizJuego[1][0]=new Celda(true,TipoCelda.Rojo,-1);
//            _matrizJuego[1][2]=new Celda(true,TipoCelda.Rojo,-1);
//            _matrizJuego[2][1]=new Celda(true,TipoCelda.Rojo,-1);

        }
    }

    public void setColor(int X, int Y, TipoCelda tipo) {
        if (X >= 0 && X < _matrizJuego[0].length && Y >= 0 && Y < _matrizJuego.length && _matrizJuego[Y][X].getEsFicha()) {
            boolean wasWhite = _matrizJuego[Y][X].getTipoCelda() == TipoCelda.Blanco;
            _matrizJuego[Y][X].setTipo(tipo);
            if (wasWhite && tipo != TipoCelda.Blanco)
                _numFichasBlancas--;
            else if (!wasWhite && tipo == TipoCelda.Blanco)
                _numFichasBlancas++;
        }
    }

    public Pair<Integer, Integer> identificaFicha(float xPos, float yPos) {
        int identificadorX = (int) (xPos / _casillasAncho);
        int identificadorY = (int) (yPos / _casillasAlto);

        return new Pair<Integer, Integer>(identificadorX, identificadorY);
    }

    //Auxiliares que hay que quitar, se ponen para completar onclick
    public boolean leftClick() {
        return true;
    }

    //ESTAMOS SUPONIENDO QUE TODOS LOS CLICKS CAEN EN CASILLAS
    public void onClick(float xPos, float yPos) {
        Pair<Integer, Integer> posicionFicha = identificaFicha(xPos, yPos);

        //Identificar si es click izquierdo o click derecho (preguntar)
        boolean siguiente = leftClick() ? true : false;

        //0 = NO hay cambios. 1 = se quita blanca del tablero. 2 = se añade blanca al tablero
        _tracker.addMove(posicionFicha.first, posicionFicha.second, _matrizJuego[posicionFicha.first][posicionFicha.second].getTipoCelda());
        int result = _matrizJuego[posicionFicha.first][posicionFicha.second].cambiarFicha(siguiente);

        if (result == 1) --_numFichasBlancas;
        else if (result == 2) ++_numFichasBlancas;
        //if(//han pulsado el botón de restaurar)
//            _tracker.restoreMove(this);
    }

    public boolean compruebaSolucion() {
        if (_numFichasBlancas > 0)
            return false;
        boolean esSolucion = true;
        for (int x = 0; x < _matrizSolucion[0].length && esSolucion; ++x) {
            for (int j = 0; j < _matrizSolucion[1].length; ++j) {
                if (_matrizJuego[x][j].getEsFicha() && _matrizJuego[x][j].getTipoCeldaAsInt() != _matrizSolucion[x][j])
                    esSolucion = false;
            }
        }
        return esSolucion;
    }

    public Celda[][] getMatrizJuego() {
        return _matrizJuego;
    }

    //La matriz solucíón solo guarda los colores de cada posición
    private int _matrizSolucion[][];
    private Celda _matrizJuego[][];
    private int _filas;
    private int _columnas;
    private float _casillasAncho;
    private float _casillasAlto;
    private MoveTracker _tracker;
    private  int _numFichasBlancas;
}
