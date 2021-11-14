package es.fdi.ucm.gdv.vdism.maranwi.logica;

import java.util.ArrayList;
import java.util.List;
import java.util.Random;

public class HintsManager {

    public HintsManager(){
        _board = null;
        _hintsList = new ArrayList<Hint>();
    }

    enum HintType{
        ONE,TWO,THREE,FOUR,FIVE,SIX,SEVEN,
        NONE
    }

    public void setBoard(Celda[][] b){
        _board = b;
    }

    public Hint getRandomHint(){
        //No hay pistas
        if(_hintsList.isEmpty()) return new Hint(HintType.NONE,-1,-1);

        //Si el jugador comete un error, es la primera pista que se da
        if(_playerError){
            return _hintsList.get(_hintsList.size()-1);
        }
        //Si no, escoge una aleatoria
        else {
            Random rand = new Random();
            return _hintsList.get(rand.nextInt(_hintsList.size()));
        }
    }

    /**
     * Actualiza la lista de pistas, revisa en el tablero de juego que pistas se añaden
     */
    public void updateHintsList(){
        _hintsList.clear();

        _playerError = false;

        for (int i = 0; i < _board[0].length; ++i) {
            for (int j = 0; j < _board[1].length; ++j) {
                Celda currentCelda = _board[i][j];
                //Si es un numero azul, se comprueban la pista 4 , 5 , 1 , 2 y 3
                if(currentCelda.getRequiredNeighbours() != -1 && currentCelda.getTipoCelda() == TipoCelda.Azul){
                    int[] hintsInfo = calculateHintsInfo(i,j);
                    //PISTA 4 es un error del jugador (no pasaran en la generacion del tablero)
                    if(hintsInfo[0] > currentCelda.getRequiredNeighbours()){
                        _playerError = true;
                        _hintsList.add(new Hint(HintType.FOUR,i,j));
                        return;
                    }
                    //PISTA 5 es un error del jugador (no pasaran en la generacion del tablero)
                    else if(hintsInfo[1]==0 && hintsInfo[0] < currentCelda.getRequiredNeighbours()){
                        _playerError = true;
                        _hintsList.add(new Hint(HintType.FIVE,i,j));
                        return;
                    }
                    //PISTA 1
                    else if(hintsInfo[1]==1 && hintsInfo[0] == currentCelda.getRequiredNeighbours()){
                        _hintsList.add(new Hint(HintType.ONE,i,j));
                        continue;
                    }
                    //PISTA 2
                    int[] hint2 = calculateHint2( i, j, currentCelda.getRequiredNeighbours());
                    if(hint2[0]!=0){
                        Hint p = new Hint(HintType.TWO, i, j);
                        p.setWhereToApply(hint2[1], hint2[2]);
                        _hintsList.add(p);
                    }
                    //PISTA 3
                    int[] hint3 = calculateHint3( i, j, currentCelda.getRequiredNeighbours());
                    if(hint3[0]!=0){
                        Hint p = new Hint(HintType.THREE, i, j);
                        p.setWhereToApply(hint2[1], hint2[2]);
                        _hintsList.add(p);
                    }
                }
                //Si es un azul no numero, o blanco, y esta cerrada, se comprueban la pista 6 y 7
                else if(currentCelda.getRequiredNeighbours()==-1 && currentCelda.getTipoCelda() != TipoCelda.Rojo && isClosed(i,j)){
                    //PISTA 6 y 7
                    Hint p = (currentCelda.getTipoCelda()==TipoCelda.Blanco) ? new Hint(HintType.SIX,i,j) : new Hint(HintType.SEVEN,i,j);
                    _hintsList.add(p);
                }
            }
        }
    }

    /**
     * Calcula y devuelve la informacion para las pistas de error del jugador y la pista 1
     * @return arr {NumeroAzulesVisibles, EstaAbierta en alguna direccion}
     */
    private int[] calculateHintsInfo(int x, int y){
        // {Azules, EstaAbierta}
        int[] arr = new int[2];

        for(int[] d : _dirs){
            int currentPosX = x + d[0];
            int currentPosY = y + d[1];
            while (validPos(currentPosX,currentPosY)){
                if (_board[currentPosX][currentPosY].getTipoCelda() == TipoCelda.Azul){
                    arr[0]++;
                }
                //Esta abierta
                else if (_board[currentPosX][currentPosY].getTipoCelda() != TipoCelda.Rojo){
                    arr[1] = 1;
                    break;
                }
                else{
                    break;
                }
                currentPosX += d[0];
                currentPosY += d[1];
            }
        }

        return arr;
    }

