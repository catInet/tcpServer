using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tcpServerPublish
{
    class MessageGenerator
    {
        public static string generateMessage(int classNumber)
        {
            switch (classNumber)
            {
                case 1:
                    return "Прыг-скок, еще выше!";
          
                case 2:
                    return "Побежали!";
     
                case 3:
                    return "Я куда-то лезу, зато жив!";
        
                case 4:
                    return "Гуляю, медленно и неторопясь";
           
                case 5:
                    return "Весь день бы так лежал!";

                case 6:
                    return "Сижу, жду, угадай кого!";

                case 7:
                    return "Интересно, коты могут стоять.";

                default:
                    return "Занят чем-то странны. Подебаж меня!";
            }
        }
    }
}
