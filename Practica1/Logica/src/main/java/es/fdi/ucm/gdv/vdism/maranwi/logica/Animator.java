package es.fdi.ucm.gdv.vdism.maranwi.logica;

import java.util.HashMap;
import java.util.Map;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;

public class Animator {

    public Animator(Tablero board){
        _board = board;
        _animations = new HashMap<String, Animations>();
    }

    public void addAnimationElement(int row, int col, boolean isRestoreMove, boolean isHint){
        Celda c = _board.getMatrizJuego()[row][col];
        Animations a = new Animations(c);
        if(c.getEsFicha() && !isRestoreMove){
            //Fade Animation
            _board.nextColor(row, col);
            MyColor cOut = c.getLastColor();
            MyColor cIn = c.getColor();
            a.startFadeAnimation(cIn, cOut);
        }
        else if(c.getEsFicha() && isRestoreMove){
            //Fade Animation
            _board.nextColor(row, col);
            MyColor cIn = c.getLastColor();
            MyColor cOut = c.getColor();
            a.startFadeAnimation(cIn, cOut);
        }
        else if(!c.getEsFicha() && !isHint){
            //Fast Move Animation
            _board.showLockImgs();
            a.startFastMoveAnimation();
        }
        else if(isHint){
            //Slow Move Animation
            MyColor borderColor = new MyColor(0x000000FF);
            a.startSlowMoveAnimation(borderColor);
        }
        _animations.put(c.getButton().getId(), a);
    }

    public void update(double deltaTime){
        for (Map.Entry<String, Animations> elem : _animations.entrySet()) {
            if(!elem.getValue().update(deltaTime)){
                //_animations.remove(elem);
                System.out.println("Animation in element " + elem.getValue().getId() + " end.");
            }
        }
    }

    public void render(Graphics g){
        //System.out.println(_animations.size() + " ");
        for (Map.Entry<String, Animations> elem : _animations.entrySet()) {
            //System.out.println("ENTRA");
            elem.getValue().render(g);
        }
    }

    Map<String, Animations> _animations;
    Tablero _board;
}
