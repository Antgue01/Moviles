package es.fdi.ucm.gdv.vdism.maranwi.pcgame;

import es.fdi.ucm.gdv.vdism.maranwi.pc.OhNo;
import es.fdi.ucm.gdv.vdism.maranwi.pcengine.PCEngine;

public class PCGame {
    public static void main(String[] args) {
        PCEngine engine = new PCEngine();
        OhNo myGame = new OhNo();
        engine.SetApplication(myGame);
        engine.Play();
    }
}