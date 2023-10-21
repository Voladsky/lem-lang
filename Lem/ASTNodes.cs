using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lem
{
    public class Node
    {
        public virtual T Visit<T>(IVisitor<T> v) => v.VisitNode(this);
    }
    public class ExprNode: Node
    {
        public override T Visit<T>(IVisitor<T> v) => v.VisitExprNode(this);
    }
    public class StatementNode: Node
    {
        public override T Visit<T>(IVisitor<T> v) => v.VisitStatementNode(this);
    }
    public class BinOpNode: ExprNode
    {
        public ExprNode Left, Right;
        public string Op;
        public BinOpNode(ExprNode left, ExprNode right, string op)
        {
            Left = left;
            Right = right;
            Op = op;
        }
        public override T Visit<T>(IVisitor<T> v) => v.VisitBinOp(this);
    }
    public class StatementListNode: StatementNode
    {
        public List<StatementNode> lst = new List<StatementNode>();
        public void Add(StatementNode st) => lst.Add(st);
        public override T Visit<T>(IVisitor<T> v) => v.VisitStatementList(this);
    }
    public class ExprListNode: Node
    {
        public List<ExprNode> lst = new List<ExprNode>();
        public void Add(ExprNode st) => lst.Add(st);
        public override T Visit<T>(IVisitor<T> v) => v.VisitExprList(this);
    }
    public class IntNode: ExprNode
    {
        public int Value;
        public IntNode(int value) => Value = value;
        public override T Visit<T>(IVisitor<T> v) => v.VisitInt(this);
    }
    public class DoubleNode: ExprNode
    {
        public double Value;
        public DoubleNode(double value) => Value = value;
        public override T Visit<T>(IVisitor<T> v) => v.VisitDouble(this);
    }
    public class IdNode: ExprNode
    {
        public string Name;
        public IdNode(string name) => Name = name;
        public override T Visit<T>(IVisitor<T> v) => v.VisitId(this);
    }
    public class AssignNode: StatementNode
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
    public class IfNode: StatementNode
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
    public class WhileNode: StatementNode
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
    public class ProcCallNode: StatementNode
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

    public class RangeNode: Node
    {
        public ExprNode LeftBorder;
        public ExprNode RightBorder;
        public RangeNode(ExprNode leftBorder, ExprNode rightBorder)
        {
            LeftBorder = leftBorder;
            RightBorder = rightBorder;
        }
        public override T Visit<T>(IVisitor<T> v) => v.VisitRangeNode(this);
    }
    public class ForNode: StatementNode
    {
        public IdNode IteratedId;
        public RangeNode Range;
        public StatementNode Stat;
        public ForNode(IdNode iteratedId, RangeNode range, StatementNode stat)
        {
            IteratedId = iteratedId;
            Range = range;
            Stat = stat;
        }
        public override T Visit<T>(IVisitor<T> v) => v.VisitForNode(this);
    }
}
