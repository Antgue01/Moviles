package es.fdi.ucm.gdv.vdism.maranwi.practica1;
import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;

import es.fdi.ucm.gdv.vdism.maranwi.androidengine.AndroidGraphics;
import es.fdi.ucm.gdv.vdism.maranwi.logica.OhNo;

public class AndroidGame extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        _ohno=new OhNo();
        _manager = new ExecutionThreadManager(_ohno, this,getAssets());
        AndroidGraphics g=(AndroidGraphics)_manager.getEngine().getGraphics();
        setContentView(g.getView());
    }

    @Override
    protected void onResume() {
        super.onResume();
        _manager.resume();

    }

    @Override
    protected void onPause() {
        super.onPause();
        _manager.pause();
    }

    OhNo _ohno;
    ExecutionThreadManager _manager;
}