package es.fdi.ucm.gdv.vdism.maranwi.logica;

import java.util.HashMap;
import java.util.LinkedList;
import java.util.Map;
import java.util.Queue;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;

public class Animator {

    public Animator(Tablero board){
        _board = board;
        _animations = new HashMap<String, Animations>();
        _queue = new LinkedList<String>();
        _currentSlowMove = null;
    }

    public void addAnimationElement(int row, int col, boolean isRestoreMove, boolean isHint){
        clearSlowMove();

        Celda c = _board.getMatrizJuego()[row][col];
        Animations a = new Animations(c);
        if(c.getEsFicha() && !isRestoreMove && !isHint){
            //Fade Animation
            _board.nextColor(row, col);
            MyColor cOut = c.getLastColor();
            MyColor cIn = c.getColor();
            a.startFadeAnimation(cIn, cOut);
            //System.out.println("Fade in row: " + c.getButton().getBoardX() + " col: " + c.getButton().getBoardY());
        }
        else if(c.getEsFicha() && isRestoreMove && !isHint){
            //Fade Animation
            MyColor cIn = c.getLastColor();
            MyColor cOut = c.getColor();
            a.startFadeAnimation(cIn, cOut);
            //System.out.println("Fade out row: " + c.getButton().getBoardX() + " col: " + c.getButton().getBoardY());
        }
        else if(!c.getEsFicha() && !isHint){
            //Fast Move Animation
            _board.showLockImgs();
            a.startFastMoveAnimation();
            //System.out.println("FastMove in row: " + c.getButton().getBoardX() + " col: " + c.getButton().getBoardY());
        }
        else if(isHint){
            //Slow Move Animation
            MyColor borderColor = new MyColor(0x000000FF);
            a.startSlowMoveAnimation(borderColor);
            _currentSlowMove = a;
        }
        _animations.put(a.getId(), a);
    }

    public void update(double deltaTime){
        for (Map.Entry<String, Animations> elem : _animations.entrySet()) {
            if(!elem.getValue().update(deltaTime)){
                _queue.add(elem.getKey());
                //System.out.println("Animation in element " + elem.getValue().getId() + " end.");
            }
        }
        while(!_queue.isEmpty()){
            String e = _queue.poll();
            if(e!= null)
                _animations.remove(e);
        }
    }

    public void render(Graphics g){
        for (Map.Entry<String, Animations> elem : _animations.entrySet()) {
            elem.getValue().render(g);
        }
    }

    public void clickOutOfBoard(){
        clearSlowMove();
    }

    private void clearSlowMove(){
        if(_currentSlowMove != null){
            _currentSlowMove.getTarget().getButton().setHasAnimation(false);
            _animations.remove(_currentSlowMove.getId());
            _currentSlowMove = null;
        }
    }

    private Animations _currentSlowMove;
    private Queue<String> _queue;
    private Map<String, Animations> _animations;
    private Tablero _board;
}
