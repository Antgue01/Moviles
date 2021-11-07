package es.fdi.ucm.gdv.vdism.maranwi.pcengine;

import java.awt.Color;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.Graphics2D;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;
import java.io.FileInputStream;
import java.io.InputStream;
import java.util.HashMap;

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
                if (_myGraphics != null) {
                    Graphics2D g = (Graphics2D) _myGraphics;
                    g.scale(_scaleX, _scaleY);
                    g.translate(_translationX, _translationY);
                }
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
        _fonts = new HashMap<String, Font>();

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

    public void adjustToScreen() {
        Dimension size = _frame.getSize();
        //Hacemos la regla de tres para ver si cabría

        double newY = _height * size.width / _width;
        double newX = _width * size.height / _height;
        //Si escalando la Y no cabríamos
        if (newY > size.height) {
            _scaleX = newX / _width;
            _scaleY = size.height / (double) _height;
            double centerX = size.width / 2;
            double newPosX = centerX - (newX/2);
            translate(newPosX-_posX , 0);


        } else if (newX > size.width) {
            _scaleX = size.width / (double) _width;
            _scaleY = newY / _height;
            double centerY = size.height / 2;
            double newPosY = centerY - (newY/2);
            translate(0, newPosY );
        }
    }

    public void translate(double x, double y) {
        _translationX = x;
        _translationY = y;
    }

    @Override
    public void scale(double x, double y) {
        _scaleX = x;
        _scaleY = y;
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
        java.awt.Font currentFont = _fonts.get(tag);
        if (currentFont != null)
            _myGraphics.setFont(currentFont);
    }


    public void addMouseListener(MouseListener ml) {
        _frame.addMouseListener(ml);
    }

    public void addMouseMotionListener(MouseMotionListener mml) {
        _frame.addMouseMotionListener(mml);
    }

    double _posX = 0;
    double _posY = 0;
    double _translationX = 0;
    double _translationY = 0;
    double _scaleX = 1;
    double _scaleY = 1;
    HashMap<String, Font> _fonts;
    private java.awt.Graphics _myGraphics;
    private int _width;
    private int _height;
    private JFrame _frame;
}
