/*
 *  Program    -> Statements
 *  Statements -> Statements Statement
 *             |  eps
 *  Statement  -> if ( Expression ) Statement
 *             |  while ( Expression ) Statement
 *             |  { Statements }
 *             |  id = Expression
 *  expression     → equality ;
 *  equality       → comparison ( ( "!=" | "==" ) comparison )* ;
 *  comparison     → term ( ( ">" | ">=" | "<" | "<=" ) term )* ;
 *  term           → factor ( ( "-" | "+" ) factor )* ;
 *  factor         → unary ( ( "/" | "*" ) unary )* ;
 *  unary          → ( "!" | "-" ) unary
 *                 | primary ;
 *  primary        → NUMBER | STRING | "true" | "false" | "nil"
 *                 | "(" expression ")" ;
*/

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
            /*
                        var x = NodeCreation.Id("x");
                        var one = NodeCreation.Num(1.0);
                        var ten = NodeCreation.Num(10.0);

                        var progr = NodeCreation.StL(
                            NodeCreation.Ass(x, one),
                            NodeCreation.Wh(NodeCreation.Bin(x, "<", ten),
                                NodeCreation.StL(
                                   NodeCreation.ProcCall(NodeCreation.Id("print"), NodeCreation.ExL(x)),
                                   NodeCreation.Ass(x, NodeCreation.Bin(x, "+", one))
                                )
                            )
                        );
                        var interpret = new InterpretVisitor();
                        progr.Visit(interpret);
                        var lex = new Lexer("a = 3.0; if\na<2.0\n\nthen a = 0.0 else a = 23.0;\n print(a * 223.0 + 10.0) ");
                        Token t = null;
                        do
                        {
                            t = lex.NextToken();
                            Console.WriteLine($"{t.pos},{t.type},{t.value}");
                        } while (t.type != TokenType.Eof);

                        var par = new Parser(lex);
                        var root = par.MainProgram();
                        var int2 = new InterpretVisitor();
                        root.Visit(int2);

            */
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