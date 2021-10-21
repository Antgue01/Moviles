package es.fdi.ucm.gdv.vdism.maranwi.engine;

public interface Graphics {
    public Image newImage(String name);
    public Font newFont(String filename, int size, boolean isBold);
    public void clear(int color);
    public void translate(int x,int y);
    public void scale(int x,int y);
    public void save();
    public void restore();
    public void drawImage(Image image, int x,int y, int width, int height);
    public void setColor(int color);
    public void fillCircle(int cx,int cy,int r);
    public void drawText(String text,int x,int y);
    public int getWidth();
    public int getHeight();
}
