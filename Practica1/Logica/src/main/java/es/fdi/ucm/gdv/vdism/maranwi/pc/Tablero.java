package es.fdi.ucm.gdv.vdism.maranwi.pc;
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
    }


    /*
        0 = Azul
        1 = Rojo
     */
    public void rellenaMatrizResueltaRandom(int RAD, int BOARD_LOGIC_OFFSET_X, int BOARD_LOGIC_OFFSET_Y, Font font, int fontColor) {
        java.util.Random r = new Random();
        for (int x = 0; x < _matrizSolucion[0].length; ++x) {
            for (int j = 0; j < _matrizSolucion[1].length; ++j) {
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

    public void nextColor(int row, int col){
        //Devuelve 0 si no hay cambios
        //Devuelve 1 si se cambia una ficha blanca a otro color - se pierde una blanca en el tablero -
        //Devuelve 2 si se cambia una ficha de otro color a blanca - se añade una blanca en el tablero -
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
}
