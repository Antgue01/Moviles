package es.fdi.ucm.gdv.vdism.maranwi.pcgame;

import es.fdi.ucm.gdv.vdism.maranwi.pc.OhNo;
import es.fdi.ucm.gdv.vdism.maranwi.pcengine.PCEngine;

public class PCGame {
    public static void main(String[] args) {
        OhNo myGame = new OhNo();
        PCEngine engine = new PCEngine(myGame, "OhNo!");
        engine.Play();
    }
}