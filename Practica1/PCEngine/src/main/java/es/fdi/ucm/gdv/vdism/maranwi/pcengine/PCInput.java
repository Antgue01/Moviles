package es.fdi.ucm.gdv.vdism.maranwi.pcengine;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;
import java.util.ArrayList;
import java.util.List;


public class PCInput implements MouseListener, MouseMotionListener,es.fdi.ucm.gdv.vdism.maranwi.engine.Input {

    List<TouchEvent>getEvents(){
        List<TouchEvent> auxEvents=new ArrayList<>(_events);
        _events.clear();
        return  auxEvents;
    }
    private List<TouchEvent> _events;

    @Override
    public void mouseClicked(MouseEvent mouseEvent) {
        TouchEvent.TouchType type= TouchEvent.TouchType.pulsacion;
        TouchEvent myEvent=new TouchEvent(type,mouseEvent.getX(),mouseEvent.getY(),mouseEvent.getID());
        _events.add(myEvent);
    }

    @Override
    public void mousePressed(MouseEvent mouseEvent) {

    }

    @Override
    public void mouseReleased(MouseEvent mouseEvent) {
        TouchEvent.TouchType type= TouchEvent.TouchType.liberacion;
        TouchEvent myEvent=new TouchEvent(type,mouseEvent.getX(),mouseEvent.getY(),mouseEvent.getID());
        _events.add(myEvent);
    }

    @Override
    public void mouseEntered(MouseEvent mouseEvent) {

    }

    @Override
    public void mouseExited(MouseEvent mouseEvent) {

    }

    @Override
    public void mouseDragged(MouseEvent mouseEvent) {
        TouchEvent.TouchType type= TouchEvent.TouchType.desplazamiento;
        TouchEvent myEvent=new TouchEvent(type,mouseEvent.getX(),mouseEvent.getY(),mouseEvent.getID());
        _events.add(myEvent);
    }

    @Override
    public void mouseMoved(MouseEvent mouseEvent) {

    }

    @Override
    public List<TouchEvent> getTouchEvents() {
        return _events;
    }
}
