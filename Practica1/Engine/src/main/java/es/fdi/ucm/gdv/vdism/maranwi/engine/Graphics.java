package es.fdi.ucm.gdv.vdism.maranwi.engine;

public interface Graphics {
    /**
     * Crea una nueva imagen
     * @param filename Nombre de archivo
     * @return Objeto Image creado
     */
    Image newImage(String filename);

    /**
     * Crea una nueva fuente
     * @param filename Nombre de archivo
     * @param size Tamanio de fuente
     * @param isBold En negrita
     * @return
     */
    Font newFont(String filename, int size, boolean isBold);

    /**
     * Limpia el fondo con un color
     * @param rgba Formato hexadecimal
     */
    void clear(int rgba);

    /**
     * Limpia el fondo con un color
     * @param r Red {0,255}
     * @param g Green {0,255}
     * @param b Blue {0,255}
     * @param a Alpha {0,255}
     */
    void clear(int r, int g, int b, int a);

    /**
     * Limpia el fondo con un color
     * @param color
     */
    void clear(Color color);
    void translate(double x,double y);
    void scale(double x,double y);
    void drawImage(Image img, int x,int y, int width, int height);
    void drawImage(Image img, int x,int y, int width, int height, int alpha);

    /**
     * Establece el color
     * @param rgba Formato hexadecimal
     */
    void setColor(int rgba);

    /**
     * Establece el color
     * @param r Red {0,255}
     * @param g Green {0,255}
     * @param b Blue {0,255}
     * @param a Alpha {0,255}
     */
    void setColor(int r,int g, int b,int a);

    /**
     * Establece el color
     * @param color
     */
    void setColor(Color color);

    /**
     * Pinta ovalo relleno
     * @param cx posicion respecto a la esquina superior izquierda del ovalo
     * @param cy posicion respecto a la esquina superior izquierda del ovalo
     * @param rx radio en x
     * @param ry radio en y
     */
    void fillOval(int cx, int cy, int rx, int ry);

    /**
     * Pinta ovalo
     * @param cx posicion respecto a la esquina superior izquierda del ovalo
     * @param cy posicion respecto a la esquina superior izquierda del ovalo
     * @param rx radio en x
     * @param ry radio en y
     * @param strokeWidth anchura de dibujado
     */
    void drawOval(int cx, int cy, int rx, int ry, float strokeWidth);

    /**
     * Pinta un texto con la fuente establecida
     * @param text
     * @param x posicion en x
     * @param y posicion en y
     */
    void drawText(String text,int x,int y);
    int getWindowWidth();
    int getWindowHeight();
    int getCanvasWidth();
    int getCanvasHeight();

    /**
     * Establece una fuente
     * @param font
     */
    void setFont(Font font);

}