    /**
     * Calcula y devuelve la informacion para la pista 2
     * @return arr {CumplePista,PosXAplicar,PosYAplicar}
     */
    private int[] calculateHint2(int x, int y, int requiredNumber){
        int[] arr = new int[3];

        for(int[] d : _dirs){
            int blues = 0;
            int currentPosX = x + d[0];
            int currentPosY = y + d[1];
            //Si en esa direccion hay una blanca, la cuenta como azul y sigue contando en esa direccion
            if(validPos(currentPosX,currentPosY) && _board[currentPosX][currentPosY].getTipoCelda() == TipoCelda.Blanco){
                blues++;
            }else continue;

            blues += countBluesAtDir(currentPosX, currentPosY, d);

            //¿Excederia el numero al haber colocado azul en esa direccion?
            if(blues > requiredNumber){
                arr[0] = 1; //true
                arr[1] = x+d[0]; //posX de celda que se deberia cerrar
                arr[2] = y+d[1]; //posY de celda que se deberia cerrar
                break;
            }

        }

        return arr;
    }

    /**
     * Calcula y devuelve la informacion para la pista 3
     * @return arr {CumplePista,PosXAplicar,PosYAplicar}
     */
    private int[] calculateHint3(int x, int y, int requiredNumber){
        int[] arr = new int[3];

        int[] countDirs = new int[4];
        int total = 0;

        //Array para contar en todas las direcciones
        for (int i = 0; i<countDirs.length; i++){
            countDirs[i] = possiblesAtDir(x,y,_dirs[i]);
            total += countDirs[i];
        }

        for (int i = 0; i < _dirs.length; i++){
            //La direccion elegida tiene inmediatamente una pared o un limite, se pasa a la siguiente
            if(countDirs[i]==0){
                continue;
            }
            int[] dir = _dirs[i];
            //countOtras las posibles que tienen el resto de direcciones = (las posibles en todas las direcciones - las posibles en esta)
            int countOtras = total - countDirs[i];
            //se suman a countOtras las azules adyacentes
            int azulesAdyacentes = countBluesAtDir(x,y,dir);
            countOtras += azulesAdyacentes;

            //Si esa direccion es imprescindible, porque sin ella no se llega al numero requerido
            if(countOtras < requiredNumber){
                arr[0] = 1; // true
                arr[1] = x + dir[0] * (azulesAdyacentes + 1); //posX de celda que deberia ser azul si o si
                arr[2] = y + dir[1] * (azulesAdyacentes + 1) ; //posY de celda que deberia ser azul si o si
                break;
            }
        }

        return arr;
    }

    /**
     * Comprobar limites
     */
    private boolean validPos(int x, int y){
        return (x >= 0 && x < _board[0].length) && (y >= 0 && y < _board[1].length);
    }

    /**
     * Calcula las fichas azules (sin saltos de espacios en blanco) en una direccion
     */
    private int countBluesAtDir(int x, int y, int[] dir){
        int blues = 0;
        int currentPosX = x + dir[0];
        int currentPosY = y + dir[1];

        while (validPos(currentPosX,currentPosY) && _board[currentPosX][currentPosY].getTipoCelda() == TipoCelda.Azul){
            blues++;
            currentPosX += dir[0];
            currentPosY += dir[1];
        }

        return blues;
    }

    /**
     * Cuenta cuantas fichas posibles hay en una direccion (azules y blancas) hasta una pared o limite, dada una posicion
     */
    private int possiblesAtDir(int x, int y, int[] dir){
        int count = 0;

        int currentPosX = x + dir[0];
        int currentPosY = y + dir[1];

        while (validPos(currentPosX,currentPosY)){
            if(_board[currentPosX][currentPosY].getTipoCelda() == TipoCelda.Rojo){
                break;
            }
            count++;
            currentPosX += dir[0];
            currentPosY += dir[1];
        }

        return count;
    }

    /**
     * Comprueba si esta posicion esta cerrada por paredes y limites
     */
    private boolean isClosed(int x, int y){
        for(int[] d : _dirs){
            int currentPosX = x + d[0];
            int currentPosY = y + d[1];
            if(validPos(currentPosX,currentPosY) && _board[currentPosX][currentPosY].getTipoCelda() != TipoCelda.Rojo){
                return false;
            }
        }
        return true;
    }

    private Celda[][] _board;
    private List<Hint> _hintsList;

    /** Direcciones en el tablero {Derecha, Abajo, Izquierda, Arriba} */
    private int[][] _dirs = {{1,0},{0,-1},{-1,0},{0,1}};

    private boolean _playerError = false;
}
