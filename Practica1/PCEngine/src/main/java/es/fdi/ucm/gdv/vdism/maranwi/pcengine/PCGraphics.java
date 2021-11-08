package es.fdi.ucm.gdv.vdism.maranwi.pcengine;

import java.awt.Color;
import java.awt.Dimension;
import java.awt.Font;
import java.awt.Graphics2D;
import java.awt.Image;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.HashMap;

import javax.swing.JFrame;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;


public class PCGraphics implements es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics {
    public PCGraphics(String windowName, int logicWidth, int logicHeight) {
        _logicWidth = logicWidth;
        _logicHeight = logicHeight;
        //averiguamos tamaño pantalla
        init(windowName);
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

    private void init(String windowName) {
        _frame = new JFrame(windowName);
        _frame.setSize(_logicWidth, _logicHeight);

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


    public void newImage(String name, String tag) {
        java.awt.Image searched = _images.get(tag);
        //We only create the image if we don't have it yet
        if (searched == null) {
            java.awt.Image img = null;
            try {
                img = javax.imageio.ImageIO.read(new java.io.File(name));
            } catch (IOException e) {
                System.err.println("Error cargando la imagen: " + e);

            }
            if (img != null)
                _images.put(tag, img);

        }
    }


    public void newFont(String filename, String tag, int size, boolean isBold) {

        java.awt.Font searched = _fonts.get(tag);
        int style = isBold ? java.awt.Font.BOLD : java.awt.Font.PLAIN;
        //We create the font only if we don't have it yet
        if (searched == null || searched.getSize() != size || searched.getStyle() != style) {
            java.awt.Font f = null;

            try (InputStream is = new FileInputStream(filename)) {
                f = java.awt.Font.createFont(java.awt.Font.TRUETYPE_FONT, is);
            } catch (Exception e) {
                System.err.println("Error cargando la fuente: " + e);
            }
            f = f.deriveFont(style, size);
            _fonts.put(tag, f);
        }
    }


    public void clear(int color) {
        if (color != -1) {
            _myGraphics.setColor(new Color(color));
            _myGraphics.fillRect(0, 0, _logicWidth, _logicHeight);
        }

    }

    public void adjustToScreen(Application app) {
        Dimension size = _frame.getSize();
        //Hacemos la regla de tres para ver si cabría

        double newY = _logicHeight * size.width / _logicWidth;
        double newX = _logicWidth * size.height / _logicHeight;
        double newPosX = 0.0f, newPosY = 0.0f;
        //Si escalando la Y no cabríamos

        if (newY > size.height) {
            _scaleX = newX / _logicWidth;
            _scaleY = size.height / (double) _logicHeight;
            double centerX = size.width / 2;
            newPosX = centerX - (newX / 2);
            translate(newPosX, 0);
        } else if (newX > size.width) {
            _scaleX = size.width / (double) _logicWidth;
            _scaleY = newY / _logicHeight;
            double centerY = size.height / 2;
            newPosY = centerY - (newY / 2);
            translate(0, newPosY);
        }
        _width = _scaleX * _logicWidth;
        _height = _scaleY * _logicHeight;
        app.setApplicationZone(_width, _height, size.getWidth(), size.getHeight());
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

    ///si nos da widht o height -1 se considera que ese parametro es full
    @Override
    public void drawImage(String image, int x, int y, int width, int height) {
        Image img = _images.get(image);
        if (img != null) {
            if (width < 0)
                width = img.getWidth(_frame);
            if (height < 0)
                height = img.getHeight(_frame);
            _myGraphics.drawImage(img, x, y, width, height, _frame);
        }
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
        return _logicWidth;
    }

    @Override
    public int getHeight() {
        return _logicHeight;
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

    double _translationX = 0;
    double _translationY = 0;
    double _scaleX = 1;
    double _scaleY = 1;
    HashMap<String, Font> _fonts;
    HashMap<String, java.awt.Image> _images;
    private java.awt.Graphics _myGraphics;
    private int _logicWidth;
    private int _logicHeight;
    private double _width;
    private double _height;
    private JFrame _frame;
}
