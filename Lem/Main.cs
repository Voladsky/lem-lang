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


namespace Lem
{
    public class Program
    {
        public static void Main(string[] args)
        {
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
            var lex = new Lexer("a = 3; if\na<2\n\nthen a = 0 else a = 23;\n print(a * 223 + 10) ");
            Token t = null;
            do
            {
                t = lex.NextToken();
                Console.WriteLine($"{t.pos},{t.type},{t.value}");
            } while (t.type != TokenType.Eof);
        }
    }
}