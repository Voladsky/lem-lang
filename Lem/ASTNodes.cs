using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lem
{
    internal class Node
    {
        public virtual T Visit<T>(IVisitor<T> v) => v.VisitNode(this);
    }
    internal class ExprNode: Node
    {
        public override T Visit<T>(IVisitor<T> v) => v.VisitExprNode(this);
    }
    internal class StatementNode: Node
    {
        public override T Visit<T>(IVisitor<T> v) => v.VisitStatementNode(this);
    }
    internal class BinOp: ExprNode
    {
        public ExprNode Left, Right;
        public string Op;
        public BinOp(ExprNode left, ExprNode right, string op)
        {
            Left = left;
            Right = right;
            Op = op;
        }
        public override T Visit<T>(IVisitor<T> v) => v.VisitBinOp(this);
    }
    internal class StatementList: StatementNode
    {
        public List<StatementNode> lst = new List<StatementNode>();
        public void Add(StatementNode st) => lst.Add(st);
        public override T Visit<T>(IVisitor<T> v) => v.VisitStatementList(this);
    }
    internal class ExprListNode: Node
    {
        public List<ExprNode> lst = new List<ExprNode>();
        public void Add(ExprNode st) => lst.Add(st);
        public override T Visit<T>(IVisitor<T> v) => v.VisitExprList(this);
    }
    internal class IntNode: ExprNode
    {
        public int Value;
        public IntNode(int value) => Value = value;
        public override T Visit<T>(IVisitor<T> v) => v.VisitInt(this);
    }
    internal class DoubleNode: ExprNode
    {
        public double Value;
        public DoubleNode(double value) => Value = value;
        public override T Visit<T>(IVisitor<T> v) => v.VisitDouble(this);
    }
    internal class IdNode: ExprNode
    {
        public string Name;
        public IdNode(string name) => Name = name;
        public override T Visit<T>(IVisitor<T> v) => v.VisitId(this);
    }
    internal class AssignNode: StatementNode
    {
        public IdNode Ident;
        public ExprNode Expr;
        public AssignNode(IdNode ident, ExprNode expr)
        {
            Ident = ident;
            Expr = expr;
        }
        public override T Visit<T>(IVisitor<T> v) => v.VisitAssign(this);
    }
    internal class IfNode: StatementNode
    {
        public ExprNode Condition;
        public StatementNode ThenStat, ElseStat;
        public IfNode(ExprNode condition, StatementNode thenStat, StatementNode elseStat)
        {
            Condition = condition;
            ThenStat = thenStat;
            ElseStat = elseStat;
        }
        public override T Visit<T>(IVisitor<T> v) => v.VisitIf(this);
    }
    internal class WhileNode: StatementNode
    {
        public ExprNode Condition;
        public StatementNode Stat;
        public WhileNode(ExprNode condition, StatementNode stat)
        {
            Condition = condition;
            Stat = stat;
        }
        public override T Visit<T>(IVisitor<T> v) => v.VisitWhile(this);
    }
    internal class ProcCallNode: StatementNode
    {
        public IdNode Name;
        public ExprListNode Pars;
        public ProcCallNode(IdNode name, ExprListNode pars)
        {
            Name = name;
            Pars = pars;
        }
        public override T Visit<T>(IVisitor<T> v) => v.VisitProcCall(this);
    }
    internal static class NodeCreation
    {
        public static BinOp Bin(ExprNode left, string op, ExprNode right) => new BinOp(left, right, op);
        public static AssignNode Ass(IdNode ident, ExprNode expr) => new AssignNode(ident, expr);
        public static IdNode Id(string name) => new IdNode(name);
        public static DoubleNode Num(double value) => new DoubleNode(value);
        public static IntNode Num(int value) => new IntNode(value);
        public static IfNode Iff(ExprNode cond, StatementNode th, StatementNode el) => new IfNode(cond, th, el);
        public static WhileNode Wh(ExprNode cond, StatementNode body) => new WhileNode(cond, body);
        public static StatementNode StL(params StatementNode[] ss)
        {
            var lst = new StatementList();
            lst.lst = ss.ToList();
            return lst;
        }
        public static ExprListNode ExL(params ExprNode[] ss)
        {
            var lst = new ExprListNode();
            lst.lst = ss.ToList();
            return lst;
        }
        public static ProcCallNode ProcCall(IdNode name, ExprListNode exlist) => new ProcCallNode(name, exlist);
    }
}
