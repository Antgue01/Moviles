package es.fdi.ucm.gdv.vdism.maranwi.pc;

import java.awt.Graphics;
import java.awt.Color;

public class PCGraphics
        //implements Graphics
{
 //Image newImage(String name)
  void clear(int color){
      if(color!=-1)
      {
          _myGraphics.setColor(new Color(color));
          _myGraphics.fillRect(0,0,_width,_height);
      }

  }
  void setColor(int color){
    if(color!=-1)
        _myGraphics.setColor(new Color(color));
  }
  void fillCircle(int cx, int cy,int r){
    _myGraphics.fillOval(cx,cy,r,r);
  }
  //void translate(int x,int y){}
  //void scale(float x,float y){}
  //void save(){}
  //void restore() {}
  //void drawImage(Image image, int x,int y, int w, int h){}
  //void void drawText(Font text, int x, int y){]
  //Image newImage(String name){}
  //Font newFont(String filename, int w, int h,boolean isBold){}
  int getWidth(){return  _width;}
  int getHeigth(){return  _height;}
  private Graphics _myGraphics;
  private int _width;
  private int _height;
}
