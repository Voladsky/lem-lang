using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lem
{
    internal interface IVisitor<T>
    {
        T VisitNode(Node bin);
        T VisitExprNode(ExprNode bin);
        T VisitStatementNode(StatementNode bin);
        T VisitBinOp(BinOp bin);
        T VisitStatementList(StatementList stl);
        T VisitExprList(ExprListNode exprlist);
        T VisitInt(IntNode n);
        T VisitDouble(DoubleNode n);
        T VisitId(IdNode id);
        T VisitAssign(AssignNode ass);
        T VisitIf(IfNode ifn);
        T VisitWhile(WhileNode whn);
        T VisitProcCall(ProcCallNode f);

    }
}
