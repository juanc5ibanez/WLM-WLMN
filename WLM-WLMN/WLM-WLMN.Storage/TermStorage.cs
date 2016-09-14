using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Cassandra;
using WLM_WLMN.Common.DTOs;
using WLM_WLMN.Common.Interfaces;

namespace WLM_WLMN.CassandraStorage
{
    public class TermStorage : ITermStorage
    {
        private ISession _session;

        public TermStorage()
        {
            _session = Common.Session;
            _session.Execute("CREATE TABLE IF NOT EXISTS terms( terms_id uuid ,content text,creation_Date timestamp, PRIMARY KEY(terms_id));");
        }

        public void CreateOrUpdate(Term termToSave)
        {
            var insertPreparation =_session.Prepare("insert into wlm.terms (terms_id ,content,creation_Date) VALUES(?,?,?)");
            var insertBinding = insertPreparation.Bind(termToSave.Id, termToSave.Text,termToSave.CreationDate);
            _session.Execute(insertBinding);
        }

        public List<Term> GetTerms()
        {
            List<Term> terms = new List<Term>();
            var result = _session.Execute("select * from wlm.terms;");
            foreach (Row row in result)
            {
                terms.Add(new Term()
                {
                    Id = row.GetValue<Guid>("terms_id"),
                    Text = row.GetValue<string>("content"),
                    CreationDate = row.GetValue<DateTime>("creation_date")
                });
            }
            return terms;
        }

        public Term GetTerm(Guid id)
        {
            var resultPrepare = _session.Prepare("select * from wlm.terms where terms_id = ?;");
            var resultBuild = resultPrepare.Bind(id);
            var result = _session.Execute(resultBuild).ToList();
            if (result.Any())
            {
                var row = result.First();
                return new Term()
                {
                    Id = row.GetValue<Guid>("terms_id"),
                    Text = row.GetValue<string>("content"),
                    CreationDate = row.GetValue<DateTime>("creation_date")
                };
            }
            else
            {
                return null;
            }
        }

        public void DeleteTerm(Guid id)
        {
            var resultPrepare = _session.Prepare("delete from wlm.terms where terms_id = ?;");
            var resultBuild = resultPrepare.Bind(id);
            _session.Execute(resultBuild).ToList();
        }
    }
}
