package es.fdi.ucm.gdv.vdism.maranwi.logica;

import java.util.Arrays;
import java.util.Random;

public class BoardGenerator {

    public BoardGenerator(int rows, int cols, int [][]gameMatrix, int[][] dirs){
        _rows = rows;
        _cols = cols;
        _gameMatrix = gameMatrix;
        _dirs = dirs;
        _rand = new Random();
        _procesed = new boolean[_rows][_cols];
        Arrays.fill(_procesed, false);
        //TODO instanciar y rellenar el map con las ids aqui
        newBoard();
    }

    private void newBoard(){

        int newRow = 0;
        int newCol = 0;
        int cont = 0;
        boolean isEnd = false;
        boolean found = false;

        while(!isEnd){
            //Try a random position
            found = false;
            cont = 0;
            while(cont < 3 && !found){
                newRow = _rand.nextInt(_rows);
                newCol = _rand.nextInt(_cols);
                if(validPos(newRow, newCol) && !_procesed[newRow][newCol]){
                    found = true;
                }else ++cont;
            }

            //If we cant get a random position find the first one not processed
            //todo CAMBIAR ESTE RECORRIDO POR UN MAP QUE SEA DE STRINGS Y QUE CADA ID SEA ROW+COL (SIN BORRAR EL ARRAY DE BOOLEANOS!)
            //TODO ES DECIR AÑADIR UNA VARIABLE AUXILIAR MÁS QUE SEA EL MAP Y QUE SE RELLENE CON LOS IDS DE TODAS LAS CELDAS DEL TABLERO
            //TODO PARA PODER HACER ESTE CAMBIO CUANDO SE PROCESE (mas abajo) UNA CELDA HAY QUE SACARLA DEL MAP
            //TODO ASI PODEMOS COGER EL PRIMERO DEL MAP Y NO HAY QUE HACER 1 RECORRIDO CADA VEZ QUE SE QUIERE COGER UNA
            if(!found){
                for(int row = 0; row < _procesed[0].length && !found; ++row){
                    for(int col = 0; col < _procesed[1].length && !found; ++col){
                        if(!_procesed[row][col]){
                            found = true;
                            newRow = row;
                            newCol = col;
                        }
                    }
                }
            }

            //If we cant get a not proccesed target, board is full, otherwise procese it
            if(!found) isEnd = true;
            else{
                int neigbours = tryNewBlueNumeric(newRow, newCol);
                if(neigbours != -1){
                    //todo establecer como numérica con valor neigbours y cerrar con rojos en todas las direcciones
                }else{
                   //todo establecer como ficha roja
                }
                _procesed[newRow][newCol] = true;
            }
        }
    }

    private int tryNewBlueNumeric(int row, int col){
        int maxNeigbours = _cols;

        //if totalNeigbours > maxNeigbours -> red case
        if(getTotalNeigbours(row, col) > maxNeigbours)
            return -1;

        //Random neigbours
        int randNeigbours = _rand.nextInt(maxNeigbours); //nextInt = exclusive in last valor
        randNeigbours += 1; //We want a random number in range [1, maxNeigbours]

        int dir = 0, totalNeigbours = 0;
        int []usedDirs = new int[4];
        Arrays.fill(usedDirs, -1); //-1 = DIR NOT USED / CHECKED
        boolean existNewPossibleNeigbours = true;

        while(existNewPossibleNeigbours){
            dir = getRandomDir(usedDirs);

            //If exist not checked dir
            if(dir != -1){
                usedDirs[dir] = 0; //Now this dir is checked
                int candidateRow = row + _dirs[dir][0];
                int candidateCol = col + _dirs[dir][1];

                if(validPos(candidateRow, candidateCol) && !_procesed[candidateRow][candidateCol]){
                    //Calculate how many blue are in this direction
                    int n = getNeigboursInDir(candidateRow, candidateCol, _dirs[dir]);

                    //If it is exceeded, discard the dir
                    if(totalNeigbours + n > randNeigbours){
                        //todo  TIENE QUE SER UN ROJO (PORQUE SE HA EXCEDIDO SI PONE UN AZUL EN ESA DIRECCIÓN)
                    }
                    else{
                        //todo  SI NO SE DESCARTA ESTABLECER LA CELDA (candidateRow,candidateCol) COMO AZUL y ES FICHA
                        totalNeigbours += n;
                    }
                    _procesed[candidateRow][candidateCol] = true;
                }
            }
            else{
                existNewPossibleNeigbours = false;
            }
        }
        return totalNeigbours;
    }

    private int getTotalNeigbours(int row, int col){
        int total = 0;
        for(int[] d : _dirs){
            total += getNeigboursInDir(row, col, d);
        }
        return total;
    }

    private int getNeigboursInDir(int row, int col, int[] dir){
        int total = 0;
        //todo CONTAR LAS AZULES ADYACENTES EN ESTA DIRECCIÓN (no tener en cuenta la posicion row/col, SOLO los siguientes)
        return total;
    }

    private int getRandomDir(int[] usedDirs){
        //Try random dir
        int cont = 0;
        int dir = 0;
        boolean existNewDir = false;

        while(cont < 4 && !existNewDir){
            dir = _rand.nextInt(_dirs.length); //Random dir
            if(usedDirs[dir] == -1) existNewDir = true; //If this dir is not used we got a new dir
        }
        //If we cant get a random dir try to get the first one not used
        if(!existNewDir){
            for(dir = 0; dir < usedDirs.length && !existNewDir; ++dir)
                if(usedDirs[dir] == -1) existNewDir = true;
        }
        return (existNewDir) ? dir : -1;
    }

    private boolean validPos(int row, int col){
        return (row >= 0 && row < _rows) && (col >= 0 && col < _cols);
    }

    private int[][] _dirs;
    private int[][] _gameMatrix;
    boolean _procesed[][];
    private java.util.Random _rand;
    private int _rows;
    private int _cols;

}
