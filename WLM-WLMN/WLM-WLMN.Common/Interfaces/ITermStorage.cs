using System;
using System.Collections.Generic;
using WLM_WLMN.Common.DTOs;

namespace WLM_WLMN.Common.Interfaces
{
    public interface ITermStorage
    {
        void CreateOrUpdate(Term termToSave);
        List<Term> GetTerms();
        Term GetTerm(Guid id);
        void DeleteTerm(Guid id);
    }
}