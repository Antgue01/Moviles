package es.fdi.ucm.gdv.vdism.maranwi.practica1;
import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;

import es.fdi.ucm.gdv.vdism.maranwi.androidengine.AndroidEngine;
import es.fdi.ucm.gdv.vdism.maranwi.androidengine.AndroidGraphics;
import es.fdi.ucm.gdv.vdism.maranwi.logica.OhNo;

public class AndroidGame extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        OhNo myGame = new OhNo();
        _engine = new AndroidEngine(myGame, this, getAssets());

        AndroidGraphics g = (AndroidGraphics)_engine.getGraphics();
        setContentView(g.getView());
    }

    @Override
    protected void onResume() {
        super.onResume();
        _engine.resume();
    }

    @Override
    protected void onPause() {
        super.onPause();
        _engine.pause();
    }

    @Override
    protected void onDestroy(){
        super.onDestroy();
        _engine.destroy();
    }

    AndroidEngine _engine;
}