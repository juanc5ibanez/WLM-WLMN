using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WLM_WLMN.Common.DTOs;
using WLM_WLMN.Common.Interfaces;

namespace WLM_WLMN.Logic.Terms
{
    public class TermLogic : ITermLogic
    {
        private ITermStorage _termStorage;

        public TermLogic(ITermStorage termStorage)
        {
            _termStorage = termStorage;
        }

        public void CreateOrUpdate(Term termToSave)
        {
            if (termToSave.Id == Guid.Empty)
                termToSave.Id = Guid.NewGuid();
            if(termToSave.CreationDate == DateTime.MinValue)
                termToSave.CreationDate = DateTime.Now;
            _termStorage.CreateOrUpdate(termToSave);
        }

        public List<Term> GetTerms()
        {
            return _termStorage.GetTerms();
        }

        public Term GetTerm(Guid id)
        {
            return _termStorage.GetTerm(id);
        }

        public void DeleteTerm(Guid id)
        {
            _termStorage.DeleteTerm(id);
        }
    }
}
