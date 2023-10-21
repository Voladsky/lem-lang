using Parser;
using Lexer;
using System.Xml.Linq;
using ParserHelper;

namespace Lem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var fname = "D:\\Sci/lem-lang/Lem/basic.lem";
            try
            {   
                Scanner scanner = new Scanner(new FileStream(fname, FileMode.Open));
                var par = new Parser.Parser(scanner);
                var b = par.Parse();
                var root = par.root;
                var interpreter = new InterpretVisitor();
                root.Visit(interpreter);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл {0} не найден", fname);
            }
            catch (LexException e)
            {
                Console.WriteLine("Лексическая ошибка. " + e.Message);
            }
            catch (SyntaxException e)
            {
                Console.WriteLine("Синтаксическая ошибка. " + e.Message);
            }
        }
    }
}