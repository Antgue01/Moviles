package es.fdi.ucm.gdv.vdism.maranwi.pcengine;
import java.awt.event.MouseEvent;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;
import java.util.ArrayList;
import java.util.List;


public class PCInput implements MouseListener, MouseMotionListener,es.fdi.ucm.gdv.vdism.maranwi.engine.Input {


    public PCInput(){
        _events=new ArrayList<TouchEvent>();
    }
    @Override
    public void mouseClicked(MouseEvent mouseEvent) { }

    @Override
    public void mousePressed(MouseEvent mouseEvent) {
        TouchEvent.TouchType type= TouchEvent.TouchType.pulsacion;
        TouchEvent myEvent=new TouchEvent(type,mouseEvent.getX(),mouseEvent.getY(),mouseEvent.getID());
        synchronized (this)
        {
            _events.add(myEvent);
        }
    }

    @Override
    public void mouseReleased(MouseEvent mouseEvent) {
        TouchEvent.TouchType type= TouchEvent.TouchType.liberacion;
        TouchEvent myEvent=new TouchEvent(type,mouseEvent.getX(),mouseEvent.getY(),mouseEvent.getID());
        synchronized (this)
        {
            _events.add(myEvent);
        }
    }

    @Override
    public void mouseEntered(MouseEvent mouseEvent) { }

    @Override
    public void mouseExited(MouseEvent mouseEvent) { }

    @Override
    public void mouseDragged(MouseEvent mouseEvent) {
        TouchEvent.TouchType type= TouchEvent.TouchType.desplazamiento;
        TouchEvent myEvent=new TouchEvent(type,mouseEvent.getX(),mouseEvent.getY(),mouseEvent.getID());
        synchronized (this)
        {
            _events.add(myEvent);
        }
    }

    @Override
    public void mouseMoved(MouseEvent mouseEvent) { }

    @Override
    public List<TouchEvent> getTouchEvents() {
        List<TouchEvent> auxEvents;
        synchronized (this)
        {
            auxEvents=new ArrayList<>(_events);
            _events.clear();
        }
        return  auxEvents;
    }
    private List<TouchEvent> _events;

}
