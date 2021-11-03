package es.fdi.ucm.gdv.vdism.maranwi.pc;

import java.lang.reflect.Array;
import java.util.ArrayList;


public class Pista {
    public void Aplicar(Tablero t) {
        Celda[][] tablero = t.getMatrizJuego();
        boolean applied = false;
        //todo encontrar la clase pair, si es que existe
        int[][] dirs = new int[4][2];
        dirs[0][0] = 1;
        dirs[0][1] = 0;
        dirs[1][0] = -1;
        dirs[1][1] = 0;
        dirs[2][0] = 0;
        dirs[2][1] = 1;
        dirs[3][0] = 0;
        dirs[3][1] = -1;
        boolean Applicable = true;
        int[] offsets = new int[4];
        for (int i = 0; i < tablero[0].length && !applied; i++) {
            for (int j = 0; j < tablero[1].length && !applied; j++) {
                //si es fija
                if (!tablero[i][j].getEsFicha() && tablero[i][j].getTipoCelda() == TipoCelda.Azul) {
                    int total = 0;
                    //miramos si nos podemos saltar esta pista, ya que si tiene más vecinos de la cuenta está mal
                    for (int k = 0; k < dirs.length && Applicable; k++) {
                        offsets[k] = count(tablero, i, j, dirs[k][0], dirs[k][1], TipoCelda.Azul);
                        total += offsets[k]-1;
                        if (total > tablero[i][j].getRequiredNeighbours())
                            Applicable = false;

                    }
                    //si no nos hemos pasado
                    if (Applicable && total == tablero[i][j].getRequiredNeighbours()) {
                        for (int k = 0; k < dirs.length && Applicable; k++) {
                            int targetX = i + dirs[k][0] * offsets[k];
                            int targetY = j + dirs[k][1] * offsets[k];
                            if (targetX >= 0 && targetX < tablero[0].length && targetY >= 0 && targetY < tablero[1].length)
                                t.setColor(targetX, targetY, TipoCelda.Rojo);
                        }
                        applied = true;
                    }
                }

            }
        }
    }


    private int count(Celda[][] tablero, int X, int Y, int dirX, int dirY, TipoCelda target) {
        boolean counted = false;
        int number = 0;
        int myX = X , myY = Y ;
        for (int i = 0; i < tablero[0].length && !counted; i++) {
            for (int j = 0; j < tablero[1].length && !counted; j++) {
                //si me salgo paro
                if (myX > tablero[1].length || myX < 0 || myY > tablero[0].length || myY < 0)
                    return number;
                if (tablero[myY][myX].getTipoCelda() == target)
                    number++;
                else
                    counted = true;
                myX += dirX;
                myY += dirY;

            }
        }
        return number;
    }

    //cuenta todas las fichas
    private int count(Celda[][] tablero, int X, int Y, TipoCelda target) {
        int total = 0;
        int[][] dirs = new int[4][2];
        dirs[0][0] = 1;
        dirs[0][1] = 0;
        dirs[1][0] = -1;
        dirs[1][1] = 0;
        dirs[2][0] = 0;
        dirs[2][1] = 1;
        dirs[3][0] = 0;
        dirs[3][1] = -1;
        for (int i = 0; i < dirs.length; i++) {
            total += count(tablero, X, Y, dirs[i][0], dirs[i][1], target);
        }
        return total;

    }
}

