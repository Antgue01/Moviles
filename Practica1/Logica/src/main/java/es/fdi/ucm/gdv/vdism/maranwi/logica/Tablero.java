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

        _lockTokensList = new LinkedList<Celda>();
        _showLockImgs = false;

        _hintsManager = new HintsManager();

        //generaTablero();
    }

    public Celda[][] getMatrizJuego() {
        return _matrizJuego;
    }

    /**
     * Obtiene una pista aleatoria, en caso de error del jugador, devolvera una pista de error primero
     */
    public Hint getAHint(){

        _hintsManager.updateHintsList();

        return _hintsManager.getRandomHint();
    }

    public void setLockImg(Image img) { _lockImg = img;}

    public void generaTablero(){
        boolean valid = false;
        while (!valid){
            //Genera una _matrizJuego ocultando casillas aleatorias y una _matrizSolucion con los colores azul y rojo que deberian ir en todas las casillas

            //Intenta resolverlo mediante pistas
            Hint h = getAHint();
            //Se podria comprobar tambien si ha sido relleno para evitar que se quede atascado en el bucle dando pistas.
            while (h.getHintType() != HintsManager.HintType.NONE){
                //Se aplican a _matrizJuego
                applyHint(h);
                h = getAHint();
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
     * Comprobar limites
     */
    private boolean validPos(int x, int y){
        return (x >= 0 && x < _columnas) && (y >= 0 && y < _filas);
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
    private void applyHint(Hint hint){
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

        _hintsManager.setBoard(_matrizJuego);
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

    LinkedList<Celda> _lockTokensList;
    boolean _showLockImgs;

    HintsManager _hintsManager;

    /** Direcciones en el tablero {Derecha, Abajo, Izquierda, Arriba} */
    private int[][] _dirs = {{1,0},{0,-1},{-1,0},{0,1}};
}
