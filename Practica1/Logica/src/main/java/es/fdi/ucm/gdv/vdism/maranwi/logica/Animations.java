package es.fdi.ucm.gdv.vdism.maranwi.logica;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;

enum AnimationType { Fade, FastMove, SlowMove}

public class Animations {

    public Animations(Celda target){
        _target = target;
        _startTime = -1;
        _doAnimation = false;
        _id = target.getButton().getId();
       _target.getButton().setHasAnimation(true);
    }

    public void startFadeAnimation(MyColor in, MyColor out){
        _type = AnimationType.Fade;
        _colorIn = in;
        _colorOut = out;
        _durationTime = FADE_TIME;
        _velocity = FADE_VELOCITY;
        _doAnimation = true;
    }

    public void startFastMoveAnimation(){
        _type = AnimationType.FastMove;
        _durationTime = FAST_MOVE_TIME;
        _velocity = FAST_MOVE_VELOCITY;
        _borderColor = _target.getColor();
        _baseColor = _target.getColor();
        _radiusMoveAnimation = _target.getButton().getRadius();
        _doAnimation = true;
    }

    public void startSlowMoveAnimation(MyColor c){
        _type = AnimationType.SlowMove;
        _durationTime = SLOW_MOVE_TIME;
        _velocity = SLOW_MOVE_VELOCITY;
        _borderColor = c;
        _baseColor = _target.getColor();
        _radiusMoveAnimation = _target.getButton().getRadius();
        _doAnimation = true;
    }

    //Return false if animation end, true other wise
    public boolean update(double deltaTime){
        if(_startTime == -1) _startTime = deltaTime;

        if (_type != AnimationType.SlowMove && (deltaTime - _startTime >= _durationTime)){
            _target.getButton().setHasAnimation(false);
            return false;
        }


        else if(deltaTime - _lastAnimationTime >= _velocity){
            _doAnimation = true;

            if(_type == AnimationType.FastMove || _type == AnimationType.SlowMove){
                //_radius = _radius + _increment;
                _incrementMoveAnimation += _directionMoveAnimation;

                if(_incrementMoveAnimation <= -OFFSET_SIZE || _incrementMoveAnimation >= OFFSET_SIZE){
                    _directionMoveAnimation *= -1;
                }
                else _radiusMoveAnimation += _incrementMoveAnimation;
            }

            _lastAnimationTime = deltaTime;
        }
        return true;
    }

    public void render(Graphics g){
        if(_doAnimation){
            if(_type == AnimationType.Fade){
                double inverse_blending = 1 - BLENDING_FACTOR;

                double red = 0, green = 0, blue = 0;
                if(_type == AnimationType.Fade){
                    red =   _colorOut.getRed()   * BLENDING_FACTOR +   _colorIn.getRed()   * inverse_blending;
                    green = _colorOut.getGreen() * BLENDING_FACTOR +   _colorIn.getGreen() * inverse_blending;
                    blue =  _colorOut.getBlue()  * BLENDING_FACTOR +   _colorIn.getBlue()  * inverse_blending;

                }
                g.setColor((int) red, (int) green, (int) blue, 255);
                g.fillCircle(_target.getButton().getXPos(), _target.getButton().getYPos(), _target.getButton().getRadius());
            }
            else if(_type == AnimationType.FastMove){
                g.setColor(_baseColor);
                System.out.println("Pintar Animacion FastMove en " +  _target.getButton().getXPos() +","+ _target.getButton().getYPos()+" con radio: " + _target.getButton().getRadius());
                g.fillCircle(_target.getButton().getXPos(), _target.getButton().getYPos(), _target.getButton().getRadius());
            }
            else if( _type == AnimationType.SlowMove){
                g.setColor(_borderColor);
                g.fillCircle(_target.getButton().getXPos(), _target.getButton().getYPos(), _target.getButton().getRadius() + _incrementMoveAnimation);
                g.setColor(_baseColor);
                g.fillCircle(_target.getButton().getXPos(), _target.getButton().getYPos(), _target.getButton().getRadius());
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

    private MyColor _colorOut;
    private MyColor _colorIn;
    private MyColor _baseColor;
    private MyColor _borderColor;

    private int _radiusMoveAnimation;
    private int _incrementMoveAnimation;
    private int _directionMoveAnimation;
    private static final int OFFSET_SIZE = 10;

    private static final double BLENDING_FACTOR = 0.5;
    private static final double FADE_TIME = 1000;
    private static final double FAST_MOVE_TIME = 1000000000;
    private static final double SLOW_MOVE_TIME = 1000000000;

    private static final double FADE_VELOCITY = 10;
    private static final double FAST_MOVE_VELOCITY = 10;
    private static final double SLOW_MOVE_VELOCITY = 100;
}
