package es.fdi.ucm.gdv.vdism.maranwi.pcengine;

import java.awt.Color;
import java.awt.Graphics;

import javax.swing.JFrame;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Font;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Image;


public class PCGraphics implements es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics
{
    public PCGraphics(){
        init();
    }

    public boolean prepararBuffer(){
         //do {
          /*  do {
                _myGraphics = _frame.getBufferStrategy().getDrawGraphics();
                try {
                    //ventana.render(graphics);
                }
                finally {
                    _myGraphics.dispose();
                }
            } while(_frame.getBufferStrategy().contentsRestored()); //True si se ha limpiado con un color de fondo y está preparado
            */
         //   strategy.show();
        //} while(strategy.contentsLost()); //Devuelve si se ha perdido el buffer de pintado
        return false;
    }

    private void init(){
        _frame=new JFrame();
        _width=800;
        _height=600;
        _frame.setSize(_width,_height);
        _myGraphics = _frame.getBufferStrategy().getDrawGraphics();
    }

    public Image newImage(String name) {
        return null;
    }


    public Font newFont(String filename, int size, boolean isBold) {
        return null;
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

    }

    public int getWidth(){return  _width;}

    @Override
    public int getHeight() {
        return _height;
    }


    private java.awt.Graphics _myGraphics;
    private int _width;
    private int _height;
    private JFrame _frame;
}
