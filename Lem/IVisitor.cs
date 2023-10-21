using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lem
{
    public interface IVisitor<T>
    {
        T VisitNode(Node bin);
        T VisitExprNode(ExprNode bin);
        T VisitStatementNode(StatementNode bin);
        T VisitBinOp(BinOpNode bin);
        T VisitStatementList(StatementListNode stl);
        T VisitExprList(ExprListNode exprlist);
        T VisitInt(IntNode n);
        T VisitDouble(DoubleNode n);
        T VisitId(IdNode id);
        T VisitAssign(AssignNode ass);
        T VisitIf(IfNode ifn);
        T VisitWhile(WhileNode whn);
        T VisitProcCall(ProcCallNode f);
        T VisitRangeNode(RangeNode r);
        T VisitForNode(ForNode forn);
    }
}
