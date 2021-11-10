package es.fdi.ucm.gdv.vdism.maranwi.pc;


public class Pista {
    public void aplicar(Tablero t, boolean playing) {
        _currentHint = "";
        Celda[][] tablero = t.getMatrizJuego();
        //todo encontrar la clase pair, si es que existe
        int[][] dirs = new int[4][2];
        dirs[0][0] = 1; //derecha
        dirs[0][1] = 0;
        dirs[1][0] = -1;//izquierda
        dirs[1][1] = 0;
        dirs[2][0] = 0;//abajo
        dirs[2][1] = 1;
        dirs[3][0] = 0;//arriba
        dirs[3][1] = -1;
        //Pista 1 (lleva implícita la detección de la pista 4 con el bool Aplicable)
        //boolean applied = false;
        //QUITAR LAS DOS SIGUIENTES LÍNEAS Y DESCOMENTAR LA ANTERIOR => MODO TESTEO  ---------- IMPORTANTE BORRAR ESTO !!!!!!!!!!!!!!!!!!!!!!!!!!!
        //QUITAR LAS DOS SIGUIENTES LÍNEAS Y DESCOMENTAR LA ANTERIOR => MODO TESTEO  ---------- IMPORTANTE BORRAR ESTO !!!!!!!!!!!!!!!!!!!!!!!!!!!
        //QUITAR LAS DOS SIGUIENTES LÍNEAS Y DESCOMENTAR LA ANTERIOR => MODO TESTEO  ---------- IMPORTANTE BORRAR ESTO !!!!!!!!!!!!!!!!!!!!!!!!!!!
        //QUITAR LAS DOS SIGUIENTES LÍNEAS Y DESCOMENTAR LA ANTERIOR => MODO TESTEO  ---------- IMPORTANTE BORRAR ESTO !!!!!!!!!!!!!!!!!!!!!!!!!!!
        boolean applied = true;
        _currentHint = "Si se pone una azul en una determinada direccion\nquedairresoluble";
        int[] offsets = new int[4];
        for (int i = 0; i < tablero[0].length && !applied; i++) {
            for (int j = 0; j < tablero[1].length && !applied; j++) {
                boolean Applicable = true;
                //si es fija
                if (!tablero[i][j].getEsFicha() && tablero[i][j].getTipoCelda() == TipoCelda.Azul) {
                    int total = 0;
                    //miramos si nos podemos saltar esta pista, ya que si tiene más vecinos de la cuenta está mal,
                    //a la vez que rellenamos las distancias a las fichas que no son azules para usarlas luego
                    //mientras que contamos el número de azules circundantes
                    for (int k = 0; k < dirs.length; k++) {
                        offsets[k] = countTargetColor(tablero, j, i, dirs[k][0], dirs[k][1], TipoCelda.Azul, true);
                        total += offsets[k] - 1;
                        if (total > tablero[i][j].getRequiredNeighbours()) {
                            Applicable = false;
                            //Pista 4 (la 4 y la 5 no se pueden "ejecutar", ya que no sabrías cuál de todas debes quitar ya que
                            // puede que estuviera bien
                            if (playing)
                                _currentHint = "Demasiadas fichas azules en un numero";
//                            applied=true;
                        }

                    }
                    //si no nos hemos pasado
                    if (Applicable && total == tablero[i][j].getRequiredNeighbours()) {
                        if (playing) {
                            _currentHint = "Hay que cerrar la casilla";
                            applied = true;
                        } else {
                            for (int k = 0; k < dirs.length; k++) {
                                int targetX = j + dirs[k][0] * offsets[k];
                                int targetY = i + dirs[k][1] * offsets[k];
                                if (targetX >= 0 && targetX < tablero[1].length && targetY >= 0 && targetY < tablero[0].length
                                        && tablero[targetY][targetX].getTipoCelda() == TipoCelda.Blanco) {
                                    t.setColor(targetX, targetY, TipoCelda.Rojo);
                                    applied = true;
                                }
                            }
                        }
                    }
                    //Pista 2 (el if solo comprueba si es aplicable ya que si total es mayor que las requeridas aplicable va a
                    //ser false y si es igual se encarga el caso de arriba)
                    else if (Applicable) {
                        int numRojas = 0;
                        for (int k = 0; k < dirs.length; k++) {
                            int targetX = j + dirs[k][0] * offsets[k];
                            int targetY = i + dirs[k][1] * offsets[k];
                            //Si el target existe y es blanco
                            if (targetX >= 0 && targetX < tablero[0].length && targetY >= 0 && targetY < tablero[1].length) {
                                if (tablero[targetY][targetX].getTipoCelda() == TipoCelda.Blanco) {//todo si encontramos las pairs podríamos implementar un count que pare en cuanto alcance un valor y
                                    //todo devuelva un pair booleano, int diciendo si se ha pasado y cuántas fichas ha contado
                                    //se suma 1 en el if porque si pusieramos una azul en el target habría una más
                                    //p es el indice de la dirección opuesta
                                    int p = k % 2 == 0 ? 1 : -1;
                                    if (countTargetColor(tablero, targetX + dirs[k][0], targetY + dirs[k][1], dirs[k][0], dirs[k][1], TipoCelda.Azul, true) + 1
                                            + offsets[k] + offsets[k + p]
                                            > tablero[i][j].getRequiredNeighbours()) {
                                        if(!playing)
                                        t.setColor(targetX, targetY, TipoCelda.Rojo);
                                        applied = true;
                                    }

                                    //Si por el contrario es roja está cerrada en esa dirección
                                } else if (tablero[targetY][targetX].getTipoCelda() == TipoCelda.Rojo)
                                    numRojas++;
                                //Si no existe el target también está cerrada en esa dirección
                            } else numRojas++;
                            //pista 5
                            // Si estamos cerrados y no hay suficientes vecinos nos hemos equivocado
                            //la segunda comprobación creo que no hace falta porque si nos hemos pasado de azules Applicable
                            //es false y no entra aquí y si aplicable es true y son justas entra en el otro if
                            if (numRojas == 4 && total < tablero[i][j].getRequiredNeighbours()) {
                                if (playing)
                                    _currentHint = "Demasiado pocos azules en un numero";
//                                applied = true;
                            }
                            //Si hubiera tenido que aplicarla y estoy jugando
                            if(applied && playing)
                                _currentHint = "Si se pone una azul en una determinada direccion\nquedairresoluble";
                        }
                        ////////////////////
                        total = 0;
                        boolean tieneSolucion = true;
                        for (int k = 0; k < dirs.length && tieneSolucion; k++) {
                            offsets[k] = countTargetColor(tablero, j, i, dirs[k][0], dirs[k][1], TipoCelda.Rojo, false);
                            if (k > 0 && offsets[k] >= tablero[i][j].getRequiredNeighbours() && total >= tablero[i][j].getRequiredNeighbours())
                                tieneSolucion = false;
                            total += offsets[k];
                        }
                        if (total < tablero[i][j].getRequiredNeighbours())
                            tieneSolucion = false;
                        if (playing && tieneSolucion)
                            _currentHint = "Hay una ficha comun en todas las posibles soluciones";
                    }
                }
                //pistas 6 y 7
                else if (tablero[i][j].getEsFicha()) {
                    //si es una ficha vacía y está cerrada o es una azul y está cerrada(ergo el usuario se ha equivocado)
                    //hay que poner rojo
                    //todo no se si habría que comprobar que las rojas no las ha puesto el usuario (es decir su getFicha es true)
                    if (tablero[i][j].getTipoCelda() != TipoCelda.Rojo && isClosed(tablero, i, j, dirs)) {
                        if (playing) {
                            if (tablero[i][j].getTipoCelda() == TipoCelda.Azul)
                                _currentHint = "Hay una azul que no ve ningun vecino";
                            else _currentHint = "Hay una blanca que se puede cerrar";
                        } else
                            t.setColor(i, j, TipoCelda.Rojo);
                        applied = true;
                    }
                }

            }
        }
    }


