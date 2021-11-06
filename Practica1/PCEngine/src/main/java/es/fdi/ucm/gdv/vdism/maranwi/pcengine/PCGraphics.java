package es.fdi.ucm.gdv.vdism.maranwi.pcengine;

import java.awt.Color;
import java.awt.Font;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;
import java.io.FileInputStream;
import java.io.InputStream;
import java.util.Dictionary;

import javax.swing.JFrame;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Image;


public class PCGraphics implements es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics {
    public PCGraphics() {
        //averiguamos tamaño pantalla
        init();

    }

    public void draw(Application app) {

        do {
            do {
                _myGraphics = _frame.getBufferStrategy().getDrawGraphics();
                try {
                    app.onRender(this);
                } finally {
                    _myGraphics.dispose();
                }
            } while (_frame.getBufferStrategy().contentsRestored()); //True si se ha limpiado con un color de fondo y está preparado

            _frame.getBufferStrategy().show();
        } while (_frame.getBufferStrategy().contentsLost()); //Devuelve si se ha perdido el buffer de pintado
    }

    private void init() {
        _frame = new JFrame("OhNo!");
        _width = 400;
        _height = 600;
        _frame.setSize(_width, _height);
        _frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        _frame.setIgnoreRepaint(true);
        _frame.setVisible(true);
//        _fonts=new Dictionary<String, java.awt.Font>();

        int intentos = 100;
        while (intentos-- > 0) {
            try {
                _frame.createBufferStrategy(2);
                break;
            } catch (Exception e) {
            }
        } // while pidiendo la creación de la buffeStrategy
        if (intentos == 0) {
            System.err.println("No pude crear la BufferStrategy");
            return;
        }
    }


    public Image newImage(String name) {
        return null;
    }


    public void newFont(String filename, String tag, int size, boolean isBold) {

        java.awt.Font f = null;
        int style = isBold ? java.awt.Font.BOLD : java.awt.Font.PLAIN;

        try (InputStream is = new FileInputStream(filename)) {
            f = java.awt.Font.createFont(java.awt.Font.TRUETYPE_FONT, is);
        } catch (Exception e) {
            System.err.println("Error cargando la fuente: " + e);
        }
        f = f.deriveFont(style, size);
        _fonts.put(tag, f);
    }


    public void clear(int color) {
        if (color != -1) {
            _myGraphics.setColor(new Color(color));
            _myGraphics.fillRect(0, 0, _width, _height);
        }

    }


    public void translate(int x, int y) {

    }

    @Override
    public void scale(int x, int y) {

    }

    @Override
    public void save() {

    }

    @Override
    public void restore() {

    }

    @Override
    public void drawImage(Image image, int x, int y, int width, int height) {
    }

    public void setColor(int color) {
        if (color != -1)
            _myGraphics.setColor(new Color(color));
    }

    public void fillCircle(int cx, int cy, int r) {
        if (_myGraphics != null)
            _myGraphics.fillOval(cx, cy, r, r);
        else System.out.println("No hay graphics xhaval");
    }

    @Override
    public void drawText(String text, int x, int y) {
        _myGraphics.drawString(text, x, y);
    }

    public int getWidth() {
        return _width;
    }

    @Override
    public int getHeight() {
        return _height;
    }

    @Override
    public void setFont(String tag) {
        java.awt.Font currentFont = null;
        try {
            currentFont = _fonts.get(tag);
        } catch (Exception e) {
            System.err.println("No existe esa fuente");
        }
        if (currentFont != null)
            _myGraphics.setFont(currentFont);
    }


    public void addMouseListener(MouseListener ml) {
        _frame.addMouseListener(ml);
    }

    public void addMouseMotionListener(MouseMotionListener mml) {
        _frame.addMouseMotionListener(mml);
    }

    Dictionary<String, java.awt.Font> _fonts;
    private java.awt.Graphics _myGraphics;
    private int _width;
    private int _height;
    private JFrame _frame;
}
