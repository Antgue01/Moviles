package es.fdi.ucm.gdv.vdism.maranwi.logica;

import java.util.HashMap;
import java.util.Map;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;

public class Animator {

    public Animator(Tablero board){
        _board = board;
        _animations = new HashMap<String, Animations>();
    }

    public void addAnimationElement(int row, int col, boolean isRestoreMove){
        Celda c = _board.getMatrizJuego()[row][col];
        if(c.getEsFicha() && !isRestoreMove){
            //_animations.put()
            _board.nextColor(row, col);
        }
        else if(!c.getEsFicha()){
            _board.showLockImgs();
        }
    }

    public void update(float deltaTime){
        for (Map.Entry<String, Animations> elem : _animations.entrySet()) {
            if(!elem.getValue().update(deltaTime))
                _animations.remove(elem);
        }
    }

    public void render(Graphics g){
        for (Map.Entry<String, Animations> elem : _animations.entrySet()) {
            elem.getValue().render(g);
        }
    }


    Map<String, Animations> _animations;
    Tablero _board;
    private static final double FADE_IN_TIME = 0;
    private static final double FADE_OUT_TIME = 0;
    private static final double FAST_MOVE_TIME = 0;
    private static final double SLOW_MOVE_TIME = 0;

    private static final double FADE_IN_VELOCITY = 0;
    private static final double FADE_OUT_VELOCITY = 0;
    private static final double FAST_MOVE_VELOCITY = 0;
    private static final double SLOW_MOVE_VELOCITY = 0;
}
