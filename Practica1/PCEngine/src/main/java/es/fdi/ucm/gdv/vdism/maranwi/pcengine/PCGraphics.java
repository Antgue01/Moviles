package es.fdi.ucm.gdv.vdism.maranwi.pcengine;

import java.awt.AlphaComposite;
import java.awt.Dimension;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Font;

import java.awt.Graphics2D;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Image;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Color;

import java.awt.RenderingHints;
import java.awt.event.ComponentAdapter;
import java.awt.event.ComponentEvent;

import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;

import java.util.HashMap;

import javax.swing.JFrame;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;


public class PCGraphics implements es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics {

    public PCGraphics(String windowName, int logicWidth, int logicHeight) {
        _logicWidth = logicWidth;
        _logicHeight = logicHeight;

        _frame = new JFrame(windowName);
        _fonts = new HashMap<String, PCFont>();

        init();
    }

    private void init() {

        _frame.setSize(_logicWidth, _logicHeight);

        _frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        _frame.setIgnoreRepaint(true);
        _frame.setVisible(true);

        /** Evento de reescalado */
        _frame.addComponentListener(new ComponentAdapter() {
            @Override
            public void componentResized(ComponentEvent e) {
                super.componentResized(e);
                _resized = true;
            }
        });
        /** Evento de cierre de ventana */
        _frame.addWindowListener(new WindowAdapter() {
            @Override
            public void windowClosing(WindowEvent e) {
                super.windowClosing(e);
                System.out.println("CLOSING WINDOW");
                _windowClosed = true;
            }
        });

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

        if (getResized())
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
                        g.setRenderingHint(RenderingHints.KEY_ANTIALIASING, RenderingHints.VALUE_ANTIALIAS_ON);
                        g.setRenderingHint(RenderingHints.KEY_INTERPOLATION, RenderingHints.VALUE_INTERPOLATION_BILINEAR);
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
            sprite = javax.imageio.ImageIO.read(new java.io.File("images/" + name));
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
            font = new PCFont("fonts/" + filename, size, isBold);
            _fonts.put(id,font);
        }
        return font;
    }

    @Override
    public void clear(int color) {
        setColor(color);
        _myGraphics.fillRect(0, 0, _logicWidth, _logicHeight);

    }

    @Override
    public void clear(int r, int g, int b, int a) {
        setColor(r,g,b,a);
        _myGraphics.fillRect(0, 0, _logicWidth, _logicHeight);
    }

    @Override
    public void clear(Color color) {
        setColor(color);
        _myGraphics.fillRect(0, 0, _logicWidth, _logicHeight);
    }

    @Override
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
        if (img != null){
            _myGraphics.drawImage(((PCImage) (img)).getAwtImage(), x, y, width, height, _frame);
        }
    }

    @Override
    public void drawImage(Image img, int x, int y, int width, int height, int alpha) {
        if (img != null){
            //Apply alpha
            float a = (float)alpha / 255f;
            AlphaComposite ac = AlphaComposite.getInstance(AlphaComposite.SRC_OVER, a);
            Graphics2D g = (Graphics2D) _myGraphics;
            g.setComposite(ac);

            _myGraphics.drawImage(((PCImage) (img)).getAwtImage(), x, y, width, height, _frame);

            //Reset alpha
            ac = AlphaComposite.getInstance(AlphaComposite.SRC_OVER, 1.0f);
            g.setComposite(ac);
        }
    }

    @Override
    public void setColor(int rgba) {
        _myGraphics.setColor(new java.awt.Color(rgba));
    }

    @Override
    public void setColor(int r, int g, int b, int a) {
        if (r > -1 && r < 256 && g > -1 && g < 256 && b > -1 && b < 256 && a > -1 && a < 256)
            _myGraphics.setColor(new java.awt.Color(r, g, b, a));
    }

    @Override
    public void setColor(Color color) {
        int r = color.getRed();
        int b = color.getBlue();
        int g = color.getGreen();
        int a = color.getAlpha();
        if (r > -1 && r < 256 && g > -1 && g < 256 && b > -1 && b < 256 && a > -1 && a < 256) {
            _myGraphics.setColor(new java.awt.Color(r, g, b, a));
        }
    }

    @Override
    public void fillOval(int cx, int cy, int rx, int ry) {
        _myGraphics.fillOval(cx, cy, rx, ry);
    }

    @Override
    public void drawOval(int cx, int cy, int rx, int ry) {
        _myGraphics.drawOval(cx, cy, rx, ry);
    }

    @Override
    public void drawText(String text, int x, int y) {
        String[] lines = text.split("\n");
        int verticalOffset = _myGraphics.getFontMetrics().getHeight();

        for (int i = 0; i<lines.length; ++i){
            _myGraphics.drawString(lines[i],x,y + (verticalOffset * i));
        }
    }

    @Override
    public int getWindowWidth() {
        return _frame.getWidth();
    }

    @Override
    public int getWindowHeight() {
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
        java.awt.Font f = ((PCFont) font).getJavaFont();
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
            _myGraphics.setColor(new java.awt.Color(color));
            _myGraphics.fillRect(0, 0, _frame.getWidth(), _frame.getHeight());
        }
    }

    private void adjustToScreen() {
        Dimension size = _frame.getSize();

        //Hacemos la regla de tres para ver si cabría
        double newW = _logicWidth * size.height / _logicHeight;
        double newH = _logicHeight * size.width / _logicWidth;

        if (newH >= size.height) {
            //Factor de escala
            _scaleY = (double)size.height / (double)_logicHeight;
            _scaleX = _scaleY;
        }
        else if (newW >= size.width) {
            //Factor de escala
            _scaleX = (double)size.width / (double)_logicWidth;
            _scaleY = _scaleX;
        }

        _canvasWidth = _scaleX * (double)_logicWidth;
        _canvasHeight = _scaleY * (double)_logicHeight;

        _translationX = ((double)size.width - _canvasWidth)/2;
        _translationY = ((double)size.height - _canvasHeight)/2;
    }

    private boolean getResized() {
        if (_resized) {
            _resized = false;
            return true;
        }
        return false;
    }

    public boolean getClosed(){
        return _windowClosed;
    }

    double _translationX = 0;
    double _translationY = 0;
    double _scaleX = 1;
    double _scaleY = 1;
    HashMap<String, PCFont> _fonts;
    private java.awt.Graphics _myGraphics;
    private int _logicWidth;
    private int _logicHeight;
    private double _canvasWidth;
    private double _canvasHeight;
    private JFrame _frame;
    private java.awt.image.BufferStrategy _strategy;
    private boolean _resized = false;
    private boolean _windowClosed = false;
}
