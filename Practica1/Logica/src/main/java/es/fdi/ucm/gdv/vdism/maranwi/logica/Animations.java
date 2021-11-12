package es.fdi.ucm.gdv.vdism.maranwi.logica;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Color;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;

enum AnimationType { FadeIn, FadeOut, FastMove, SlowMove}

public class Animations {

    public Animations(Celda target, AnimationType type, long velocity, long durationTime){
        _target = target;
        _type = type;
        _velocity = velocity;
        _durationTime = durationTime;
        _startTime = 0;
        _doAnimation = false;
    }

    public void setFadeAnimation(AnimationType type, Color in, Color out, float blendingFactor, double duration, double velocity){
        _type = type;
        _colorIn = in;
        _colorOut = out;
        _colorBlendingFactor = blendingFactor;
        _durationTime = duration;
        _velocity = velocity;
    }

    public void setMove(AnimationType type, int color, double duration, double velocity ){
        _type = type;
        _colorMoveAnimation = color;
        _durationTime = duration;
        _velocity = velocity;
    }

    //Return false if animation end, true other wise
    public boolean update(double deltaTime){
        if (_type != AnimationType.SlowMove && (deltaTime - _startTime >= _durationTime))
            return false;
        else if(System.nanoTime() - _lastAnimationTime >= _velocity){
            _doAnimation = true;

            if(_type == AnimationType.FastMove || _type == AnimationType.SlowMove){
                //_radius = _radius + _increment;
                _increment += _direction;

                if(_increment <= -OFFSET_SIZE || _increment >= OFFSET_SIZE){
                    _direction *= -1;
                }
            }

            _lastAnimationTime = deltaTime;
        }
        return true;
    }

    public void render(Graphics g){
        if(_doAnimation){
            if(_type == AnimationType.FadeIn || _type == AnimationType.FadeOut){
                Color x,y; //set by you
                float blending;//set by you

                float inverse_blending = 1 - _colorBlendingFactor;

                float red = 0, green = 0, blue = 0;
                if(_type == AnimationType.FadeIn){
                    red =   _colorOut.getRed()   * _colorBlendingFactor +   _colorIn.getRed()   * inverse_blending;
                    green = _colorOut.getGreen() * _colorBlendingFactor +   _colorIn.getGreen() * inverse_blending;
                    blue =  _colorOut.getBlue()  * _colorBlendingFactor +   _colorIn.getBlue()  * inverse_blending;

                }else if(_type == AnimationType.FadeOut){
                    red =   _colorIn.getRed()   * _colorBlendingFactor +   _colorOut.getRed()   * inverse_blending;
                    green = _colorIn.getGreen() * _colorBlendingFactor +   _colorOut.getGreen() * inverse_blending;
                    blue =  _colorIn.getBlue()  * _colorBlendingFactor +   _colorOut.getBlue()  * inverse_blending;
                }

                //Color blended = new Color (red / 255, green / 255, blue / 255);
//                Color.red(int color)
//                Color.blue(int color)
//                Color.green(int color)
                g.setColor(getIntFromColor((int) red, (int) green, (int) blue));
                //g.fillCircle(_xPos, _yPos, _radius);
            }

            else if(_type == AnimationType.FastMove || _type == AnimationType.SlowMove){
                g.setColor(_colorMoveAnimation);
                //g.fillCircle(_xPos, _yPos, _radius);
            }
            _doAnimation = false;
        }
    }

    private int getIntFromColor(int Red, int Green, int Blue){
        int R = Math.round(255 * Red);
        int G = Math.round(255 * Green);
        int B = Math.round(255 * Blue);

        R = (R << 16) & 0x00FF0000;
        G = (G << 8) & 0x0000FF00;
        B = B & 0x000000FF;

        return 0xFF000000 | R | G | B;
    }

    private int getIntFromColor2(int Red, int Green, int Blue){
        Red = (Red << 16) & 0x00FF0000; //Shift red 16-bits and mask out other stuff
        Green = (Green << 8) & 0x0000FF00; //Shift Green 8-bits and mask out other stuff
        Blue = Blue & 0x000000FF; //Mask out anything not blue.

        return 0xFF000000 | Red | Green | Blue; //0xFF000000 for 100% Alpha. Bitwise OR everything together.
    }

    public String getId() { return _id;}

    private String _id;
    private Celda _target;
    private AnimationType _type;

    private boolean _doAnimation;

    private double _startTime;
    private double _lastAnimationTime;
    private double _durationTime;
    private double _velocity;

    private Color _colorOut;
    private Color _colorIn;
    private int _colorMoveAnimation;
    private float _colorBlendingFactor;

    private int _increment;
    private int _direction;
    private static final int OFFSET_SIZE = 10;
}
