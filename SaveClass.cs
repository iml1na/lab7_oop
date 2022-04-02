using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lab7
{
    class SaveClass
    {
        string path = @"C:\Users\Аделина\source\repos\Lab7\Lab7\Shapes.txt";
        FileStream file;
        int size;
        PPoint[] shapes;
        public SaveClass()
        {
            file = new FileStream(path, FileMode.OpenOrCreate);
        }
        public void SaveShapes(MyStorage storage)
        {
            file.SetLength(0);
            StreamWriter stream = new StreamWriter(file);
            size = storage.getSize();
            shapes = new PPoint[size];
            stream.WriteLine(size);
            for (int i = 0; i < size; i++)
                storage.getObject(i).Save(stream);
            stream.Close();
            file.Close();
        }
        public MyStorage LoadShapes()
        {
            StreamReader stream = new StreamReader(file);
            size = Int32.Parse(stream.ReadLine());
            shapes = new PPoint[size];
            MyStorage storage = new MyStorage();
            CreateShape crs = new CreateShape();
            for (int i = 0; i < size; i++)
            {
                shapes[i] = crs.create(stream);
                if (shapes[i] != null)
                storage.addObject(shapes[i]);               
            }
            //size = 0;
            return storage;
        }
    }
}
