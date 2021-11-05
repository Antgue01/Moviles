package es.fdi.ucm.gdv.vdism.maranwi.pcengine;

import java.io.FileInputStream;
import java.io.InputStream;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Font;

public class PCFont implements Font {

    public PCFont(String fontName, boolean isBold, int Size) {
        int style = isBold ? java.awt.Font.BOLD : java.awt.Font.PLAIN;

        try (InputStream is = new FileInputStream(fontName)) {
            _font = java.awt.Font.createFont(java.awt.Font.TRUETYPE_FONT, is);
        } catch (Exception e) {
            System.err.println("Error cargando la fuente: " + e);
        }
        _font =_font.deriveFont(style, Size);

    }


    private java.awt.Font _font;
}
