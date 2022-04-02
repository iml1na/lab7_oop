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
    class PPoint
    {
        protected bool isClick = false;
        protected int x;
        protected int y;
        protected int radius;
        protected Pen pen = new Pen(Color.Black);
        virtual public void MakeClickTrue()//выделить
        {
            isClick = true;
        }
        virtual public void MakeClickFalse()
        {
            isClick = false;
        }
        virtual public void Draw(PictureBox pb, Graphics g, Bitmap bmp)//метод, рисующий фигуры 
        {
            if (isClick == true)
                pen.Color = Color.Blue;
            else
                pen.Color = Color.Black;
            if (radius < 5)
                radius = 5;
        }
        virtual public bool isClicked(MouseEventArgs e)//нажато ли в область фигуры
        {
            return false;
        }
        virtual public void HorizontalMove(int _move)
        {
            x = x + _move;
        }
        virtual public void VerticalMove(int _move)
        {
            y = y + _move;
        }
        virtual public void Resize(int _radius)
        {
            radius = radius + _radius;
        }
        virtual protected void CheckBoard(PictureBox pb)
        {
            if ((x + radius / 2) > pb.Width)//проверяет на границы по ширине
                x = pb.Width - (radius / 2);
            else if ((x - radius / 2) < 0)
                x = radius / 2;
            if ((y + radius / 2) > pb.Height)//проверка на границы по высоте
                y = pb.Height - (radius / 2);
            else if ((y - radius / 2 - 70) < 0)
                y = 70 + radius / 2;
            if (radius > pb.Width)//проверка на радиус, больше он формы или нет
                radius = pb.Width;
            if (radius > pb.Height - 70)
                radius = pb.Height - 70;
        }
        public bool IsClick()//возвращает нажато ли на фигуру
        {
            return isClick;
        }
        virtual public bool BeIn(Point p1, Point p2)
        {
            if ((x >= p1.X - radius / 2 && x <= p2.X + radius / 2) && (y >= p1.Y - radius / 2 && y <= p2.Y + radius / 2))
                return true;
            else return false;
        }
        virtual public void ColorChange(Color color) { }
        virtual public bool isA(string s)
        {
            return (s == "PPoint");
        }
        virtual public int GetLocationX()
        {
            return x;
        }
        virtual public int GetLocationY()
        {
            return y;
        }
        virtual public Point GetLocation()
        {
            return new Point(x, y);
        }
        public int GetRadius()
        {
            return radius;
        }
        virtual protected string className()
        {
            return "PPoint";
        }
        virtual public void Save(StreamWriter stream)
        {
            stream.WriteLine(className());
            SaveValues(stream);
        }
        virtual protected void SaveValues(StreamWriter stream)
        {
            stream.WriteLine(x + " "+ y + " " + radius);
        }
        virtual public void Load(StreamReader stream)
        {
            string[] words = stream.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            x = Int32.Parse(words[0]);
            y = Int32.Parse(words[1]);
            radius = Int32.Parse(words[2]);
        }        
    }
    class Shape: PPoint
    {
        protected SolidBrush brush = new SolidBrush(Color.White);      
        public override void ColorChange(Color color)
        {
            brush.Color = color;
        }
        protected override void SaveValues(StreamWriter stream)
        {
            stream.WriteLine(x + " " + y + " " + radius + " " + brush.Color.ToString());
        }
        public override void Load(StreamReader stream)
        {
            string[] words = stream.ReadLine().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            x = Int32.Parse(words[0]);
            y = Int32.Parse(words[1]);
            radius = Int32.Parse(words[2]);
            words[4] = words[4].Trim(new char[] { '[', ']' });
            ColorChange(Color.FromName(words[4]));
        }
    }

    class Circle : Shape
    {
        public Circle(int x, int y, int radius)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
            isClick = true;
        }

        public override void Draw(PictureBox pb, Graphics g, Bitmap bmp)//метод, который рисует круг 
        {
            Rectangle r = new Rectangle((x - (radius / 2)), (y - (radius / 2)), radius, radius);
            base.Draw(pb, g, bmp);
            CheckBoard(pb);
            g.FillEllipse(brush, r);
            g.DrawEllipse(pen, r);
            pb.Image = bmp;
        }
        public override bool isClicked(MouseEventArgs e)//нажато ли в область круга
        {
            if (((e.X - x) * (e.X - x) + (e.Y - y) * (e.Y - y)) <= radius/2 * radius/2)
                return true;
            else
                return false;
        }
        protected override string className()
        {
            return "Circle";
        }
    }
    class Square : Shape
    {
        public Square(int x, int y, int radius)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
            isClick = true;
        }
        public override void Draw(PictureBox pb, Graphics g, Bitmap bmp)//метод, который рисует квадрат 
        {
            Rectangle r = new Rectangle((x - (radius / 2)), (y - (radius / 2)), radius, radius);
            base.Draw(pb, g, bmp);
            CheckBoard(pb);
            g.FillRectangle(brush, r);
            g.DrawRectangle(pen, r);
            pb.Image = bmp;
        }
        public override bool isClicked(MouseEventArgs e)//нажато ли в область квадрата
        {
            if (e.X < x + radius/2 && e.X > x - radius/2 && e.Y < y + radius/2 && e.Y > y - radius/2)
                return true;
            else
                return false;
        }
        protected override string className()
        {
            return "Square";
        }
    }
    class Triangle : Shape
    {
        private Point[] points;
        public Triangle(int x, int y, int radius)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
            isClick = true;
            points = new Point[3];
        }
        public override void Draw(PictureBox pb, Graphics g, Bitmap bmp)
        {
            points[0].X = x; points[0].Y = y - radius / 2;
            points[1].X = x - radius / 2; points[1].Y = y + radius / 2;
            points[2].X = x + radius / 2; points[2].Y = y + radius / 2;
            base.Draw(pb, g, bmp);
            CheckBoard(pb);
            g.FillPolygon(brush, points);
            g.DrawPolygon(pen, points);
            pb.Image = bmp;
        }
        public override bool isClicked(MouseEventArgs e)
        {
            if (e.X < x + radius/2 && e.X > x - radius/2 && e.Y < y + radius/2 && e.Y > y - radius/2)
                return true;
            else
                return false;
        }
        protected override string className()
        {
            return "Triangle";
        }
    }
    class Line : PPoint
    {
        public Line(int x, int y, int radius)
        {
            this.x = x;
            this.y = y;
            this.radius = radius;
            isClick = true;
        }
        public override void Draw(PictureBox pb, Graphics g, Bitmap bmp)
        {
            base.Draw(pb, g, bmp);
            CheckBoard(pb);
            g.DrawLine(pen, x + radius / 2, y + radius / 2, x - radius / 2, y - radius / 2);
            pb.Image = bmp;
        }
        public override bool isClicked(MouseEventArgs e)
        {
            if (((e.X - x) * (e.X - x) + (e.Y - y) * (e.Y - y)) <= radius / 4 * radius / 4)
                return true;
            else
                return false;
        }
        protected override string className()
        {
            return "Line";
        }
    }
}
