package es.fdi.ucm.gdv.vdism.maranwi.pcengine;

import java.awt.Color;
import java.awt.Dimension;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Font;

import java.awt.Graphics2D;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Image;

import java.awt.event.ComponentEvent;
import java.awt.event.ComponentListener;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.util.HashMap;

import javax.swing.JFrame;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;


public class PCGraphics implements es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics {

    private class MyJFrame extends JFrame implements ComponentListener {

        public MyJFrame(String title) {
            super(title);
            getContentPane().addComponentListener(this);
        }

        public boolean getResized() {
            if (resized) {
                resized = false;
                return true;
            }
            return false;
        }

        @Override
        public void componentResized(ComponentEvent componentEvent) {
            resized = true;
        }

        @Override
        public void componentMoved(ComponentEvent componentEvent) {
        }

        @Override
        public void componentShown(ComponentEvent componentEvent) {
        }

        @Override
        public void componentHidden(ComponentEvent componentEvent) {
        }

        private boolean resized = false;
    }

    public PCGraphics(String windowName, int logicWidth, int logicHeight) {
        _logicWidth = logicWidth;
        _logicHeight = logicHeight;
        //averiguamos tamaño pantalla
        init(windowName);
    }

    private void init(String windowName) {
        _frame = new MyJFrame(windowName);
        _frame.setSize(_logicWidth, _logicHeight);

        _frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        _frame.setIgnoreRepaint(true);
        _frame.setVisible(true);
        _fonts = new HashMap<String, PCFont>();

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
        //Obtenemos el Buffer Strategy que se supone que acaba de crearse
        _strategy = _frame.getBufferStrategy();
    }

    public void draw(Application app) {

        if (_frame.getResized())
            adjustToScreen();

        do {
            do {
                try {
                    _myGraphics = _strategy.getDrawGraphics();
                } catch (Exception e) {
                    System.err.println("This frame won't render due to resizing");
                    return;
                }
                if (_myGraphics != null) {
                    Graphics2D g = (Graphics2D) _myGraphics;
                    clearAll(app.getBackgroundColor());
                    if (g != null) {
                        translate(_translationX, _translationY);
                        scale(_scaleX, _scaleY);
                    }
                }
                try {
                    app.onRender(this);
                } finally {
                    _myGraphics.dispose();
                }
            } while (_strategy.contentsRestored()); //True si se ha limpiado con un color de fondo y está preparado
            _strategy.show();
        } while (_strategy.contentsLost()); //Devuelve si se ha perdido el buffer de pintado
    }

    @Override
    public Image newImage(String name) {
        java.awt.Image sprite = null;
        try {
            sprite = javax.imageio.ImageIO.read(new java.io.File(name));
        } catch (Exception e) {
            e.printStackTrace();
        }
        return sprite != null ? new PCImage(sprite.getWidth(_frame), sprite.getHeight(_frame), sprite) : null;
    }

    @Override
    public Font newFont(String filename, int size, boolean isBold) {
        String id = filename + size + isBold + "";
        PCFont font = _fonts.get(id);
        if (font == null) {
            font = new PCFont(filename, size, isBold);
        }
        return font;
    }

    @Override
    public void clear(int color) {
        if (color != -1) {
            _myGraphics.setColor(new Color(color));
            _myGraphics.fillRect(0, 0, _logicWidth, _logicHeight);
        }

    }


    public void translate(double x, double y) {
        Graphics2D g2d = (Graphics2D) _myGraphics;
        if (g2d != null)
            g2d.translate(x, y);
    }

    @Override
    public void scale(double x, double y) {
        Graphics2D g2d = (Graphics2D) _myGraphics;
        if (g2d != null)
            g2d.scale(x, y);
    }


    ///si nos da widht o height -1 se considera que ese parametro es full
    @Override
    public void drawImage(Image img, int x, int y, int width, int height) {
        if (img != null)
            _myGraphics.drawImage(((PCImage) (img)).getAwtImage(), x, y, width, height, _frame);
    }

    @Override
    public void setColor(int color) {
        if (color != -1)
            _myGraphics.setColor(new Color(color));
    }

    @Override
    public void setColor(int r, int g, int b, int a) {
        if (r > -1 && r < 256 && g > -1 && g < 256 && b > -1 && b < 256 && a > -1 && a < 256)
            _myGraphics.setColor(new Color(r, g, b, a));
    }

    @Override
    public void setColor(es.fdi.ucm.gdv.vdism.maranwi.engine.Color color) {
        int r = color.getRed();
        int b = color.getBlue();
        int g = color.getGreen();
        int a = color.getAlpha();
        if (r > -1 && r < 256 && g > -1 && g < 256 && b > -1 && b < 256 && a > -1 && a < 256) {
            _myGraphics.setColor(new Color(r, g, b, a));
        }
    }

    @Override
    public void fillCircle(int cx, int cy, int r) {
        if (_myGraphics != null)
            _myGraphics.fillOval(cx, cy, r, r);
        else System.out.println("No hay graphics xhaval");
    }

    @Override
    public void drawText(String text, int x, int y) {
        _myGraphics.drawString(text, x, y);
    }

    @Override
    public int getWindowsWidth() {
        return _frame.getWidth();
    }

    @Override
    public int getWindowsHeight() {
        return _frame.getHeight();
    }

    @Override
    public int getCanvasWidth() {
        return (int) _canvasWidth;
    }

    @Override
    public int getCanvasHeight() {
        return (int) _canvasHeight;
    }

    @Override
    public void setFont(Font font) {
        java.awt.Font f = ((PCFont) (font)).getJavaFont();
        if (f != null) _myGraphics.setFont(f);
    }


    public void addMouseListener(MouseListener ml) {
        _frame.addMouseListener(ml);
    }

    public void addMouseMotionListener(MouseMotionListener mml) {
        _frame.addMouseMotionListener(mml);
    }

    private void clearAll(int color) {
        if (color != -1) {
            _myGraphics.setColor(new Color(color));
            _myGraphics.fillRect(0, 0, _frame.getWidth(), _frame.getHeight());
        }
    }

    private void adjustToScreen() {
        Dimension size = _frame.getSize();
        //Hacemos la regla de tres para ver si cabría

        double newY = _logicHeight * size.width / _logicWidth;
        double newX = _logicWidth * size.height / _logicHeight;
        double newPosX = 0.0f, newPosY = 0.0f;
        //Si escalando la Y no cabríamos

        if (newY > size.height) {
            _scaleX = newX / _logicWidth;
            _scaleY = _scaleX;
            double centerX = size.width / 2;
            newPosX = centerX - (newX / 2);
            _translationX = newPosX;
            _translationY = 0;
        } else if (newX > size.width) {
            _scaleY = newY / _logicHeight;
            _scaleX = _scaleY;
            double centerY = size.height / 2;
            newPosY = centerY - (newY / 2);
            _translationX = 0;
            _translationY = newPosY;
        }
        _canvasWidth = _scaleX * _logicWidth;
        _canvasHeight = _scaleY * _logicHeight;
        //app.setApplicationZone(_width, _height, size.getWidth(), size.getHeight());

    }

    double _translationX = 0;
    double _translationY = 0;
    double _scaleX = 1;
    double _scaleY = 1;
    HashMap<String, PCFont> _fonts;
    HashMap<String, java.awt.Image> _images;
    private java.awt.Graphics _myGraphics;
    private int _logicWidth;
    private int _logicHeight;
    private double _canvasWidth;
    private double _canvasHeight;
    private MyJFrame _frame;
    private java.awt.image.BufferStrategy _strategy;
}
