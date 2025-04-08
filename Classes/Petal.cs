using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JapanezePuzzle.Classes
{
    // Class for background fallyng flowers/petals
    public class Petal
    {
        private float _x;
        private float _y;
        private float _speedX;
        private float _speedY;
        private float _rotation;
        private float _rotationSpeed;

        public float X
        {
            get { return _x; }
            set { _x = value; }
        }
        public float Y
        {
            get { return _y; }
            set { _y = value; }
        }
        public float SpeedX
        {
            get { return _speedX; }
            set { _speedX = value; }
        }
        public float SpeedY
        {
            get { return _speedY; }
            set { _speedY = value; }
        }
        public float Rotation
        {
            get { return _rotation; }
            set { _rotation = value; }
        }
        public float RotationSpeed
        {
            get { return _rotationSpeed; }
            set { _rotationSpeed = value; }
        }
    }
}
