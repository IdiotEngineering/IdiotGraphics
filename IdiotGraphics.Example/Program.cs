using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IdiotGraphics.Example
{
  class Program
  {
    static void Main(string[] args)
    {
      var window = new Window("Hello, IdiotGraphics", 800, 600);
      window.Visiable = true;
      while (true)
      {
        window.Update();
        window.Render();
        Thread.Sleep(15);
      }
    }
  }
}
