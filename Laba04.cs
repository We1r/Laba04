using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using System.IO;
  
namespace Laba04
{
  public class Laba04
  {
    public class Memento
    {
      public string Text { get; set; }
    }
    public interface IOriginator
    {
      object GetMemento();
      void SetMemento(object memento);
    }

   [Serializable]
    public class FullText: IOriginator
    {
      public string Text { get; set; }
      public FullText() { }
      public FullText(string text)
      {
        Text = text;
      }
      public void XmlSerialize(FileStream fs)
      {
        XmlSerializer xs = new XmlSerializer(typeof(FullText));
        xs.Serialize(fs, this);
        fs.Flush();
        fs.Close();
      }

      public void XmlDeserialize(FileStream fs)
      {
        XmlSerializer xs = new XmlSerializer(typeof(FullText));
        FullText deserialized = (FullText)xs.Deserialize(fs);
        Text = deserialized.Text;
        fs.Close();
      }
      public void Serialize(FileStream fs)
      {
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, this);
        fs.Flush();
        fs.Close();
      }
      public void Deserialize(FileStream fs)
      {
        BinaryFormatter bf = new BinaryFormatter();
        FullText deserialized = (FullText)bf.Deserialize(fs);
        Text = deserialized.Text;
        fs.Close();
      }
      public void Print()
      {
        Console.WriteLine(Text);
      }
      object IOriginator.GetMemento()
      {
        return new Memento
        { Text = this.Text };
      }
      void IOriginator.SetMemento(object memento) 
      { 
        if (memento is Memento mem) 
        {
          Text = mem.Text;  
        } 
      }

    }
    static void Main(string[] args)
    {
      FullText fnc = new FullText();
      string str = "";
      Memento BinMemento = null;
      Memento XmlMemento = null;
      while (true) {
        Console.WriteLine("|1 - бинарная |" + "\n" + "|2 - Xml      |" + "\n" + "|3 - выход    |");
        Console.Write("Введите команду: ");
        int command = int.Parse(Console.ReadLine());
        Console.Write("\n");
        if (command == 1)
        {
          while (true)
          {

            Console.WriteLine("|1 - сериализация        |" + "\n" + "|2 - десериализация      |" + "\n" + "|3 - отменить измененеия |" + "\n" + "|4 - назад               |");
            Console.Write("Введите команду: ");
            int commandBin = int.Parse(Console.ReadLine());
            Console.Write("\n");
            if (commandBin == 1)
            {
              BinMemento = (Memento)((IOriginator)fnc).GetMemento();
              Console.WriteLine("Введите текст: ");
              str = Console.ReadLine();
              FileStream fs = new
                FileStream("C:\\Users\\Егор\\Documents\\GitHab\\Laba04\\Text\\Text.bin",
                FileMode.OpenOrCreate, FileAccess.Write);
              fnc = new FullText(str);
              Console.Write("Сериализация...\n");
              fnc.Serialize(fs);
              Console.WriteLine("");
            } else if (commandBin == 2)
            {
              FileStream fs = new
                FileStream("C:\\Users\\Егор\\Documents\\GitHab\\Laba04\\Text\\Text.bin",
                FileMode.OpenOrCreate, FileAccess.Read);
              fnc = new FullText();            
              fnc.Deserialize(fs);
              Console.Write("Дессериализация...\n");
              fnc.Print();
              Console.Write("\n");
            } else if (commandBin == 3)
            {
              if (BinMemento != null)
              {
                Console.WriteLine("Отмена изменений...");
                ((IOriginator)fnc).SetMemento(BinMemento);
                FileStream fs = new
                  FileStream("C:\\Users\\Егор\\Documents\\GitHab\\Laba04\\Text\\Text.bin",
                  FileMode.OpenOrCreate, FileAccess.Write);
                fnc.Serialize(fs);
                fnc.Print();
                Console.Write("\n");
              }
              else
              {
                Console.WriteLine("Нет сохраненных состояний.");
              }
            } else if (commandBin == 4)
            {
              break;
            }
          }
        }
        else if (command == 2)
          while (true) {
            {
              Console.WriteLine("|1 - сериализация        |" + "\n" + "|2 - десериализация      |" + "\n" + "|3 - отменить измененеия |" + "\n" + "|4 - назад               |");
              Console.Write("Введите команду: ");
              int commandXml = int.Parse(Console.ReadLine());
              Console.Write("\n");
              if (commandXml == 1)
              {
                XmlMemento = (Memento)((IOriginator)fnc).GetMemento();
                Console.WriteLine("Введите текст: ");
                str = Console.ReadLine();
                FileStream fs = new
                  FileStream("C:\\Users\\Егор\\Documents\\GitHab\\Laba04\\Text\\Text2.xml",
                  FileMode.OpenOrCreate, FileAccess.Write);
                fnc = new FullText(str);
                Console.Write("Сериализация...\n");
                fnc.XmlSerialize(fs);
                Console.WriteLine("");
              } else if (commandXml == 2)
              {
                FileStream fs = new
                  FileStream("C:\\Users\\Егор\\Documents\\GitHab\\Laba04\\Text\\Text2.xml",
                  FileMode.OpenOrCreate, FileAccess.Read);
                fnc = new FullText();
                Console.Write("Дессериализация...\n");
                fnc.XmlDeserialize(fs);
                Console.Write("\n");
                fnc.Print();
                Console.Write("\n");
              } else if (commandXml == 3)
              {
                if (XmlMemento != null)
                {
                  Console.WriteLine("Отмена изменений...");
                  ((IOriginator)fnc).SetMemento(XmlMemento);
                  FileStream fs = new
                    FileStream("C:\\Users\\Егор\\Documents\\GitHab\\Laba04\\Text\\Text2.xml",
                    FileMode.OpenOrCreate, FileAccess.Write);
                  fnc.XmlSerialize(fs);
                  fnc.Print();
                  Console.Write("\n");
                }
                else
                {
                  Console.WriteLine("Нет сохраненных состояний.");
                }
              } else if (commandXml == 4)
              {
                break;
              }
            }
          } else if (command == 3)
          {
            break;
          }
        }
      }
  }
}
