package es.fdi.ucm.gdv.vdism.maranwi.pcengine;

import java.io.FileInputStream;
import java.io.InputStream;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Font;

public class PCFont implements Font {

    public PCFont(String path, int size, boolean isBold) {
        _size = size;
        _isBold = isBold;

        int style = isBold ? java.awt.Font.BOLD : java.awt.Font.PLAIN;

        try (InputStream is = new FileInputStream(path)) {
            _font = java.awt.Font.createFont(java.awt.Font.TRUETYPE_FONT, is);
        } catch (Exception e) {
            System.err.println("Error cargando la fuente: " + e);
        }
        _font =_font.deriveFont(style, size);
    }

    @Override
    public int getSize() {
        return _size;
    }

    @Override
    public boolean getIsBold() {
        return _isBold;
    }


    public java.awt.Font getJavaFont(){
        return _font;
    }

    int _size;
    boolean _isBold;
    private java.awt.Font _font;
}
