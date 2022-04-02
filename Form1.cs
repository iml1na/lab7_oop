using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Reflection;
using System.IO;


namespace Lab7
{
    public partial class Form1 : Form
    {
        MyStorage storage;
        Bitmap bmp = new Bitmap(2000, 2000);
        Graphics g;
        int selectShape = 1;
        SaveClass saveClass = new SaveClass();

        Dictionary<Keys, Command> commands = new Dictionary<Keys, Command>();
        Stack<Command> history;
        public Form1()
        {
            InitializeComponent();
            storage = saveClass.LoadShapes();
            storage.AllNotChecked();
            paintBox.Invalidate();
            commands[Keys.W] = new MoveVerticalCommand(-5);
            commands[Keys.A] = new MoveHorizontalCommand(-5);
            commands[Keys.S] = new MoveVerticalCommand(5);
            commands[Keys.D] = new MoveHorizontalCommand(5);
            commands[Keys.E] = new ResizeCommand(5);
            commands[Keys.Q] = new ResizeCommand(-5);

            history = new Stack<Command>();
        }

        private void paintBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (chbGroup.Checked == false)
            {
                if (storage.isCheckedStorage(e) == false)//нажатие на пустое место
                {
                    storage.AllNotChecked();
                    switch (selectShape)
                    {
                        case 0:
                            storage.addObject(new Circle(e.X, e.Y, 50));//добавление нового круга в хранилище
                            break;
                        case 1:
                            storage.addObject(new Square(e.X, e.Y, 50));//добавление квадрата
                            break;
                        case 2:
                            storage.addObject(new Triangle(e.X, e.Y, 50));//добавление треугольника
                            break;
                        case 3:
                            storage.addObject(new Line(e.X, e.Y, 50));//добавление отрезка
                            break;

                    }
                }
                else//если нажать на круг и нажата ctrl, то можно выделить несколько кругов
                if (Control.ModifierKeys == Keys.Control)
                    storage.MakeCheckedObjectStorage(e);
                else//иначе выделяется только один круг
                {
                    storage.AllNotChecked();
                    storage.MakeCheckedObjectStorage(e);
                }
                paintBox.Invalidate();
            }
        }

        Point p1, p2;
        private void paintBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (chbGroup.Checked == true)
            {
                p1 = new Point(e.X, e.Y);
                storage.AllNotChecked();
            }
        }

        private void paintBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (chbGroup.Checked == true)
            {
                p2 = new Point(e.X, e.Y);
                Rectangle r = new Rectangle(p1.X, p1.Y, Math.Abs(p2.X - p1.X), Math.Abs(p2.Y - p1.Y));
                Pen pen = new Pen(Color.Silver);
                g.DrawRectangle(pen, r);
                paintBox.Image = bmp;
                Group group = new Group();
                for (int i = 0; i < storage.getSize(); i++)
                    if (storage.getObject(i).BeIn(p1, p2) == true)
                    {
                        storage.getObject(i).MakeClickTrue();
                        group.addShape(storage.getObject(i));
                        storage.removeObject(i);
                        i = i - 1;
                    }
                if (group.isEmpty() == false)
                    storage.addObject(group);
            }
        }

        private void paintBox_Paint(object sender, PaintEventArgs e)
        {
            g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            storage.DrawAll(paintBox, g, bmp);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.D || e.KeyCode == Keys.W || e.KeyCode == Keys.S || e.KeyCode == Keys.Q || e.KeyCode == Keys.E)
            {
                Command command = commands[e.KeyCode];
                Command newcommand = command.clone();
                for (int i = 0; i < storage.getSize(); i++)
                    if (storage.getObject(i).IsClick() == true)
                        newcommand.execute(storage.getObject(i));
                history.Push(newcommand);
            }
            else if (e.KeyCode == Keys.Z && history.Count != 0)
            {
                Command lastcommand = history.Peek();
                lastcommand.unexecute();
                history.Pop();
            }
            else if (e.KeyCode == Keys.Delete)
            {
                storage.removeCheckedObject();
                g.Clear(Color.White);
            }
        }

        private void rbCircle_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            selectShape = rb.TabIndex;
        }

        private void panelColor_Click(object sender, EventArgs e)
        {
            Panel p = (Panel)sender;
            Color color = new Color();
            if (p == pRed)
                color = Color.Red;
            else if (p == pPink)
                color = Color.Fuchsia;
            else if (p == pGreen)
                color = Color.Lime;
            else if (p == pYellow)
                color = Color.Yellow;
            else if (p == pBlue)
                color = Color.Blue;
            else color = Color.White;
            storage.ColorChange(color);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            chbGroup.Checked = false;
            saveClass.SaveShapes(storage);
        }

        private void chbGroup_CheckedChanged(object sender, EventArgs e)
        {
            storage.AllNotChecked();
            if (chbGroup.Checked == false)
                for (int i = 0; i < storage.getSize(); i++)
                    if (storage.getObject(i).isA("Group") == true)
                    {
                        Group gr = (Group)storage.getObject(i);
                        storage.removeObject(i);
                        while (gr.isEmpty() == false)
                            storage.addObject(gr.returnShape());
                        i--;
                    }
        }
       
    }
}
