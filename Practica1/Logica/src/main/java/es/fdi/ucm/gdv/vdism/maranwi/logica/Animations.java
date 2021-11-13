package es.fdi.ucm.gdv.vdism.maranwi.logica;
import es.fdi.ucm.gdv.vdism.maranwi.engine.Graphics;

enum AnimationType { Fade, FastMove, SlowMove}

public class Animations {

    public Animations(Celda target){
        _target = target;
        _startTime = -1;
        _time = 0;
        _id = target.getButton().getId();
       _target.getButton().setHasAnimation(true);
    }

    public void startFadeAnimation(MyColor in, MyColor out){
        _type = AnimationType.Fade;
        _colorIn = in;
        _colorOut = out;
        _durationTime = FADE_TIME;
        _velocity = FADE_VELOCITY;
    }

    public void startFastMoveAnimation(){
        _type = AnimationType.FastMove;
        _durationTime = FAST_MOVE_TIME;
        _velocity = FAST_MOVE_VELOCITY;
        _borderColor = _target.getColor();
        _baseColor = _target.getColor();
        _directionMoveAnimation = 1;
        _incrementMoveAnimation = INCREMENT_ANIMATIONS_SIZE;
    }

    public void startSlowMoveAnimation(MyColor c){
        _type = AnimationType.SlowMove;
        _durationTime = SLOW_MOVE_TIME;
        _velocity = SLOW_MOVE_VELOCITY;
        _borderColor = c;
        _baseColor = _target.getColor();
        _directionMoveAnimation = 1;
        _incrementMoveAnimation = INCREMENT_ANIMATIONS_SIZE;
    }

    //Return false if animation end, true other wise
    public boolean update(double deltaTime){
        if(_startTime == -1) _startTime = deltaTime;
        _time += deltaTime;

        if (_type != AnimationType.SlowMove && (_time - _startTime >= _durationTime)){
            _target.getButton().setHasAnimation(false);
            return false;
        }


        else if(_time - _lastAnimationTime >= _velocity){
            if(_type == AnimationType.FastMove || _type == AnimationType.SlowMove){
                //_radius = _radius + _increment;
                _incrementMoveAnimation += (INCREMENT_ANIMATIONS_SIZE * _directionMoveAnimation);

                if(_incrementMoveAnimation <= -OFFSET_MOVES_ANIMATIONS_SIZE || _incrementMoveAnimation >= OFFSET_MOVES_ANIMATIONS_SIZE){
                    _directionMoveAnimation *= -1;
                }
            }
            _lastAnimationTime = _time;
        }
        return true;
    }

    public void render(Graphics g){
        if(_type == AnimationType.Fade){
            double inverse_blending = 1 - BLENDING_FACTOR;

            double red = 0, green = 0, blue = 0;
            if(_type == AnimationType.Fade){
                red =   (_colorOut.getRed()   * BLENDING_FACTOR) +   (_colorIn.getRed()   * inverse_blending);
                green = (_colorOut.getGreen() * BLENDING_FACTOR) +   (_colorIn.getGreen() * inverse_blending);
                blue =  (_colorOut.getBlue()  * BLENDING_FACTOR) +   (_colorIn.getBlue()  * inverse_blending);

            }
//            System.out.println("Color in. R: " + _colorIn.getRed() + " G: " + _colorIn.getGreen() + " B: " + _colorIn.getBlue());
//            System.out.println("Color out. R: " + _colorOut.getRed() + " G: " + _colorOut.getGreen() + " B: " + _colorOut.getBlue());
//            System.out.println("Fade with colors -> Red: " + red + " Green:" + green + " Blue: " + blue);
            g.setColor((int) red, (int) green, (int) blue, 255);
            g.fillCircle(_target.getButton().getXPos(), _target.getButton().getYPos(), _target.getButton().getRadius());
        }
        else if(_type == AnimationType.FastMove){
            g.setColor(_baseColor);
            g.fillCircle(_target.getButton().getXPos(), _target.getButton().getYPos(), _target.getButton().getRadius());
        }
        else if( _type == AnimationType.SlowMove){
            g.setColor(_borderColor);
            g.fillCircle(_target.getButton().getXPos(), _target.getButton().getYPos(), _target.getButton().getRadius() + _incrementMoveAnimation);
            g.setColor(_baseColor);
            g.fillCircle(_target.getButton().getXPos(), _target.getButton().getYPos(), _target.getButton().getRadius());
        }

    }

    public String getId() { return _id;}

    private String _id;
    private Celda _target;
    private AnimationType _type;

    private double _time;
    private double _startTime;
    private double _lastAnimationTime;
    private double _durationTime;
    private double _velocity;

    private MyColor _colorOut;
    private MyColor _colorIn;
    private MyColor _baseColor;
    private MyColor _borderColor;

    private int _incrementMoveAnimation;
    private int _directionMoveAnimation;

    private static final double BLENDING_FACTOR = 0.5;
    private static final double FADE_TIME = 0.05;
    private static final double FADE_VELOCITY = 0.005;

    private static final int INCREMENT_ANIMATIONS_SIZE = 2;
    private static final int OFFSET_MOVES_ANIMATIONS_SIZE = 8;
    private static final double FAST_MOVE_TIME = 1;
    private static final double FAST_MOVE_VELOCITY = 0.005;

    private static final double SLOW_MOVE_TIME = 1;
    private static final double SLOW_MOVE_VELOCITY = 1;
}
