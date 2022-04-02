using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace Lab7
{
    class Group : PPoint
    {
        int _count;
        PPoint[] _shapes;
        int min_x;
        int min_y;
        int max_x;
        int max_y;
        int panelWidth = 0;
        int panelHeight = 0;

        public Group()
        {
            _count = 0;
            x = 0;
            y = 0;
            min_x = 0;
            min_y = 0;
            max_x = 0;
            max_y = 0;
            radius = 0;
            isClick = true;
        }

        public void addShape(PPoint obj)
        {
            if (_count == 0)
            {
                _shapes = new PPoint[_count + 1];
                min_x = obj.GetLocationX() - obj.GetRadius() / 2;
                min_y = obj.GetLocationY() - obj.GetRadius() / 2;
                max_x = obj.GetLocationX() + obj.GetRadius() / 2;
                max_y = obj.GetLocationY() + obj.GetRadius() / 2;
            }
            else
                Array.Resize(ref _shapes, _count + 1);
            _shapes[_count] = obj;
            _count++;
            for (int i=0; i<_count; i++)
            {
                int _x = obj.GetLocationX();
                int _y = obj.GetLocationY();
                int _radius = obj.GetRadius() / 2;
                if (min_x == 0 || min_x > _x - _radius)
                    min_x = _x - _radius;
                if (max_x == 0 || max_x < _x + _radius)
                    max_x = _x + _radius;
                if (min_y == 0 || min_y > _y - _radius)
                    min_y = _y - _radius;
                if (max_y == 0 || max_y < _y + _radius)
                    max_y = _y + _radius;
            }
            x = min_x;
            y = min_y;
        }
        public override bool isClicked(MouseEventArgs e)
        {
            if(e.X < max_x && e.X > min_x && e.Y < max_y && e.Y > min_y)
                return true;
            else
                return false;
        }
        public override bool BeIn(Point p1, Point p2)
        {
            if (min_x*2 >= p1.X && max_x/2 <= p2.X && min_y*2 >= p1.Y && max_y/2 <= p2.Y)
                return true;
            else return false;
        }
        public override void HorizontalMove(int _move)
        {
            min_x = min_x + _move;
            max_x = max_x + _move;
            if (min_x >= 0 && max_x <= panelWidth)
                for (int i = 0; i < _count; i++)
                _shapes[i].HorizontalMove(_move);
            if (min_x <= 0 || max_x >= panelWidth)
            {
                min_x = min_x - _move;
                max_x = max_x - _move;
            }
        }
        public override void VerticalMove(int _move)
        {
            min_y = min_y + _move;
            max_y = max_y + _move;
            if (min_y - 70 >= 0 && max_y <= panelHeight)
                for (int i = 0; i < _count; i++)
                    _shapes[i].VerticalMove(_move);
            if (min_y - 70 <= 0 || max_y >= panelHeight)
            {
                min_y = min_y - _move;
                max_y = max_y - _move;
            }
        }
        public override void Draw(PictureBox pb, Graphics g, Bitmap bmp)
        {
            panelHeight = pb.Height;
            panelWidth = pb.Width;
            Rectangle r = new Rectangle(min_x, min_y, Math.Abs(max_x - min_x), Math.Abs(max_y-min_y));
            g.DrawRectangle(Pens.Silver, r);
                for (int i = 0; i < _count; i++)
                _shapes[i].Draw(pb, g, bmp);
        }
        public override void ColorChange(Color color)
        {
            for (int i = 0; i < _count; i++)
                _shapes[i].ColorChange(color);
        }
        public override void MakeClickTrue()//выделить
        {
            isClick = true;
            for (int i = 0; i < _count; i++)
                _shapes[i].MakeClickTrue();
        }
        public override void MakeClickFalse()
        {
            isClick = false;
            for (int i = 0; i < _count; i++)
                _shapes[i].MakeClickFalse();
        }
        public override void Resize(int _radius)
        {
            min_x -= _radius/2;
            min_y -= _radius/2;
            max_x += _radius/2;
            max_y += _radius/2;
            if (min_x >= 0 && min_y >= 85 && max_x <= panelWidth && max_y <= panelHeight)
                for (int i = 0; i < _count; i++)
                    _shapes[i].Resize(_radius);
            else
            {
                if (min_x <= 0)
                {
                    min_x = 0;
                    max_x -= _radius / 2;
                }
                if (min_y <= 85)
                {
                    min_y = 85;
                    max_y -= _radius / 2;
                }
                if (max_x >= panelWidth)
                {
                    max_x = panelWidth;
                    min_x += _radius / 2;
                }
                if (max_y <= panelHeight)
                {
                    max_y = panelHeight;
                    min_y += _radius / 2;
                }
            }
        }
        public bool isEmpty()
        {
            return (_count == 0);
        }
        protected override string className()
        {
            return "Group";
        }
        public override bool isA(string s)
        {
            return (s == className());
        }
        public PPoint returnShape()
        {
            _count--;
            return _shapes[_count];
        }
    }
}
