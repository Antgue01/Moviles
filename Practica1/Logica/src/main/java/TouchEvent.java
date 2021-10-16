public class TouchEvent {
    public TouchEvent(TouchType type,int posX,int posY, int id){
        _type=type;
        _posX=posX;
        _posY=posY;
        _id=id;
    }
    enum TouchType {pulsacion,liberacion,desplazamiento};
    private TouchType _type;
    private int _posX;
    private int _posY;
    private int _id;

}
