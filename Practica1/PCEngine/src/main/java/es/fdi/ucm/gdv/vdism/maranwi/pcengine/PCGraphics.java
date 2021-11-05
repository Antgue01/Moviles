package es.fdi.ucm.gdv.vdism.maranwi.pcengine;

import java.awt.Color;
import java.awt.Graphics;
import java.awt.event.MouseListener;
import java.awt.event.MouseMotionListener;

import javax.swing.JFrame;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Application;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Font;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Image;


public class PCGraphics implements es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics
{
    public PCGraphics(){
        //averiguamos tamaño pantalla
        init();

    }

    public void draw(Application app){

        do {
            do {
                _myGraphics = _frame.getBufferStrategy().getDrawGraphics();
                try {
                    app.onRender(this);
                }
                finally {
                    _myGraphics.dispose();
                }
            } while(_frame.getBufferStrategy().contentsRestored()); //True si se ha limpiado con un color de fondo y está preparado

            _frame.getBufferStrategy().show();
        } while(_frame.getBufferStrategy().contentsLost()); //Devuelve si se ha perdido el buffer de pintado
    }

    private void init(){
        _frame=new JFrame("OhNo!");
        _width=400;
        _height=600;
        _frame.setSize(_width,_height);
        _frame.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
        _frame.setIgnoreRepaint(true);
        _frame.setVisible(true);
        int intentos = 100;
        while(intentos-- > 0) {
            try {
                _frame.createBufferStrategy(2);
                break;
            }
            catch(Exception e) {
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


    public Font newFont(String filename, int size, boolean isBold) {

        return  new PCFont(filename,isBold,size);
    }


    public void clear(int color){
        if(color!=-1)
        {
            _myGraphics.setColor(new Color(color));
            _myGraphics.fillRect(0,0,_width,_height);
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

    public void setColor(int color){
        if(color!=-1)
            _myGraphics.setColor(new Color(color));
    }
    public void fillCircle(int cx, int cy,int r){
        if(_myGraphics != null)
            _myGraphics.fillOval(cx,cy,r,r);
        else System.out.println("No hay graphics xhaval");
    }

    @Override
    public void drawText(String text, int x, int y) {
        _myGraphics.drawString(text,x,y);
    }

    public int getWidth(){return  _width;}

    @Override
    public int getHeight() {
        return _height;
    }

    public void addMouseListener(MouseListener ml){
        _frame.addMouseListener(ml);
    }
    public void addMouseMotionListener(MouseMotionListener mml) {
        _frame.addMouseMotionListener(mml);
    }

    private java.awt.Graphics _myGraphics;
    private int _width;
    private int _height;
    private JFrame _frame;
}
