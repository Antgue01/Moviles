package es.fdi.ucm.gdv.vdism.maranwi.pc;
import java.util.ArrayList;
import java.util.List;
import java.util.Random;
import java.util.Stack;

import javax.swing.event.HyperlinkEvent;

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
    }

    /*
        0 = Azul
        1 = Rojo
     */

    private boolean validPos(int x, int y){
        return (x >= 0 && x < _matrizJuego[0].length) &&
                (y >= 0 && y < _matrizJuego[1].length);
    }

    private int[] getErrorHints(int x, int y){
        // {Azules, EstaAbierta}
        int[] arr = {0,0};

        for(int[] d : dirs){
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

    private int[] calculateHint2(int x, int y, int number){
        int[] arr = {0,0,0};

        for(int[] d : dirs){
            int blues = 0;
            int currentPosX = x + d[0];
            int currentPosY = y + d[1];
            //Si en esa direccion hay una blanca, la cuenta como azul y sigue contando en esa direccion
            if(validPos(currentPosX,currentPosY) && _matrizJuego[currentPosX][currentPosY].getTipoCelda() == TipoCelda.Blanco){
                blues++;
                currentPosX += d[0];
                currentPosY += d[1];
            }
            while (validPos(currentPosX,currentPosY)){
                if (_matrizJuego[currentPosX][currentPosY].getTipoCelda() == TipoCelda.Azul){
                    blues++;
                }
                else{
                    break;
                }
                currentPosX += d[0];
                currentPosY += d[1];
            }
            //¿Excederia el numero al haber colocado azul en esa direccion?
            if(blues > number){
                arr[0] = 1; //true
                arr[1] = x+d[0]; //posX de celda que se deberia cerrar
                arr[2] = y+d[1]; //posY de celda que se deberia cerrar
            }

        }

        return arr;
    }

    private int[] calculateHint3(int x, int y){
        int[] arr = {0,0,0};

        for(int[] d : dirs){
            int blues = 0;
            int currentPosX = x + d[0];
            int currentPosY = y + d[1];

        }

        return arr;
    }

    private boolean isClosed(int x, int y){
        for(int[] d : dirs){
            int currentPosX = x + d[0];
            int currentPosY = y + d[1];
            if(validPos(currentPosX,currentPosY) && _matrizJuego[currentPosX][currentPosY].getTipoCelda() != TipoCelda.Rojo){
                return false;
            }
        }
        return true;
    }

    private void updateHintsList(){
        _hintsList.clear();

        _playerError = false;

        for (int i = 0; i < _matrizJuego[0].length; ++i) {
            for (int j = 0; j < _matrizJuego[1].length; ++j) {
                Celda currentCelda = _matrizJuego[i][j];
                //Si es un numero azul, se comprueban la pista 4 , 5 , 1
                if(!currentCelda.getEsFicha() && currentCelda.getTipoCelda() == TipoCelda.Azul){
                    int[] hintsInfo = getErrorHints(i,j);
                    //PISTA 4 es un error del jugador (no pasaran en la generacion del tablero)
                    if(hintsInfo[0] > currentCelda.getRequiredNeighbours()){
                        _playerError = true;
                        _hintsList.add(new Pista(Pista.HintType.FOUR,i,j));
                        return;
                    }
                    //PISTA 5 es un error del jugador (no pasaran en la generacion del tablero)
                    else if(hintsInfo[1]!=0 && hintsInfo[0] < currentCelda.getRequiredNeighbours()){
                        _playerError = true;
                        _hintsList.add(new Pista(Pista.HintType.FIVE,i,j));
                        return;
                    }
                    //PISTA 1
                    else if(hintsInfo[1]!=1 && hintsInfo[0] == currentCelda.getRequiredNeighbours()){
                        _hintsList.add(new Pista(Pista.HintType.ONE,i,j));
                    }
                    //PISTA 2
                    int[] hint2 = calculateHint2( i, j, currentCelda.getRequiredNeighbours());
                    if(hint2[0]!=0){
                        Pista p = new Pista(Pista.HintType.TWO, i, j);
                        p.setWhereToApply(hint2[1], hint2[2]);
                        _hintsList.add(p);
                    }
                    //PISTA 3
                    int[] hint3 = calculateHint3( i, j);
                    if(hint3[0]!=0){
                        Pista p = new Pista(Pista.HintType.THREE, i, j);
                        p.setWhereToApply(hint2[1], hint2[2]);
                        _hintsList.add(p);
                    }
                }
                //Si es un azul no numero, o blanco, se comprueban la pista 6 y 7
                else if(currentCelda.getEsFicha() && currentCelda.getTipoCelda() != TipoCelda.Rojo && isClosed(i,j)){
                    //PISTA 6 y 7
                    Pista p = (currentCelda.getTipoCelda()==TipoCelda.Blanco) ? new Pista(Pista.HintType.SIX,i,j):new Pista(Pista.HintType.SEVEN,i,j);
                }
            }
        }
    }

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


    public void rellenaMatrizResueltaRandom(int RAD, int BOARD_LOGIC_OFFSET_X, int BOARD_LOGIC_OFFSET_Y, Font font, int fontColor) {
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
                    c = new Celda(id, esFicha, TipoCelda.Blanco, -1, x, j, RAD, BOARD_LOGIC_OFFSET_X, BOARD_LOGIC_OFFSET_Y, font, fontColor);
                    ++_numFichasBlancas;
                } else {
                    int neigbours = _matrizSolucion[x][j] == 0 ? r.nextInt(3) + 1 : -1;
                    c = new Celda(id, esFicha, TipoCelda.values()[_matrizSolucion[x][j]], neigbours, x, j, RAD, BOARD_LOGIC_OFFSET_X, BOARD_LOGIC_OFFSET_Y, font, fontColor);
                    if(neigbours == -1) c.getButton().setImage(_lockImg, _lockImg.getWidth() / 2, _lockImg.getHeigth() / 2);
                }

                _matrizJuego[x][j] = c;
            }
        }
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

    public void nextColor(int row, int col){
        _moves.push(new Move(row, col, _matrizJuego[row][col].getTipoCelda()));
        checkResult(_matrizJuego[row][col].cambiarFicha(true));
    }

    public void restoreMove(){
        if (!_moves.empty()) {
            Move last=_moves.pop();
            checkResult(_matrizJuego[last.getX()][last.getY()].cambiarFicha(false));
            System.out.println("Move restored: " + last.getX() + "," + last.getY() + " with value: " + last.getType() + " Numblancas: " + _numFichasBlancas);
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

    public void setLockImg(Image img) { _lockImg = img;}

    private void checkResult(int result){
        //Result = 0 -> no changes
        //Result = 1 ->  -white token
        //Result = 2 ->  +white token
        if(result == 1) --_numFichasBlancas;
        else if(result == 2) ++_numFichasBlancas;
    }

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

    //derch, abajo, izq, arriba
    private int[][] dirs = {{1,0},{0,-1},{-1,0},{0,1}};

}
