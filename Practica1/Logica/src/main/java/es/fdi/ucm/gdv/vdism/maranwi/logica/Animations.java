package es.fdi.ucm.gdv.vdism.maranwi.logica;

import es.fdi.ucm.gdv.vdism.maranwi.engine.Color;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;

enum AnimationType { Fade, FastMove, SlowMove}

public class Animations {

    public Animations(Celda target, AnimationType type, long velocity, long durationTime){
        _target = target;
        _type = type;
        _velocity = velocity;
        _durationTime = durationTime;
        _startTime = 0;
        _doAnimation = false;
    }

    public void newFadeAnimation(AnimationType type, Color in, Color out, float blendingFactor, double duration, double velocity){
        _type = type;
        _colorIn = in;
        _colorOut = out;
        _colorBlendingFactor = blendingFactor;
        _durationTime = duration;
        _velocity = velocity;
    }

    public void setMoveAnimation(AnimationType type, int color, double duration, double velocity ){
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
            if(_type == AnimationType.Fade){
                Color x,y; //set by you
                float blending;//set by you

                float inverse_blending = 1 - _colorBlendingFactor;

                float red = 0, green = 0, blue = 0;
                if(_type == AnimationType.Fade){
                    red =   _colorOut.getRed()   * _colorBlendingFactor +   _colorIn.getRed()   * inverse_blending;
                    green = _colorOut.getGreen() * _colorBlendingFactor +   _colorIn.getGreen() * inverse_blending;
                    blue =  _colorOut.getBlue()  * _colorBlendingFactor +   _colorIn.getBlue()  * inverse_blending;

                }

                //Color blended = new Color (red / 255, green / 255, blue / 255);
//                Color.red(int color)
//                Color.blue(int color)
//                Color.green(int color)
                //g.setColor(getIntFromColor((int) red, (int) green, (int) blue));
                //g.fillCircle(_xPos, _yPos, _radius);
            }

            else if(_type == AnimationType.FastMove || _type == AnimationType.SlowMove){
                g.setColor(_colorMoveAnimation);
                //g.fillCircle(_xPos, _yPos, _radius);
            }
            _doAnimation = false;
        }
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
