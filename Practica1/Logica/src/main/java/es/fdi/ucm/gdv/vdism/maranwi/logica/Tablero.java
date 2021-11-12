package es.fdi.ucm.gdv.vdism.maranwi.logica;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;
import java.util.Random;
import java.util.Stack;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Font;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Image;

public class Tablero {
    public Tablero(int filas, int columnas) {
        _filas = filas;
        _columnas = columnas;

        _matrizSolucion = new int[_filas][_columnas];
        _matrizJuego = new Celda[_filas][_columnas];

        _numFichasBlancas = 0;
        _moves = new Stack<Move>();

        _hintsList = new ArrayList<Pista>();

        _lockTokensList = new LinkedList<Celda>();
        _showLockImgs = false;

        //generaTablero();
    }

    public Celda[][] getMatrizJuego() {
        return _matrizJuego;
    }

    /**
     * Obtiene una pista aleatoria, en caso de error del jugador, devolvera una pista de error primero
     */
    public Pista getAHint(){

        updateHintsList();

        //No hay pistas
        if(_hintsList.isEmpty()) return new Pista(Pista.HintType.NONE,-1,-1);

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

    public void setLockImg(Image img) { _lockImg = img;}

    public void generaTablero(){
        boolean valid = false;
        while (!valid){
            //Genera una _matrizJuego ocultando casillas aleatorias y una _matrizSolucion con los colores azul y rojo que deberian ir en todas las casillas

            //Intenta resolverlo mediante pistas
            Pista p = getAHint();
            //Se podria comprobar tambien si ha sido relleno para evitar que se quede atascado en el bucle dando pistas.
            while (p.getHintType()!= Pista.HintType.NONE){
                //Se aplican a _matrizJuego
                applyHint(p);
                p = getAHint();
            }

            //Si se consigue resolver, comparar _matrizJuego con _matrizSolucion
            boolean iguales = true;
            for (int i = 0; i < _matrizJuego[0].length && iguales; ++i) {
                for (int j = 0; j < _matrizJuego[1].length && iguales; ++j) {
                    if(_matrizJuego[i][j].getTipoCeldaAsInt() != _matrizSolucion[i][j])
                        iguales = false;
                }
            }
            //Si son iguales, es valida
            valid = iguales ? true : false;
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
                if (_matrizJuego[currentPosX][currentPosY].getTipoCelda() == TipoCelda.Azul){
                    arr[0]++;
                }
                //Esta abierta
                else if (_matrizJuego[currentPosX][currentPosY].getTipoCelda() != TipoCelda.Rojo){
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
            if(validPos(currentPosX,currentPosY) && _matrizJuego[currentPosX][currentPosY].getTipoCelda() == TipoCelda.Blanco){
                blues++;
            }

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
        return (x >= 0 && x < _columnas) && (y >= 0 && y < _filas);
    }

    /**
     * Calcula las fichas azules (sin saltos de espacios en blanco) en una direccion
     */
    private int countBluesAtDir(int x, int y, int[] dir){
        int blues = 0;
        int currentPosX = x + dir[0];
        int currentPosY = y + dir[1];

        while (validPos(currentPosX,currentPosY) && _matrizJuego[currentPosX][currentPosY].getTipoCelda() == TipoCelda.Azul){
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
            if(_matrizJuego[currentPosX][currentPosY].getTipoCelda() == TipoCelda.Rojo){
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
            if(validPos(currentPosX,currentPosY) && _matrizJuego[currentPosX][currentPosY].getTipoCelda() != TipoCelda.Rojo){
                return false;
            }
        }
        return true;
    }

    /**
     * Actualiza la lista de pistas, revisa en el tablero de juego que pistas se añaden
     */
    private void updateHintsList(){
        _hintsList.clear();

        _playerError = false;

        for (int i = 0; i < _matrizJuego[0].length; ++i) {
            for (int j = 0; j < _matrizJuego[1].length; ++j) {
                Celda currentCelda = _matrizJuego[i][j];
                //Si es un numero azul, se comprueban la pista 4 , 5 , 1 , 2 y 3
                if(currentCelda.getRequiredNeighbours() != -1 && currentCelda.getTipoCelda() == TipoCelda.Azul){
                    int[] hintsInfo = calculateHintsInfo(i,j);
                    //PISTA 4 es un error del jugador (no pasaran en la generacion del tablero)
                    if(hintsInfo[0] > currentCelda.getRequiredNeighbours()){
                        _playerError = true;
                        _hintsList.add(new Pista(Pista.HintType.FOUR,i,j));
                        return;
                    }
                    //PISTA 5 es un error del jugador (no pasaran en la generacion del tablero)
                    else if(hintsInfo[1]==0 && hintsInfo[0] < currentCelda.getRequiredNeighbours()){
                        _playerError = true;
                        _hintsList.add(new Pista(Pista.HintType.FIVE,i,j));
                        return;
                    }
                    //PISTA 1
                    else if(hintsInfo[1]==1 && hintsInfo[0] == currentCelda.getRequiredNeighbours()){
                        _hintsList.add(new Pista(Pista.HintType.ONE,i,j));
                        continue;
                    }
                    //PISTA 2
                    int[] hint2 = calculateHint2( i, j, currentCelda.getRequiredNeighbours());
                    if(hint2[0]!=0){
                        Pista p = new Pista(Pista.HintType.TWO, i, j);
                        p.setWhereToApply(hint2[1], hint2[2]);
                        _hintsList.add(p);
                    }
                    //PISTA 3
                    int[] hint3 = calculateHint3( i, j, currentCelda.getRequiredNeighbours());
                    if(hint3[0]!=0){
                        Pista p = new Pista(Pista.HintType.THREE, i, j);
                        p.setWhereToApply(hint2[1], hint2[2]);
                        _hintsList.add(p);
                    }
                }
                //Si es un azul no numero, o blanco, y esta cerrada, se comprueban la pista 6 y 7
                else if(currentCelda.getRequiredNeighbours()==-1 && currentCelda.getTipoCelda() != TipoCelda.Rojo && isClosed(i,j)){
                    //PISTA 6 y 7
                    Pista p = (currentCelda.getTipoCelda()==TipoCelda.Blanco) ? new Pista(Pista.HintType.SIX,i,j):new Pista(Pista.HintType.SEVEN,i,j);
                    _hintsList.add(p);
                }
            }
        }
    }

    /**
     * Cierra con paredes en todas las direcciones desde el punto x y
     */
    private void close(int x, int y){
        for(int[] d : _dirs){
            int currentPosX = x + d[0];
            int currentPosY = y + d[1];
            while (validPos(currentPosX,currentPosY)){
                //Si se encuentra una pared, deja de ir en esa direccion
                if(_matrizJuego[currentPosX][currentPosY].getTipoCelda() == TipoCelda.Rojo){
                    break;
                }
                //Si se encuentra una celda Blanca en esa direccion, la hace roja y deja de ir en esa direccion
                else if(_matrizJuego[currentPosX][currentPosY].getTipoCelda() == TipoCelda.Blanco){
                    _matrizJuego[currentPosX][currentPosY].setTipo(TipoCelda.Rojo);
                    break;
                }
                currentPosX += d[0];
                currentPosY += d[1];
            }

        }
    }

    /**
     * Aplicar pistas
     */
    private void applyHint(Pista hint){
        int[] pos = hint.getPos();

        switch (hint.getHintType()){
            case ONE:
                close(pos[0],pos[1]);
                break;
            case TWO:{
                int[] posToApply = hint.getWhereToApply();
                _matrizJuego[posToApply[0]][posToApply[1]].setTipo(TipoCelda.Rojo);
            }
                break;
            case THREE:{
                int[] posToApply = hint.getWhereToApply();
                _matrizJuego[posToApply[0]][posToApply[1]].setTipo(TipoCelda.Azul);
                break;
            }
            case FOUR:
                //No se deberia aplicar
                break;
            case FIVE:
                //No se deberia aplicar
                break;
            case SIX:
                _matrizJuego[pos[0]][pos[1]].setTipo(TipoCelda.Rojo);
                break;
            case SEVEN:
                _matrizJuego[pos[0]][pos[1]].setTipo(TipoCelda.Rojo);
                break;
        }
    }

    public void rellenaMatrizResueltaRandom(int RAD, int BOARD_LOGIC_OFFSET_X, int BOARD_LOGIC_OFFSET_Y, Font font, MyColor fontColor) {
        java.util.Random r = new Random();
        for (int x = 0; x < _matrizSolucion[0].length; ++x) {
            for (int j = 0; j < _matrizSolucion[1].length; ++j) {
                //0 = Azul, 1 = Rojo
                _matrizSolucion[x][j] = r.nextInt(2);

                boolean esFicha = r.nextBoolean();
                Celda c;
                //Fil(f) col(c)
                //<f,c> to int => f * COL + c
                //Int to <f,c> => f = n / COLS, c = n % COL
                int id = x * _matrizJuego[0].length + j;

                if (esFicha) {
                    c = new Celda(id,esFicha,TipoCelda.values()[_matrizSolucion[x][j]],-1,x,j,RAD,BOARD_LOGIC_OFFSET_X,BOARD_LOGIC_OFFSET_Y,font,fontColor);
                    ++_numFichasBlancas;
                } else {
                    //Si no es ficha se calculan los posibles vecinos y se instancia la celda
                    int neigbours = _matrizSolucion[x][j] == 0 ? r.nextInt(3) + 1 : -1;
                    c = new Celda(id, esFicha, TipoCelda.values()[_matrizSolucion[x][j]], neigbours, x, j, RAD, BOARD_LOGIC_OFFSET_X, BOARD_LOGIC_OFFSET_Y, font, fontColor);

                    //Si no es azul numérica(no tiene vecinos) es candado, se configura su imágen y se añade a la lista de tokens
                    if(neigbours == -1){
                        c.getButton().setImage(_lockImg, _lockImg.getWidth() / 2, _lockImg.getHeigth() / 2, false);
                        _lockTokensList.add(c);
                    }
                }

                _matrizJuego[x][j] = c;
            }
        }
    }

    public void nextColor(int row, int col){
        _moves.push(new Move(row, col, _matrizJuego[row][col].getTipoCelda()));
        checkResult(_matrizJuego[row][col].cambiarFicha(true));
    }

    public Celda restoreMove(){
        if (!_moves.empty()) {
            Move last=_moves.pop();
            checkResult(_matrizJuego[last.getX()][last.getY()].cambiarFicha(false));
            System.out.println("Move restored: " + last.getX() + "," + last.getY() + " with value: " + last.getType() + " Numblancas: " + _numFichasBlancas);
            return _matrizJuego[last.getX()][last.getY()];
        }
        return null;
    }

    public void showLockImgs(){
        _showLockImgs = !_showLockImgs;
        for(int x = 0; x<_lockTokensList.size(); ++x){
            _lockTokensList.get(x).getButton().setShowImg(_showLockImgs);
        }
    }

    private void checkResult(int result){
        //Result = 0 -> no changes
        //Result = 1 ->  -white token
        //Result = 2 ->  +white token
        if(result == 1) --_numFichasBlancas;
        else if(result == 2) ++_numFichasBlancas;
    }

    /*public boolean compruebaSolucion() {
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
    }*/

    /*public void setColor(int col, int row, TipoCelda tipo) {
        if (col >= 0 && col < _matrizJuego[0].length && row >= 0 && row < _matrizJuego.length && _matrizJuego[row][col].getEsFicha()) {
            boolean wasWhite = _matrizJuego[row][col].getTipoCelda() == TipoCelda.Blanco;
            _matrizJuego[row][col].setTipo(tipo);
            if (wasWhite && tipo != TipoCelda.Blanco)
                _numFichasBlancas--;
            else if (!wasWhite && tipo == TipoCelda.Blanco)
                _numFichasBlancas++;
        }
    }*/

    //La matriz solucíón solo guarda los colores de cada posición
    private int _matrizSolucion[][];
    private Celda _matrizJuego[][];
    private int _filas;
    private int _columnas;
    private  int _numFichasBlancas;
    private Stack<Move> _moves;

    private Image _lockImg;

    private List<Pista> _hintsList;
    private boolean _playerError = false;

    /** Direcciones en el tablero {Derecha, Abajo, Izquierda, Arriba} */
    private int[][] _dirs = {{1,0},{0,-1},{-1,0},{0,1}};

    LinkedList<Celda> _lockTokensList;
    boolean _showLockImgs;
}