    private int countTargetColor(Celda[][] tablero, int X, int Y, int dirX, int dirY, TipoCelda target, boolean equals) {
        boolean counted = false;
        int number = 0;
        int myX = X, myY = Y;
        for (int i = 0; i < tablero.length && !counted; i++) {
            for (int j = 0; j < tablero[0].length && !counted; j++) {
                //si me salgo paro
                if (myX >= tablero[0].length || myX < 0 || myY >= tablero.length || myY < 0)
                    return number;
                if ((equals && tablero[myY][myX].getTipoCelda() == target) || (!equals && tablero[myY][myX].getTipoCelda() != target))
                    number++;
                else
                    counted = true;
                myX += dirX;
                myY += dirY;

            }
        }
        return number;
    }

    private boolean isClosed(Celda[][] tablero, int X, int Y, int[][] dirs) {
        int closedSides = 0;
        for (int k = 0; k < dirs.length; k++) {
            int targetX = X + dirs[k][0];
            int targetY = Y + dirs[k][1];
            //si me salgo
            if (targetX < 0 || targetX >= tablero[0].length || targetY < 0 || targetY >= tablero.length
                    //o mi adyacente es roja estoy cerrado en esa dirección
                    || tablero[targetY][targetX].getTipoCelda() == TipoCelda.Rojo)
                closedSides += 1;


        }
        return closedSides == 4;
    }


    public String getCurrentHint() { return _currentHint;}


    private String _currentHint;
}

