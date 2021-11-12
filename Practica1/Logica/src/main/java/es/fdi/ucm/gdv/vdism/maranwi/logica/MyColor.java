package es.fdi.ucm.gdv.vdism.maranwi.logica;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Color;

public class MyColor implements Color {

    public MyColor(int r, int g, int b, int alpha){
        _red = r;
        _green = g;
        _blue = b;
        _alpha = alpha;
        RGBA_FromInts();
    }
    public MyColor(int rgba){
        _rgba=rgba;
        intsFrom_RGBA();
    }

    @Override
    public int getRGBA() {
        return _rgba;
    }

    @Override
    public int getRed() {
        return _red;
    }

    @Override
    public int getGreen() {
        return _green;
    }

    @Override
    public int getBlue() {
        return _blue;
    }

    @Override
    public int getAlpha() {
        return _alpha;
    }

    public void setRGBA(int rgba){ _rgba = rgba; intsFrom_RGBA(); };
    public void setRed(int red){ _red = red; RGBA_FromInts();}
    public void setGreen(int green){ _green = green; RGBA_FromInts();};
    public void setBlue(int blue){ _blue = blue; RGBA_FromInts();};
    public void setAlpha(int alpha){ _alpha = alpha; RGBA_FromInts();};

    private void RGBA_FromInts(){
        int Red = (_red << 24) & 0xFF000000; //Shift red 16-bits and mask out other stuff
        int Green = (_green << 16) & 0x00FF0000; //Shift Green 8-bits and mask out other stuff
        int Blue = (_blue  << 8)& 0x0000FF00; //Mask out anything not blue.
        int Alpha = _alpha & 0x000000FF;

        _rgba =  0x00000000 | Red | Green | Blue | Alpha; //0xFF000000 for 100% Alpha. Bitwise OR everything together.
    }

    private void intsFrom_RGBA(){
        _red=(_rgba & 0xFF000000) >> 24; //Shift red 16-bits and mask out other stuff
        _green= (_rgba & 0x00FF0000) >> 16; //Shift Green 8-bits and mask out other stuff
        _blue= (_rgba & 0x0000FF00) >> 8; //Mask out anything not blue.
        _alpha = _rgba & 0x000000FF;
    }


    private int _rgba;
    private int _red;
    private int _green;
    private int _blue;
    private int _alpha;
}
