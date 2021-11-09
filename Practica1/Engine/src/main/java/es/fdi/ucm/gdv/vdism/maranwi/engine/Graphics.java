package es.fdi.ucm.gdv.vdism.maranwi.engine;

public interface Graphics {
    //public Image newImage(String name,String tag);
    public void newImage(String name,String tag);
    //public Font newFont(String filename, int size, boolean isBold);
    public void newFont(String filename, int size, boolean isBold);
    public void clear(int color);
    public void translate(double x,double y);
    public void scale(double x,double y);
    public void save();
    public void restore();
    public void drawImage(String tag, int x,int y, int width, int height);
    public void setColor(int color);
    public void fillCircle(int cx,int cy,int r);
    public void drawText(String text,int x,int y);
    public int getWindowsWidth();
    public int getWindowsHeight();
    public int getCanvasWidth();
    public int getCanvasHeight();
    public  void setFont(String tag);

}
