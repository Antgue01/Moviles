package es.fdi.ucm.gdv.vdism.maranwi.engine;

public interface Graphics {
    Image newImage(String name);
    Font newFont(String filename, int size, boolean isBold);
    void clear(int rgba);
    void clear(int r, int g, int b, int a);
    void clear(Color color);
    void translate(double x,double y);
    void scale(double x,double y);
    void drawImage(Image img, int x,int y, int width, int height);
    void drawImage(Image img, int x,int y, int width, int height, int alpha);
    void setColor(int rgba);
    void setColor(int r,int g, int b,int a);
    void setColor(Color color);
    void fillCircle(int cx,int cy,int r);
    void drawText(String text,int x,int y);
    int getWindowsWidth();
    int getWindowsHeight();
    int getCanvasWidth();
    int getCanvasHeight();
    void setFont(Font font);

}
