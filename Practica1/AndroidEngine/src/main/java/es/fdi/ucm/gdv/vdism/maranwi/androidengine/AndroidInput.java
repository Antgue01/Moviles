package es.fdi.ucm.gdv.vdism.maranwi.androidengine;

import android.view.MotionEvent;
import android.view.View;

import java.util.ArrayList;
import java.util.List;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Input;

public class AndroidInput implements Input, View.OnTouchListener {

    public AndroidInput() {
        _events = new ArrayList<TouchEvent>();
    }

    @Override
    public List<TouchEvent> getTouchEvents() {
        List<TouchEvent> auxEvents;
        synchronized (this) {
            auxEvents = new ArrayList<>(_events);
            _events.clear();
        }
        return auxEvents;
    }

    @Override
    public boolean onTouch(View v, MotionEvent event) {
        ArrayList<TouchEvent> events = new ArrayList<TouchEvent>();

        TouchEvent.TouchType type = null;
        int eventMask = event.getActionMasked();
        if (eventMask == MotionEvent.ACTION_UP || eventMask == MotionEvent.ACTION_POINTER_UP)
            type = TouchEvent.TouchType.liberacion;
        else if (eventMask == MotionEvent.ACTION_DOWN || eventMask == MotionEvent.ACTION_POINTER_DOWN)
            type = TouchEvent.TouchType.pulsacion;
        else if (eventMask == MotionEvent.ACTION_MOVE)
            type = TouchEvent.TouchType.desplazamiento;
        if (type != null) {
            int id = event.getActionIndex();
            MotionEvent.PointerCoords coords = new MotionEvent.PointerCoords();
            event.getPointerCoords(id, coords);
            TouchEvent e = new TouchEvent(type, (int) coords.x, (int) coords.y, id);
            synchronized (this) {
                _events.add(e);
            }
            return true;

        } else return false;
    }

    private List<TouchEvent> _events;
}
