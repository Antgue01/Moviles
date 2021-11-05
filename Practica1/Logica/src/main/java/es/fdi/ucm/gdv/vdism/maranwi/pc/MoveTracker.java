package es.fdi.ucm.gdv.vdism.maranwi.pc;

import java.util.Stack;

public class MoveTracker {
    public MoveTracker() {
        _moves = new Stack<Move>();
    }

    public void addMove(int X, int Y, TipoCelda type) {
        _moves.push(new Move(X, Y, type));
    }

    public void restoreMove(Tablero t) {
        if (!_moves.empty()) {
            Move last=_moves.pop();
            t.setColor(last.getX(), last.getY(),last.getType());
        }
    }

    private Stack<Move> _moves;

}
