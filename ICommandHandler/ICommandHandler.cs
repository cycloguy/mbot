using EmailModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ICommandHandler
{
  public  delegate void PostBack(object sender, MyEmailEventArgs e);
    public interface ICommandHandler
    {
        void Process(MyEmailEventArgs e);
        event PostBack OnPostBack;

    }
}
