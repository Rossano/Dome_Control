using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arduino.Dome
{
    public enum Status
    {
        TURN_LEFT,
        TURN_RIGHT,
        NO_TURN
    };

    public class Dome_Movement_Class
    {
        public double _current_pos { get; set; }
        public double _target_pos { get; set; }
        public Dome_Movement_Class(double cur_pos)
        {
            if (cur_pos < 360 && cur_pos >= 0) _current_pos = cur_pos;
            else throw new ArgumentOutOfRangeException("Invalid current position angle as argument!");
        }

        public Status find_rotation_sense(double target)
        {
            if (target < 360 && target >= 0) _target_pos = target;
            else throw new ArgumentOutOfRangeException("Invalid target angle as argument!");

//            Angle thetaAngle = theta;
//            Angle AzimuthAngle = Azimuth;
//            if ((thetaAngle - AzimuthAngle) >= (AzimuthAngle - thetaAngle))
            if (Math.Abs(_target_pos - _current_pos) <= 2) return Status.NO_TURN;
            if ((_target_pos - _current_pos) > (_target_pos - _current_pos))
            {
 //               tl.LogMessage("SlewToAzimuth", "Slewing ANTICLOCKWISE");
 //               _arduino.Slew(Direction.ANTICLOCWISE);
                return Status.TURN_LEFT;
            }
            else
            {
//                tl.LogMessage("SlewToAzimuth", "Slewing CLOCKWISE");
//                _arduino.Slew(Direction.CLOCKWISE);
                return Status.TURN_RIGHT;
            }

            if((_current_pos - _target_pos) == 0) return Status.NO_TURN;
            if (Math.Abs(_current_pos - target) > 0) return Status.TURN_LEFT;
            else return Status.TURN_RIGHT;
        }
    }
}
