package es.fdi.ucm.gdv.vdism.maranwi.pcengine;

import java.awt.Color;

import javax.swing.JFrame;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Font;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Image;


public class PCGraphics implements es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics
{

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
        _myGraphics.fillOval(cx,cy,r,r);
    }

    @Override
    public void drawText(String text, int x, int y) {

    }

    public int getWidth(){return  _width;}

    @Override
    public int getHeight() {
        return _height;
    }

    public  void init(){
        _frame=new JFrame();
        _width=800;
        _height=600;
        _frame.setSize(_width,_height);

    }
    private java.awt.Graphics _myGraphics;
    private int _width;
    private int _height;
    private JFrame _frame;


}
