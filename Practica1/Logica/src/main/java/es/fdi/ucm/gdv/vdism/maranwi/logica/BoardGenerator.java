package es.fdi.ucm.gdv.vdism.maranwi.logica;

import java.lang.reflect.Array;
import java.util.Arrays;
import java.util.HashMap;
import java.util.Random;

public class BoardGenerator {

    public BoardGenerator(int rows, int cols, int[][] dirs) {
        _rows = rows;
        _cols = cols;
        _gameMatrix = new int[rows][cols];
        _dirs = dirs;
        _rand = new Random();
        _procesed = new boolean[_rows][_cols];
        _advancedInDir = new int[4];
        _freePositions = new HashMap<Integer, int[]>();

        for (int i = 0; i < _gameMatrix.length; i++)
            for (int j = 0; j < _gameMatrix[0].length; j++) {
                int[] pos = {i, j};
                _freePositions.put(i * _gameMatrix.length + j, pos);
                _procesed[i][j] = false;
                _gameMatrix[i][j] = -2;
            }

        //TODO instanciar y rellenar el map con las ids aqui
        newBoard();
    }

    public int[][] getGeneratedBoard() {
        return _gameMatrix;
    }

    private void newBoard() {

        int newRow = 0;
        int newCol = 0;
        int cont = 0;
        boolean isEnd = false;
        boolean found = false;

        while (!isEnd) {
            //Try a random position
            found = false;
            cont = 0;
            while (cont < 3 && !found) {
                newRow = _rand.nextInt(_rows);
                newCol = _rand.nextInt(_cols);
                if (validPos(newRow, newCol) && !_procesed[newRow][newCol]) {
                    found = true;
                } else ++cont;
            }

            //If we cant get a random position find the first one not processed
            //todo CAMBIAR ESTE RECORRIDO POR UN MAP QUE SEA DE STRINGS Y QUE CADA ID SEA ROW+COL (SIN BORRAR EL ARRAY DE BOOLEANOS!)
            //TODO ES DECIR AÑADIR UNA VARIABLE AUXILIAR MÁS QUE SEA EL MAP Y QUE SE RELLENE CON LOS IDS DE TODAS LAS CELDAS DEL TABLERO
            //TODO PARA PODER HACER ESTE CAMBIO CUANDO SE PROCESE (mas abajo) UNA CELDA HAY QUE SACARLA DEL MAP
            //TODO ASI PODEMOS COGER EL PRIMERO DEL MAP Y NO HAY QUE HACER 1 RECORRIDO CADA VEZ QUE SE QUIERE COGER UNA
            //me da una libre pero los hashmap no garantizan orden
            if (!found) {
                if(!_freePositions.isEmpty()){
                    int[] position = _freePositions.entrySet().iterator().next().getValue();
                    newRow = position[0];
                    newCol = position[1];
                    found = true;
                }
            }

            //If we cant get a not proccesed target, board is full, otherwise procese it
            if (!found) isEnd = true;
            else {

                int neigbours = tryNewBlueNumeric(newRow, newCol);
                if (neigbours != -1) {
                    //todo establecer como numérica con valor neigbours y cerrar con rojos en todas las direcciones
                    _gameMatrix[newRow][newCol] = neigbours;
                    for(int x = 0; x < _dirs.length; x++){
                        int newRedRow = newRow + (_dirs[x][0] * (_advancedInDir[x] + 1));
                        int newRedCol = newCol + (_dirs[x][1] * (_advancedInDir[x] + 1));

                        if(validPos(newRedRow, newRedCol) && _gameMatrix[newRedRow][newRedCol] == -2){
                            _gameMatrix[newRedRow][newRedCol] = -1;
                            _procesed[newRedRow][newRedCol] = true;
                            _freePositions.remove(newRedRow * _gameMatrix.length + newRedCol);
                        }
                    }
                } else {
                    //todo establecer como ficha roja
                    _gameMatrix[newRow][newCol] = -1;
                }
                _procesed[newRow][newCol] = true;
                _freePositions.remove(newRow * _gameMatrix.length + newCol);
            }
        }
    }

