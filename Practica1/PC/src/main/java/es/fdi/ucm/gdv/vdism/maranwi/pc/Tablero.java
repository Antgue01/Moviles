package es.fdi.ucm.gdv.vdism.maranwi.pc;

import jdk.internal.net.http.common.Pair;



public class Tablero {
    public void Tablero(int filas, int columnas, int width, int height){
        _filas = filas;
        _columnas = columnas;
        _width = width;
        _height = height;

        _casillasAlto = height / filas;
        _casillasAncho = width / columnas;


        _matriz = new int [_filas][_columnas];
        _matrizJuego = new Celda[_filas][_columnas];
    }


    /*
        0 = Pared
        1 = Numérico
        2 = Azul
        3 = Rojo
     */
    public void rellenaMatrizResueltaRandom(){
        for(int x = 0; x < _matriz[0].length; ++x){
            for(int j = 0; j< _matriz[1].length; ++j){
                _matriz[x][j] = (int) Math.random() * 2;

                boolean esFicha = ((int) Math.random() * 2 ) == 1;
                Celda c = new Celda(esFicha, TipoCelda.values()[_matriz[x][j]]);
                _matrizJuego[x][j] = c;
            }
        }
    }



    public Pair<Integer, Integer> identificaFicha(float xPos, float yPos){
       int identificadorX = (int) (xPos / _casillasAncho);
       int identificadorY = (int) (yPos / _casillasAlto);

        return new Pair<Integer, Integer>(identificadorX, identificadorY);
    }

    public void cambiarFicha(boolean siguiente, Pair<Integer, Integer> casilla){

    }

    int _matriz[][];
    Celda _matrizJuego[][];
    int _filas;
    int _columnas;
    float _casillasAncho;
    float _casillasAlto;
    int _width;
    int _height;
}
