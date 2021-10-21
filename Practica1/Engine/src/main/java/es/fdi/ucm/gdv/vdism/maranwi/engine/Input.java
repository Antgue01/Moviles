package es.fdi.ucm.gdv.vdism.maranwi.engine;


import java.util.List;

public interface Input {

    class TouchEvent {
        public TouchEvent(TouchType type,int posX,int posY, int id){
            _type=type;
            _posX=posX;
            _posY=posY;
            _id=id;
        }
        public enum TouchType {pulsacion,liberacion,desplazamiento};
        private TouchType _type;
        private int _posX;
        private int _posY;
        private int _id;

    }
    List<TouchEvent> getTouchEvents();
}
