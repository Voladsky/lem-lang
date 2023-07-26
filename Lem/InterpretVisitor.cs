using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lem
{
    internal class InterpretVisitor : IVisitor<object>
    {
        public object VisitNode(Node bin) => null;
        public object VisitStatementNode(StatementNode st) => null;
        public object VisitExprNode(ExprNode bin) => null;
        public object VisitBinOp(BinOpNode bin)
        {
            var l = bin.Left.Visit(this);
            var r = bin.Right.Visit(this);
            object result;
            switch (bin.Op)
            {
                case "+":
                    result = (double)l + (double)r;
                    break;
                case "<":
                    result = (double)l < (double)r;
                    break;
                case "*":
                    result = (double)l * (double)r;
                    break;
                default:
                    throw new InvalidOperationException("Not implemented operator!");
            }
            return result;
        }
        public object VisitStatementList(StatementListNode stl)
        {
            foreach (var s in stl.lst)
                s.Visit(this);
            return null;
        }
        public object VisitExprList(ExprListNode exprList) => null;
        public object VisitInt(IntNode n) => n.Value;
        public object VisitDouble(DoubleNode d) => d.Value;
        public object VisitId(IdNode id) => SymTable.Table[id.Name];
        public object VisitAssign(AssignNode ass)
        {
            var val = ass.Expr.Visit(this);
            SymTable.Table[ass.Ident.Name] = val;
            return null;
        }
        public object VisitIf(IfNode ifn)
        {
            var cond = ifn.Condition.Visit(this);
            if ((bool)cond)
                ifn.ThenStat.Visit(this);
            else if (ifn.ElseStat != null)
                ifn.ElseStat.Visit(this);
            return null;
        }
        public object VisitWhile(WhileNode whn)
        {
            while ((bool)whn.Condition.Visit(this))
                whn.Stat.Visit(this);
            return null;
        }
        public object VisitProcCall(ProcCallNode f)
        {
            if (f.Name.Name.ToLower() == "print")
                Console.WriteLine(f.Pars.lst[0].Visit(this));
            return null;
        }
    }
}
