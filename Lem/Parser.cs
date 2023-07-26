using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lem
{
    /*
    Program := StatementListNode
    StatementListNode := Statement (';' Statement)*
    Statement := Assign | ProcCall | IfStatement
    Assign := Id ('=' | '+=' | '-=' | '*=' | '/=') Expr 
    ProcCall := Id '(' ExprList ')
    Expr := Comp (CompOp Comp)*
    CompOp := '<' | '>' | '<=' | '>=' | '==' | '!='
    Comp := Term (AddOp Term)*
    AddOp := '+' | '-' | '||'
    Term := Factor (MultOp Factor)*
    MultOp := '*' | '/' | '&&'
    Factor := NUM | REALNUM | '(' Expr ') | FuncCall 
    FuncCall := Id '(' ExprList ')
    ExprList := Expr (',' Expr)*
    */

    // TODO : add pos to Nodes
    internal class Parser: ParserBase
    {
        public Parser(Lexer lex): base(lex) { }
        public StatementNode MainProgram()
        {
            current = 0;
            var res = StatementList();
            Requires(TokenType.Eof);
            return res;
        }
        StatementNode StatementList()
        {
            var stl = new StatementListNode();
            stl.Add(Statement());
            while (IsMatch(TokenType.Semicolon))
                stl.Add(Statement());
            return stl;
        }
        StatementNode Statement()
        {
            // id = expr id += expr
            // id(exprlist)
            // if expr then stat [else stat]
            // while expr do stat
            Check(new TokenType[] {TokenType.Id, TokenType.tkIf, TokenType.tkWhile, TokenType.LBrace});
            if (IsMatch(TokenType.tkIf))
            {
                var cond = Expr();
                Requires(TokenType.tkThen);
                var thenstat = Statement();
                var elsestat = IsMatch(TokenType.tkElse) ? Statement() : null;
                return new IfNode(cond, thenstat, elsestat);
            }
            else if (IsMatch(TokenType.tkWhile))
            {
                var cond = Expr();
                Requires(TokenType.tkDo);
                var stat = Statement();
                return new WhileNode(cond, stat);
            }
            else if (IsMatch(TokenType.LBrace))
            {
                StatementNode stl = StatementList();
                Requires(TokenType.RBrace);
                return stl;
            }
            var id = Ident();
            if (IsMatch(TokenType.Assign))
                return new AssignNode(id, Expr());
            // TODO : Add AssignPlus, AssignMinus etc.
            else if (IsMatch(TokenType.LPar))
            {
                var exlist = ExprList();
                var res = new ProcCallNode(id, exlist);
                Requires(TokenType.RPar);
                return res;
            }
            else throw new Exception();
        }
        ExprListNode ExprList()
        {
            var lst = new ExprListNode();
            lst.Add(Expr());
            while (IsMatch(TokenType.Comma))
                lst.Add(Expr());
            return lst;
        }
        // TODO : Assignment as AssignNode
        IdNode Ident()
        {
            var id = Requires(TokenType.Id);
            return new IdNode(id.value as string);
        }
        ExprNode Expr()
        {
            var ex = Comp();
            while (At(TokenType.Greater, TokenType.GreaterEqual, TokenType.Less, TokenType.LessEqual,
                TokenType.Equal, TokenType.NotEqual))
            {
                var op = NextLexem();
                var right = Comp();
                ex = new BinOpNode(ex, right, (string)op.value);
            }
            return ex;
        }
        ExprNode Comp()
        {
            var ex = Term();
            while (At(TokenType.Plus, TokenType.Minus, TokenType.tkOr))
            {
                var op = NextLexem();
                var right = Term();
                ex = new BinOpNode(ex, right, (string)op.value);
            }
            return ex;
        }
        ExprNode Term()
        {
            var ex = Factor();
            while (At(TokenType.Multiply, TokenType.Divide, TokenType.tkAnd))
            {
                var op = NextLexem();
                var right = Factor();
                ex = new BinOpNode(ex, right, (string)op.value);
            }
            return ex;
        }
        ExprNode Factor()
        {
            if (At(TokenType.Int))
                return new IntNode((int)NextLexem().value);
            if (At(TokenType.DoubleLiteral))
                return new DoubleNode((double)NextLexem().value);
            if (IsMatch(TokenType.LPar))
            {
                ExprNode res = Expr();
                Requires(TokenType.RPar);
                return res;
            }
            if (At(TokenType.Id))
            {
                return Ident();
                // TODO : Add function Nodes
                /*if (IsMatch(TokenType.LPar))
                {
                    var exlist = ExprList();
                    ProcCallNode res = new ProcCallNode(id, exlist);
                    Requires(TokenType.RPar);
                    return res;
                }*/
            }
            else throw new Exception();
        }
    }
}
