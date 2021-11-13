package es.fdi.ucm.gdv.vdism.maranwi.logica;

public class Hint {

    public Hint(HintsManager.HintType type, int x, int y){
        _type = type;
        _row = x;
        _col = y;

        switch (type){
            case ONE:
                _hintMessage = "This number can see all its dots";
                break;
            case TWO:
                _hintMessage = "Looking further in one direction\n" +
                        "would exceed this number";
                break;
            case THREE:
                _hintMessage = "One specific dot is included\n" +
                        "in all solutions imaginable";
                break;
            case FOUR:
                _hintMessage = "This number sees a bit too much";
                break;
            case FIVE:
                _hintMessage = "This number can't see enough";
                break;
            case SIX:
                _hintMessage = "This one should be easy...";
                break;
            case SEVEN:
                _hintMessage = "This one should be easy...";
                break;
            default:
                _hintMessage = "";
                break;

        }
    }


    public String getHintMessage() { return _hintMessage;}
    public HintsManager.HintType getHintType() { return _type;}

    public int[] getPos(){
        int[] pos = {_row, _col};
        return pos;
    }

    public int[] getWhereToApply(){
        int[] pos = {_rowToApply, _colToApply};
        return pos;
    }

    public void setWhereToApply(int x, int y){
        _rowToApply = x;
        _colToApply = y;
    }

    /** Descripcion de la pista */
    private String _hintMessage;
    /** Posiciones en el tablero */
    private int _row, _col;
    /** Posiciones donde se deberia colocar una ficha si fuese el caso */
    private int _rowToApply, _colToApply;

    HintsManager.HintType _type;

}