    private int tryNewBlueNumeric(int row, int col) {
        Arrays.fill(_advancedInDir, 0);
        int maxNeigbours = _cols;

        //if totalNeigbours > maxNeigbours -> red case
        int totalNeigbours = getTotalNeigbours(row, col);
        if (totalNeigbours > maxNeigbours)
            return -1;

        //Random neigbours
        int randNeigbours = _rand.nextInt(maxNeigbours); //nextInt = exclusive in last valor
        randNeigbours += 1; //We want a random number in range [1, maxNeigbours]


        //Fill blues
        int dir = 0;
        int[] usedDirs = new int[4];
        Arrays.fill(usedDirs, -1); //-1 = DIR NOT USED / CHECKED
        boolean existNewPossibleNeigbours = true;


        while (existNewPossibleNeigbours) {
            dir = getRandomDir(usedDirs);
            int candidateRow = 0, candidateCol = 0;
            //If exist not checked dir
            if (dir != -1) {
                candidateRow = row + (_dirs[dir][0] * (_advancedInDir[dir] + 1));
                candidateCol = col + (_dirs[dir][1] * (_advancedInDir[dir] + 1));

                if (validPos(candidateRow, candidateCol) && !_procesed[candidateRow][candidateCol]) {
                    //Calculate how many blue are in this direction
                    int n = getNeigboursInDir(candidateRow, candidateCol, _dirs[dir]);
                    //If it is exceeded, discard the dir
                    if (totalNeigbours + n + 1 > randNeigbours) {
                        //todo  TIENE QUE SER UN ROJO (PORQUE SE HA EXCEDIDO SI PONE UN AZUL EN ESA DIRECCIÓN)
                        _gameMatrix[candidateRow][candidateCol] = -1;
                        usedDirs[dir] = 0;//Now this dir is checked
                    } else {
                        //todo  SI NO SE DESCARTA ESTABLECER LA CELDA (candidateRow,candidateCol) COMO AZUL y ES FICHA
                        _gameMatrix[candidateRow][candidateCol] = 0;
                        totalNeigbours += n + 1;
                        _advancedInDir[dir] += n + 1;
                    }
                    _procesed[candidateRow][candidateCol] = true;
                    _freePositions.remove(candidateRow*_gameMatrix.length+candidateCol);
                }
                else usedDirs[dir]=0;
            } else {
                existNewPossibleNeigbours = false;
            }
        }
        return totalNeigbours;
    }

    private int getTotalNeigbours(int row, int col) {
        int total = 0;
        for (int i = 0; i < _dirs.length; i++) {
            _advancedInDir[i] = getNeigboursInDir(row, col, _dirs[i]);
            total += _advancedInDir[i];
        }
        return total;
    }

    private int getNeigboursInDir(int row, int col, int[] dir) {
        int total = 0;
        int newRow = row + dir[0];
        int newCol = col + dir[1];

        while (validPos(newRow, newCol)) {
            if (_gameMatrix[newRow][newCol] < 0) break;
            total++;
            newRow = newRow + dir[0];
            newCol = newCol + dir[1];
        }

        return total;
    }

    private int getRandomDir(int[] usedDirs) {
        //Try random dir
        int cont = 0;
        int dir = 0;
        boolean existNewDir = false;

        while (cont < 4 && !existNewDir) {
            dir = _rand.nextInt(_dirs.length); //Random dir
            if (usedDirs[dir] == -1) existNewDir = true;
            cont++;//If this dir is not used we got a new dir
        }
        //If we cant get a random dir try to get the first one not used
        if (!existNewDir) {
            for (dir = 0; dir < usedDirs.length && !existNewDir; dir++)
                if (usedDirs[dir] == -1) existNewDir = true;
        }
        return (existNewDir) ? dir-1 : -1;
    }

    private boolean validPos(int row, int col) {
        return (row >= 0 && row < _rows) && (col >= 0 && col < _cols);
    }

    private int[][] _dirs;
    // ROJO, AZUL, AZUL CON NEIGHBOURS
    // { -1 , 0, neighbours}
    private int[][] _gameMatrix;
    private int[] _advancedInDir;
    private boolean _procesed[][];
    private java.util.Random _rand;
    private int _rows;
    private int _cols;
    private HashMap<Integer, int[]> _freePositions;

}
